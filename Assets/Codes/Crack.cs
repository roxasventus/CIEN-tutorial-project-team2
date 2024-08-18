using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crack : MonoBehaviour
{
    Animator anim;
    Boss2_Atk bossAtk;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        bossAtk = transform.parent.GetComponentInParent<Boss2_Atk>();
    }

    public void CallCrackNBomb()
    {
        StartCoroutine(CrackNBomb());
    }

    IEnumerator CrackNBomb()
    {

        yield return new WaitForSeconds(bossAtk.explodeWaitTime - 0.4f);

        //Open the crack for last 0.3 seconds
        anim.SetTrigger("Open");
        yield return new WaitForSeconds(0.4f);

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
