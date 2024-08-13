using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2_Atk : MonoBehaviour
{
    public float explodeWaitTime;
    public int explosionRingNum;
    public float spinDuration;

    public GameObject crack;
    public Transform thrownHammerPos;

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

    }

    public void CallAtk2()
    {
        StartCoroutine(Atk2());
    }


    IEnumerator Atk2()
    {
        yield return new WaitForSeconds(0.1f);
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

    public void MoveToHammer()
    {
        transform.position = thrownHammerPos.position + Vector3.up * 0.7f;
    }

    //used as animation event. Make cracks when hammer hits the ground
    public void CallGroundCrack()
    {
        StartCoroutine (GroundCrack());
    }

    IEnumerator GroundCrack()
    {
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

            yield return new WaitForSeconds(0.35f);
        }

        yield return new WaitForSeconds(explodeWaitTime + 0.3f);

        anim.SetTrigger("Atk1_End");
        transform.position = thrownHammerPos.position + Vector3.up * 0.7f;

        StartMoving();
    }
}
