using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public Rigidbody2D target;
    public bool isAttacking = false;

    [Header("Boss Settings")]
    public int bossNum;
    public float atkTimer;

    [Header("Boss2")]
    public Transform thrownHammerPos;

    bool isLive;
    float timer;
    private int patternNum;

    Rigidbody2D rigid;
    Collider2D coll;
    Animator anim;
    SpriteRenderer spriter;
    Transform shadow;
    Transform wallColl;
    DamageFlash damageFlash;

    Boss1_Atk boss1;
    Boss2_Atk boss2;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        shadow = GetComponentsInChildren<Transform>()[1];
        wallColl = GetComponentsInChildren<Transform>()[2];
        damageFlash = GetComponent<DamageFlash>();

        if(bossNum == 0)
        {
            boss1 = GetComponent<Boss1_Atk>();
        }
        else if(bossNum == 1)
        {
            boss2 = GetComponent<Boss2_Atk>();
        }
    }

    private void Update()
    {
        if (!GameManager.instance.isLive || !isLive)
            return;

        //prevent the boss from flipping while attacking
        if (!isAttacking)
        {
            if (target.position.x < rigid.position.x)
            {
                shadow.localPosition = new Vector3(0.1f, -0.7f, 0);
                wallColl.localPosition = new Vector3(0.1f, -0.7f, 0);
                if(bossNum == 1)
                    thrownHammerPos.localPosition = new Vector3(-3.9f, -0.7f, 0);
            }
            else
            {
                shadow.localPosition = new Vector3(-0.1f, -0.7f, 0);
                wallColl.localPosition = new Vector3(-0.1f, -0.7f, 0);
                if (bossNum == 1)
                    thrownHammerPos.localPosition = new Vector3(3.9f, -0.7f, 0);
            }
        }
       
        timer += Time.deltaTime;

        //do random attack pattern
        if(timer > atkTimer)
        {
            timer = 0f;
            switch (bossNum)
            {
                case 0:
                    patternNum = Random.Range(0, 2);
                    //patternNum = 1;

                    if (patternNum == 0)
                        boss1.CallAtk1();
                    else if (patternNum == 1)
                        boss1.CallAtk2();

                    break;
                case 1:
                    //patternNum = Random.Range(0, 2);
                    patternNum = 0;

                    if (patternNum == 0)
                        boss2.CallAtk1();
                    else if (patternNum == 1)
                        boss2.CallAtk2();
                    break;
                case 2:
                    break;
            }
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
        if (!isLive || isAttacking) return; //prevent the boss from flipping while attacking

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

        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive)
            return;

        health -= collision.GetComponent<Bullet>().damage;

        //damage flash effect
        damageFlash.CallDamageFlash();

        if (health > 0)
        {
            if(!isAttacking)
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
            GameManager.instance.GetExp();

            // 효과음 재생할 부분마다 재생함수 호출
            if (GameManager.instance.isLive)
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);

        }
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }

}
