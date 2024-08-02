using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
    public Scanner scanner;
    public Player player;

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;
    DamageFlash damageFlash;

    // 시작할 때 한번만 실행되는 생명주기 Awake
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

    // 물리 연산 프레임마다 호출되는 생명주기 FixedUpdate
    void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        // 단위벡터로 만들어야 플레이어가 지나치게 빨리 움직이는 것을 막을 수 있다
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime; // Time.fixedDeltaTime: 물리 프레임 하나가 소비한 시간
        rigid.MovePosition(rigid.position + nextVec); // rigid.position: 현재 위치 
    }

    // 프레임이 종료 되기 전 실행되는 생명주기 함수
    private void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        // 애니메이터에서 설정한 파라메터 타입과 동일한 함수 작성
        anim.SetFloat("Speed", inputVec.magnitude); // (파라메터 이름, 반영할 float 값) // magnitude: 벡터의 순수한 크기 값
        // 스프라이트 방향
        if (inputVec.x != 0)
        {
            spriter.flipX = inputVec.x < 0;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (GameManager.instance.isInvincible == false)
            { 
            GameManager.instance.health -= Time.fixedDeltaTime * 10;
            //damage flash effect
            damageFlash.CallDamageFlash();
            }

            if (GameManager.instance.health < 0)
            {
                for (int index = 2; index < transform.childCount; index++)
                {
                    transform.GetChild(index).gameObject.SetActive(false);
                }
                StartCoroutine(Dead());

            }
        }
    }

    IEnumerator Dead()
    {
        // 죽을 때 밀리지 않도록 
        rigid.bodyType = RigidbodyType2D.Kinematic;

        anim.SetTrigger("Dead");
        // duration 동안 대기
        yield return new WaitForSeconds(4.2f);
        GameManager.instance.GameOver();

    }


}
