using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Boss2_Atk : MonoBehaviour
{
    [Header("Atk1")]
    public float explodeWaitTime;
    public int explosionRingNum;
    public GameObject crack;
    public Transform thrownHammerPos;

    [Header("Atk2")]
    public float spinDuration;
    public float spinWaitTime;
    public float shootInterval;
    public GameObject spinBlade;

    private float originSpeed;

    Boss boss;
    SpriteRenderer sprite;
    Animator anim;
    Collider2D[] colls;

    List<GameObject[]> rings;

    private void Awake()
    {
        boss = GetComponent<Boss>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        colls = GetComponentsInChildren<Collider2D>();
        rings = new List<GameObject[]>();
    }

    public void CallAtk1()
    {
        originSpeed = boss.speed;
        boss.speed = 0;
        boss.isAttacking = true;
        foreach (Collider2D coll in colls)
            coll.enabled = false;

        anim.SetTrigger("Atk1");
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Boss2Atk1Jump);
    }

    public void CallAtk2()
    {
        StartCoroutine(Atk2());
    }


    IEnumerator Atk2()
    {
        originSpeed = boss.speed;
        boss.speed = 0;
        GetComponent<Rigidbody2D>().isKinematic = true;
        boss.isAttacking = true;
        anim.SetTrigger("Atk2");
        Instantiate(spinBlade, transform).GetComponent<Spinner>().CallSpinNShoot();
        
        yield return new WaitForSeconds(spinWaitTime);

        anim.SetTrigger("Spin");

        yield return new WaitForSeconds(spinDuration);

        anim.SetTrigger("EndSpin");
    }

    //used as animation event
    public void StartMoving()
    {
        boss.speed = originSpeed;
        boss.isAttacking = false;
        GetComponent<Rigidbody2D>().isKinematic = false;
        foreach (Collider2D coll in colls)
            coll.enabled = true;
    }

    //used as animation event. Make cracks when hammer hits the ground
    public void CallGroundCrack()
    {
        StartCoroutine (GroundCrack());
    }

    IEnumerator GroundCrack()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Boss2Atk1Hit);

        Instantiate(crack, thrownHammerPos).GetComponent<Crack>().CallCrackNBomb();

        for(int i = 0; i < explosionRingNum; i++)
        {
            GameObject[] ring = new GameObject[8];
            for(int j = 0; j < ring.Length; j++)
            {
                ring[j] = Instantiate(crack, thrownHammerPos);
                
                Vector3 rotVec = Vector3.forward * 45 * j;
                ring[j].transform.Rotate(rotVec);
                ring[j].transform.Translate(ring[j].transform.up * 0.95f * (i + 1), Space.World);
                ring[j].GetComponent<Crack>().CallCrackNBomb();
            }
            
            rings.Add(ring);

            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(explodeWaitTime + 0.4f);

        anim.SetTrigger("Atk1_End");
        transform.position = thrownHammerPos.position + Vector3.up * 0.7f;

        StartMoving();
    }

    public void MoveShadow()
    {
        GetComponentsInChildren<Transform>()[1].position = thrownHammerPos.position;
        GetComponentsInChildren<Transform>()[2].position = thrownHammerPos.position;
    }
}
