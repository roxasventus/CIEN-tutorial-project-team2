using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public int contactDamage = 10;
    public RuntimeAnimatorController[] animCon1; //cannot serialize 2d array so I just made 3 variables.
    public RuntimeAnimatorController[] animCon2;
    public RuntimeAnimatorController[] animCon3;
    public Rigidbody2D target;

    bool isLive;

    Rigidbody2D rigid;
    Collider2D coll;
    Animator anim;
    SpriteRenderer spriter;
    Transform shadow;
    Transform wallColl;
    WaitForFixedUpdate wait;
    DamageFlash damageFlash;

    public GameObject[] dropItems;
    public float percentage;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        shadow = GetComponentsInChildren<Transform>()[1];
        wallColl = GetComponentsInChildren<Transform>()[2];
        damageFlash = GetComponent<DamageFlash>();
        wait = new WaitForFixedUpdate();
    }

    private void Update()
    {
        if (!GameManager.instance.isLive || !isLive)
            return;

        if (target.position.x < rigid.position.x)
        {
            shadow.localPosition = new Vector3(0.1f, -0.7f, 0);
            wallColl.localPosition = new Vector3(0.1f, -0.7f, 0);
        }
        else
        {
            shadow.localPosition = new Vector3(-0.1f, -0.7f, 0);
            wallColl.localPosition = new Vector3(-0.1f, -0.7f, 0);
        }
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;
        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;
        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }

    void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;
        if (!isLive) return;

        spriter.flipX = target.position.x < rigid.position.x;
    }

    void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        health = maxHealth;
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 1;
        anim.SetBool("Dead", false);
    }

    public void Init(SpawnData data)
    {
        if (anim == null)
        {
            return;
        }

        switch (data.spriteType)
        {
            case 0:
                anim.runtimeAnimatorController = animCon1[Random.Range(0, 3)];
                break;
            case 1:
                anim.runtimeAnimatorController = animCon2[Random.Range(0, 3)];
                break;
            default:
                anim.runtimeAnimatorController = animCon3[Random.Range(0, 3)];
                break;
        }
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive)
            return;

        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack());

        //damage flash effect
        damageFlash.CallDamageFlash();

        if (health > 0)
        {
            anim.SetTrigger("Hit");
            // 효과음 재생할 부분마다 재생함수 호출
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);

        }
        else
        {
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            spriter.sortingOrder = 1;
            anim.SetBool("Dead", true);
            GameManager.instance.kill++;
            if(!GameManager.instance.enemyCleaner.activeSelf)
                GameManager.instance.GetExp();

            if (Random.Range(0.0f, 1.0f) <= percentage) {
                if (collision.gameObject.name != "EnemyCleaner")
                    Instantiate(dropItems[Random.Range(0, 2)], transform.position, Quaternion.identity);
            }
            // 효과음 재생할 부분마다 재생함수 호출
            if (GameManager.instance.isLive)
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);

        }
    }

    IEnumerator KnockBack()
    {
        yield return wait; //delay for the next fixed frame
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }
}
