using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
    public Scanner scanner;

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;

    // 시작할 때 한번만 실행되는 생명주기 Awake
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
    }

    void Update()
    {
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
    }

    // 물리 연산 프레임마다 호출되는 생명주기 FixedUpdate
    void FixedUpdate()
    {
        // 단위벡터로 만들어야 플레이어가 지나치게 빨리 움직이는 것을 막을 수 있다
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime; // Time.fixedDeltaTime: 물리 프레임 하나가 소비한 시간
        rigid.MovePosition(rigid.position + nextVec); // rigid.position: 현재 위치 
    }

    // 프레임이 종료 되기 전 실행되는 생명주기 함수
    private void LateUpdate()
    {
        // 애니메이터에서 설정한 파라메터 타입과 동일한 함수 작성
        anim.SetFloat("Speed", inputVec.magnitude); // (파라메터 이름, 반영할 float 값) // magnitude: 벡터의 순수한 크기 값
        // 스프라이트 방향
        if (inputVec.x != 0)
        {
            spriter.flipX = inputVec.x < 0;
        }
    }
}
