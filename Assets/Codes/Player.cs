using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
    public float slowPercent = 0f;
    public Scanner scanner;
    public Player player;

    private bool attracted = false;

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;
    DamageFlash damageFlash;

    Rigidbody2D attractingTarget;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        damageFlash = GetComponent<DamageFlash>();
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;
        if (GameManager.instance.health < 0)
        {
            inputVec = Vector2.zero;
        }
        else {
            inputVec.x = Input.GetAxisRaw("Horizontal");
            inputVec.y = Input.GetAxisRaw("Vertical");
        }
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        Vector2 nextVec = inputVec.normalized * speed * (1f - slowPercent) * Time.fixedDeltaTime;
        Vector2 dirVec = Vector2.zero;
        if (attracted && attractingTarget != null)
        {
            dirVec = attractingTarget.position - rigid.position;
            dirVec = dirVec.normalized * 1.5f * Time.fixedDeltaTime;
        }
        rigid.MovePosition(rigid.position + nextVec + dirVec);

    }

    private void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        anim.SetFloat("Speed", inputVec.magnitude); 
        
        if (inputVec.x != 0)
        {
            spriter.flipX = inputVec.x < 0;
        }
    }

    //for contact with enemy
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            if(collision.gameObject.GetComponent<Enemy>() != null)
                GetHit(collision.gameObject.GetComponent<Enemy>().contactDamage);
            else
                GetHit(collision.gameObject.GetComponent<Boss>().contactDamage);
        }
    }

    //for area damage or lazers
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }
       
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            GetHit(collision.gameObject.GetComponent<EnemyBullet>().damage);
        }
    }

    //for enemy bullets
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }

        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            GetHit(collision.gameObject.GetComponent<EnemyBullet>().damage);
        }
        else if (collision.gameObject.CompareTag("AttractField"))
        {
            attracted = true;
            attractingTarget = collision.GetComponent<Rigidbody2D>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }

        if (collision.gameObject.CompareTag("AttractField"))
        {
            attracted = false;
            attractingTarget = null;
        }
    }

    public void GetHit(int damage = 10)
    {
        if (GameManager.instance.isInvincible == false)
        {
            GameManager.instance.health -= Time.fixedDeltaTime * damage;
            //damage flash effect
            damageFlash.CallDamageFlash();
        }

        if (GameManager.instance.health < 0)
        {
            for (int index = 3; index < transform.childCount; index++)
            {
                transform.GetChild(index).gameObject.SetActive(false);
            }
            StartCoroutine(Dead());

        }
    }

    IEnumerator Dead()
    {
        
        rigid.bodyType = RigidbodyType2D.Kinematic;

        anim.SetTrigger("Dead");
        
        yield return new WaitForSeconds(4.2f);
        GameManager.instance.GameOver();

    }


}
