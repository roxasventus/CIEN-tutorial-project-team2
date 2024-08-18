using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UIElements;

public class Boss3_Atk : MonoBehaviour
{
    public Vector2 targetDir;
    public bool boss3Atk1;

    [Header("Atk1")]
    public int dashNum;
    public float dashWaitTime;
    public Transform sliceEffect;
    public Transform sliceTarget;
    public GameObject secondSliceEffect;
    public int secondSliceNum;

    [Header("Atk2")]
    public int teleportNum;
    public float shootWaitTime;
    public int bulletNum;
    public Transform teleportFrom;
    public Transform teleportTo;

    private float originSpeed;
    private Vector3 teleportPos;

    Boss boss;
    SpriteRenderer spriter;
    Animator anim;
    Collider2D coll; 
    DamageFlash damageFlash;
    Transform[] timerCircles;

    private void Awake()
    {
        boss = GetComponent<Boss>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        timerCircles = new Transform[2];
        Init();
    }

    private void Init()
    {
        damageFlash = sliceEffect.GetComponent<DamageFlash>();
        timerCircles[0] = teleportFrom.GetComponentsInChildren<Transform>()[1];
        timerCircles[1] = teleportTo.GetComponentsInChildren<Transform>()[1];
    }

    public void CallAtk1()
    {
        StartCoroutine(Atk1());
    }

    IEnumerator Atk1()
    {
        originSpeed = boss.speed;
        boss.speed = 0;
        GetComponent<Rigidbody2D>().isKinematic = true;
        coll.enabled = false;

        for (int i = 0; i < dashNum; i++)
        {
            boss.isAttacking = false;
            boss3Atk1 = true;
            anim.SetTrigger("Atk1");
            float timer = 0;
            sliceTarget.gameObject.SetActive(true);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Boss3Atk1Target);

            while (timer <= dashWaitTime - 1f)
            {
                timer += Time.fixedDeltaTime;

                targetDir = GameManager.instance.player.GetComponent<Rigidbody2D>().position - sliceTarget.GetComponent<Rigidbody2D>().position;
                float angle = Mathf.Atan2(targetDir.y, Mathf.Abs(targetDir.x)) * Mathf.Rad2Deg;

                if (spriter.flipX)
                {
                    angle = 180f - angle;

                    if (angle < 90f)
                        angle = 90f;
                    else if (angle > 270f)
                        angle = 270f;
                }
                else
                {
                    if (angle > 90f)
                        angle = 90f;
                    else if (angle < -90f)
                        angle = -90f;
                }

                sliceTarget.GetComponent<Rigidbody2D>().SetRotation(angle);

                yield return new WaitForFixedUpdate();
            }

            //stop tracking player for last 1 second
            boss.isAttacking = true;
            yield return new WaitForSeconds(1f);

            //Slice
            sliceTarget.gameObject.SetActive(false);
            sliceEffect.gameObject.SetActive(true);
            sliceEffect.position = transform.position + Vector3.up * 0.3f;
            sliceEffect.rotation = Quaternion.FromToRotation(Vector3.right, (targetDir + Vector2.down * 0.7f).normalized);
            anim.SetBool("Slice", true);
            sliceEffect.gameObject.tag = "EnemyBullet";
            damageFlash.CallDamageFlash();

            AudioManager.instance.PlaySfx(AudioManager.Sfx.Boss3Atk1Slice);

            yield return new WaitForSeconds(0.25f);

            //Untag and disable effect
            anim.SetBool("Slice", false);
            sliceEffect.gameObject.tag = "Untagged";
            sliceEffect.gameObject.SetActive(false);

            //Instantiate second slice effects
            for (int j = 0; j < secondSliceNum; j++)
            {
                Instantiate(secondSliceEffect, transform).GetComponent<SecondSliceEffect>().CallTargetNShoot();
            }
            yield return new WaitForSeconds(dashWaitTime - 0.8f);
            //recalibrate facing direction and effect position
            boss.isAttacking = false;
            yield return new WaitForNextFrameUnit();
            boss.isAttacking = true;
        }
        anim.SetTrigger("Atk1End");
        boss3Atk1 = false;
        StartMoving();
    }

    public void CallAtk2()
    {
        StartCoroutine(Atk2());
    }

    IEnumerator Atk2()
    {
        originSpeed = boss.speed;
        boss.speed = 0;
        boss.isAttacking = true;
        GetComponent<Rigidbody2D>().isKinematic = true;
        anim.SetBool("Shoot", false);
        anim.SetTrigger("Atk2");

        for(int i = 0; i < teleportNum; i++)
        {
           

            //find teleport position
            teleportPos = GetTeleportPoint();

            //show indicator for few seconds before teleport
            teleportFrom.position = transform.position + Vector3.down * 0.7f;
            teleportTo.position = teleportPos;

            teleportFrom.gameObject.SetActive(true);
            teleportTo.gameObject.SetActive(true);
            teleportFrom.GetComponent<SpriteRenderer>().sortingOrder = 0;
            teleportTo.GetComponent<SpriteRenderer>().sortingOrder = 0;

            timerCircles[0].gameObject.SetActive(true);
            timerCircles[0].localScale = Vector3.zero;
            timerCircles[1].gameObject.SetActive(true);
            timerCircles[1].localScale = Vector3.zero;

            for (float j = 0; j < 0.9f; j += 0.1f)
            {
                timerCircles[0].localScale = Vector3.one * (j / 1f) * 1.06f;
                timerCircles[1].localScale = Vector3.one * (j / 1f) * 1.06f;
                yield return new WaitForSeconds(0.1f);
            }

            //teleport and prepare to attack
            anim.SetBool("Shoot", false);
            teleportFrom.GetComponent<SpriteRenderer>().enabled = false;
            teleportTo.GetComponent<SpriteRenderer>().enabled = false;
            transform.position = teleportPos + Vector3.up * 0.7f;

            RecalibrateIndicators();
            anim.SetTrigger("Atk2Wait");

            AudioManager.instance.PlaySfx(AudioManager.Sfx.Boss3Atk2Teleport);

            boss.isAttacking = false;
            yield return new WaitForSeconds(0.1f);
            boss.isAttacking = true;

            yield return new WaitForSeconds(shootWaitTime);

            //shoot bullets
            anim.SetBool("Shoot", true);
            MultiShoot();

            
        }
        anim.SetTrigger("Atk2End");
        
        StartMoving();
    }

    private void RecalibrateIndicators()
    {
        Vector3 teleportDis;

        teleportDis = teleportFrom.position - teleportTo.position;
        teleportFrom.position += teleportDis;
        teleportTo.position += teleportDis;

        teleportFrom.GetComponent<SpriteRenderer>().sortingOrder = 2;
        teleportTo.GetComponent<SpriteRenderer>().sortingOrder = 2;
        teleportFrom.GetComponent<Animator>().SetTrigger("Teleport");
        teleportTo.GetComponent<Animator>().SetTrigger("Teleport");

        timerCircles[0].gameObject.SetActive(false);
        timerCircles[1].gameObject.SetActive(false);

        teleportFrom.GetComponent<SpriteRenderer>().enabled = true;
        teleportTo.GetComponent<SpriteRenderer>().enabled = true;
    }

    private Vector3 GetTeleportPoint()
    {
        Vector3 reposPoint = Vector3.zero;

        int layerToNotSpawnOn = LayerMask.NameToLayer("Wall");
        bool isReposValid = false;

        //find valid reposition position
        while (!isReposValid)
        {
            Vector3 ranPoint = new Vector3(
                UnityEngine.Random.Range(-8.6f, 8.6f),
                UnityEngine.Random.Range(-4.6f, 4.6f),
                0
                );
            reposPoint = GameManager.instance.player.transform.position + ranPoint;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(reposPoint, 0.8f);

            bool invalidColl = false;

            //find collision with walls
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject.layer == layerToNotSpawnOn)
                {
                    invalidColl = true;
                    break;
                }
            }

            if (!invalidColl)
            {
                isReposValid = true;
            }
        }

        return reposPoint;
    }

    private void MultiShoot()
    {
        Vector3 baseDir = (GameManager.instance.player.transform.position - transform.position).normalized;

        for(int i = 0; i < bulletNum; i++)
        {
            Vector3 rotVec = Vector3.forward * (180 / bulletNum) * i;

            Transform enemyBullet = GameManager.instance.pool.Get(10).transform;
            enemyBullet.position = transform.position;

            Vector3 targetDir = Quaternion.AngleAxis(-90f + (180 / bulletNum) * i, Vector3.forward) * baseDir;
            targetDir = targetDir.normalized;

            enemyBullet.rotation = Quaternion.FromToRotation(Vector3.left, targetDir);

            enemyBullet.GetComponent<EnemyBullet>().Init(0, targetDir);

            AudioManager.instance.PlaySfx(AudioManager.Sfx.Boss3Atk2Shoot);
        }
    }

    public void StartMoving()
    {
        boss.speed = originSpeed;
        boss.isAttacking = false;
        GetComponent<Rigidbody2D>().isKinematic = false;
        coll.enabled = true;
    }
}
