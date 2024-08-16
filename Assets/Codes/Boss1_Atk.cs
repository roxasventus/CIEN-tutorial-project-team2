using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Boss1_Atk : MonoBehaviour
{
    [Header("Atk1")]
    public float shootWaitTime;
    public GameObject beamParent;

    [Header("Atk2")]
    public float explodeWaitTime;
    public int bombNum;
    public GameObject bomb;

    private Vector2 dir;
    private float originSpeed;

    Boss boss;
    SpriteRenderer sprite;
    Animator anim;
    Transform[] beams;
    Rigidbody2D beamRigid;

    private void Awake()
    {
        boss = GetComponent<Boss>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        beams = beamParent.GetComponentsInChildren<Transform>(true);
        beamRigid = beamParent.GetComponentInChildren<Rigidbody2D>(true);
    }

    public void CallAtk1()
    {
        StartCoroutine(Atk1());
    }

    public void CallAtk2()
    {
        originSpeed = boss.speed;

        boss.speed = 0f;
        boss.isAttacking = true;
        GetComponent<Rigidbody2D>().isKinematic = true;
        anim.SetTrigger("Atk2");

        StartCoroutine(SpawnBomb());
    }

    private IEnumerator Atk1()
    {
        float timer = 0f;
        originSpeed = boss.speed;

        Vector3 targetPos;
        float targetAngle;

        boss.speed = 0f;
        boss.isAttacking = true;
        GetComponent<Rigidbody2D>().isKinematic = true;

        beamParent.SetActive(true);

        //change the starting position and rotation according to sprite.flipX
        if (sprite.flipX)
        {
            targetPos = new Vector3(-0.823f, 0.25f, 0f);
            targetAngle = 180f;
        }
        else
        {
            targetPos = new Vector3(0.823f, 0.25f, 0f);
            targetAngle = 0f;
        }

        for(int i = 1; i < beams.Length; i++)
        {
            beams[i].localPosition = targetPos;
            beams[i].rotation = Quaternion.Euler(0f, 0f, beams[i].rotation.z + targetAngle);

        }
        
        
        
        //targeting player

        anim.SetTrigger("Atk1");
        while(timer <= shootWaitTime - 1f)
        {
            timer += Time.fixedDeltaTime;

            dir = GameManager.instance.player.GetComponent<Rigidbody2D>().position - beamRigid.position;
            float angle = Mathf.Atan2(dir.y, Mathf.Abs(dir.x)) * Mathf.Rad2Deg;

            //set limit to the angle
            
            if (sprite.flipX)
            {
                angle = 180f - angle;

                if (angle < 90f)
                    angle = 90f;
                else if (angle > 270f)
                    angle = 270f;
            }
            else{
                if (angle > 90f)
                    angle = 90f;
                else if (angle < -90f)
                    angle = -90f;
            }


            beamRigid.SetRotation(angle);

            yield return new WaitForFixedUpdate();
        }

        //stop tracking player for last 1 second
        yield return new WaitForSeconds(1f);

        //Shoot
        anim.SetTrigger("Atk1_Shoot");
        for(int i = 1; i < beams.Length; i++)
        {
            beams[i].GetComponent<Animator>().SetTrigger("Shoot");
            beams[i].gameObject.tag = "EnemyBullet";
        }
        yield return new WaitForSeconds(0.6f);

        //Untag and disable beams
        for (int i = 1; i < beams.Length; i++)
        {
            beams[i].gameObject.tag = "Untagged";
        }
       
        beamParent.SetActive(false);

        boss.speed = originSpeed;
        boss.isAttacking = false;
        GetComponent<Rigidbody2D>().isKinematic = false;
    }

    private IEnumerator SpawnBomb()
    {
        for (int i = 0; i < bombNum; i++)
        {
            GameObject o = Instantiate(bomb, transform);
            o.transform.position = GameManager.instance.player.transform.position;
            o.GetComponent<Bombing>().CallTargetNBomb();
            yield return new WaitForSeconds(0.7f);
        }
    }

    //used as animation event
    public void StartMoving()
    {
        boss.speed = originSpeed;
        boss.isAttacking = false;
        GetComponent<Rigidbody2D>().isKinematic = false;
    }
}
