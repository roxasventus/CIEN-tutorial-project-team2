using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondSliceEffect : MonoBehaviour
{
    SpriteRenderer spriter;
    Animator anim;
    Boss3_Atk bossAtk;
    DamageFlash damageFlash;

    private void Awake()
    {
        spriter = GetComponent<SpriteRenderer>(); 
        anim = GetComponent<Animator>();
        bossAtk = GetComponentInParent<Boss3_Atk>();
        damageFlash = GetComponent<DamageFlash>();
    }

    private void OnEnable()
    {
        transform.position = GameManager.instance.player.transform.position;
        Vector3 dir = Random.insideUnitCircle.normalized;
        transform.rotation = Quaternion.FromToRotation(Vector3.left, dir);
    }

    public void CallTargetNShoot()
    {
        StartCoroutine(TargetNShoot());
    }

    IEnumerator TargetNShoot()
    {
        damageFlash.CallDamageFlash();
        yield return new WaitForSeconds(bossAtk.dashWaitTime - 0.8f);

        anim.SetTrigger("Shoot");
        gameObject.tag = "EnemyBullet";
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Boss3Atk1Slice);
    }
    public void Dead()
    {
        Destroy(gameObject);
    }
}
