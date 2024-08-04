using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public enum TrapType { Beartrap, Turret, Spikes, Lava}
    public TrapType type;
    public float stopTime;

    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("CharBase"))
            return;

        switch (type)
        {
            case TrapType.Beartrap:
                StartCoroutine(BeartrapAttack());
                break;
            case TrapType.Spikes:
                if(anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                    StartCoroutine(SpikeAttack());
                break;
        }
        
    }

    IEnumerator BeartrapAttack()
    {
        GetComponent<SpringJoint2D>().connectedBody = GameManager.instance.player.GetComponentsInChildren<Rigidbody2D>()[1];
        anim.SetTrigger("Catch");
        yield return new WaitForSeconds(stopTime);
        gameObject.SetActive(false);
    }

    IEnumerator SpikeAttack()
    {
        anim.SetTrigger("Detect");
        yield return new WaitForSeconds(stopTime);
        anim.SetTrigger("Attack");
        gameObject.tag = "EnemyBullet";
    }

    public void Finish()
    {
        gameObject.tag = "Untagged";
    }

    private void OnDisable()
    {
        if (type != TrapType.Beartrap)
            return;

        GetComponent<SpringJoint2D>().connectedBody = null;
    }
}
