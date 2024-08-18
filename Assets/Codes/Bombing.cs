using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bombing : MonoBehaviour
{
    Transform timerCircle;
    Animator anim;
    Boss1_Atk bossAtk;

    private void Awake()
    {
        timerCircle = GetComponentsInChildren<Transform>()[1];
        anim = GetComponent<Animator>();
        bossAtk = GetComponentInParent<Boss1_Atk>();
    }

    public void CallTargetNBomb()
    {
        StartCoroutine(TargetNBomb());
    }

    IEnumerator TargetNBomb()
    {
        for(float i = 0; i < bossAtk.explodeWaitTime; i += 0.1f)
        {
            timerCircle.localScale = Vector3.one * (i / bossAtk.explodeWaitTime) * 1.31f;
            yield return new WaitForSeconds(0.1f);
        }

        anim.SetTrigger("Boom");
        tag = "Trap";

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Explosion);

        yield return new WaitForSeconds(0.5f);
        tag = "Untagged";
    }

    public void Dead()
    {
        Destroy(gameObject);
    }
}
