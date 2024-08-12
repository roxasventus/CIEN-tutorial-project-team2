using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bombing : MonoBehaviour
{
    Transform timerCircle;
    Animator anim;
    Boss boss;

    private void Awake()
    {
        timerCircle = GetComponentsInChildren<Transform>()[1];
        anim = GetComponent<Animator>();
        boss = GetComponentInParent<Boss>();
    }

    public void CallTargetNBomb()
    {
        StartCoroutine(TargetNBomb());
    }

    IEnumerator TargetNBomb()
    {
        for(float i = 0; i < boss.atk2WaitTime; i += 0.1f)
        {
            timerCircle.localScale = Vector3.one * (i / boss.atk2WaitTime) * 1.31f;
            yield return new WaitForSeconds(0.1f);
        }

        anim.SetTrigger("Boom");
        tag = "Trap";

        yield return new WaitForSeconds(0.5f);
        tag = "Untagged";
    }

    public void Dead()
    {
        Destroy(gameObject);
    }
}
