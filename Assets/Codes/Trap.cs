using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public enum TrapType { Beartrap, Turret, Spikes, Lava}
    public TrapType type;

    [Header("Beartrap, Turret, Spikes")]
    public float stopTime;

    [Header("Lava")]
    public float slowPercentage;
    public float damageTime;

    private float timer = 0f;

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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("CharBase"))
            return;

        switch (type)
        {
            case TrapType.Lava:
               
                GameManager.instance.player.slowPercent = slowPercentage;
                
                
                
                break;
            case TrapType.Turret:
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
