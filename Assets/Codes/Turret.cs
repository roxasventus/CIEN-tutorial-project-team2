using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public int prefabId;

    bool isLive;
    float timer = 0f;

    Animator anim;
    SpriteRenderer spriter;
    DamageFlash damageFlash;
    Trap trap;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        damageFlash = GetComponent<DamageFlash>();
        trap = GetComponentInChildren<Trap>();
    }

    private void OnEnable()
    {
        isLive = true;
        health = maxHealth;
        
            
        if (spriter.material.GetFloat("_FlashAmount") > 0)
            damageFlash.SetFlashAmount(0);
        
    }

    private void Update()
    {
        if (!GameManager.instance.isLive || !isLive)
            return;

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            return;

        timer += Time.deltaTime;
        if(timer >= trap.stopTime)
        {
            timer = 0f;
            Shoot();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive)
            return;

        health -= collision.GetComponent<Bullet>().damage;

        //damage flash effect
        damageFlash.CallDamageFlash();

        if(health <= 0)
        {
            isLive = false;
            anim.SetTrigger("Dead");
        }
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }

    private void Shoot()
    {
        float y = 1.24f;

        Vector3 targetPos = GameManager.instance.player.transform.position;
        Vector3 dir = targetPos - (transform.position + Vector3.up * y);
        dir = dir.normalized;

        Transform enemyBullet = GameManager.instance.pool.Get(prefabId).transform;
        enemyBullet.position = transform.position + Vector3.up * y;

        enemyBullet.rotation = Quaternion.FromToRotation(Vector3.left, dir);

        enemyBullet.GetComponent<EnemyBullet>().Init(0, dir); 
        
        //AudioManager.instance.PlaySfx(AudioManager.Sfx.attack1);
    }
}
