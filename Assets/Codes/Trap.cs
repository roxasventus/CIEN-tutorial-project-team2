using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public enum TrapType { Beartrap, Turret, Spikes, Lava, Boss}
    public TrapType type;
    public int damage;

    [Header("Beartrap, Turret, Spikes")]
    public float stopTime;

    [Header("Lava")]
    public float slowPercentage;

    Animator anim;

    private void Awake()
    {
        if(type == TrapType.Turret)
        {
            anim = GetComponentInParent<Animator>();  
        }
        else
        {
            anim = GetComponent<Animator>();
        }
       
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
            case TrapType.Turret:
                anim.SetBool("Attack", true);
                break;
            default:
                break;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("CharBase"))
            return;

        switch (type)
        {
            case TrapType.Lava:
               
                GameManager.instance.player.slowPercent = slowPercentage;
                
                break;

            default:
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("CharBase"))
            return;

        switch (type)
        {
            case TrapType.Lava:
                GameManager.instance.player.slowPercent = 0f;
                break;
            case TrapType.Turret:
                anim.SetBool("Attack", false);
                break;
            default:
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
        gameObject.tag = "Trap";
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
