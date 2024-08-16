using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class Boss3_Atk : MonoBehaviour
{
    public Vector2 targetDir;
    public bool boss3Atk1;
    [Header("Atk1")]
    public int dashNum;
    public float dashWaitTime;
    public GameObject secondSliceEffect;
    public int secondSliceNum;
    private float originSpeed;

    Boss boss;
    SpriteRenderer spriter;
    Animator anim;
    Collider2D coll; 
    Transform sliceEffect;
    Transform sliceTarget;
    DamageFlash damageFlash;
    private void Awake()
    {
        boss = GetComponent<Boss>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        sliceEffect = GetComponentsInChildren<Transform>(true)[3];
        sliceTarget = GetComponentsInChildren<Transform>(true)[4];
        InitDamageFlash();
    }

    private void InitDamageFlash()
    {
        damageFlash = sliceEffect.GetComponent<DamageFlash>();
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

    public void StartMoving()
    {
        boss.speed = originSpeed;
        boss.isAttacking = false;
        GetComponent<Rigidbody2D>().isKinematic = false;
        coll.enabled = true;
    }
}
