using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{

    public float health;
    public float maxHealth;
    bool isLive;

    Collider2D coll;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait;
    DamageFlash damageFlash;

    public GameObject[] dropItems;
    public float percentage;

    void Awake()
    {
        coll = GetComponent<Collider2D>();
        spriter = GetComponent<SpriteRenderer>();
        damageFlash = GetComponent<DamageFlash>();
        wait = new WaitForFixedUpdate();
    }



    void OnEnable()
    {
        isLive = true;
        health = maxHealth;
        coll.enabled = true;
        spriter.sortingOrder = 1;

        if(spriter.material.GetFloat("_FlashAmount") > 0)
            damageFlash.SetFlashAmount(0);
    }

    public void Init(SpawnData data)
    {
        maxHealth = data.health;
        health = data.health;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive)
          return;

        health -= collision.GetComponent<Bullet>().damage;

        //damage flash effect
        damageFlash.CallDamageFlash();

        if (health > 0)
        {
            // 효과음 재생할 부분마다 재생함수 호출
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);

        }
        else
        {
            isLive = false;
            coll.enabled = false;
            spriter.sortingOrder = 1;


            if (Random.Range(0.0f, 1.0f) <= percentage)
            {
                Instantiate(dropItems[Random.Range(0, 2)], transform.position, Quaternion.identity);
            }

            // 효과음 재생할 부분마다 재생함수 호출
            if (GameManager.instance.isLive)
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);

            Dead();

        }
    }


    void Dead()
    {
        gameObject.SetActive(false);
    }
}
