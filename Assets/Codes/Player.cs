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

    // ������ �� �ѹ��� ����Ǵ� �����ֱ� Awake
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
    }

    void Update()
    {
        if (GameManager.instance.isLive == false)
        {
            return;
        }
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
    }

    // ���� ���� �����Ӹ��� ȣ��Ǵ� �����ֱ� FixedUpdate
    void FixedUpdate()
    {
        if (GameManager.instance.isLive == false)
        {
            return;
        }
        // �������ͷ� ������ �÷��̾ ����ġ�� ���� �����̴� ���� ���� �� �ִ�
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime; // Time.fixedDeltaTime: ���� ������ �ϳ��� �Һ��� �ð�
        rigid.MovePosition(rigid.position + nextVec); // rigid.position: ���� ��ġ 
    }

    // �������� ���� �Ǳ� �� ����Ǵ� �����ֱ� �Լ�
    private void LateUpdate()
    {
        if (GameManager.instance.isLive == false)
        {
            return;
        }
        // �ִϸ����Ϳ��� ������ �Ķ���� Ÿ�԰� ������ �Լ� �ۼ�
        anim.SetFloat("Speed", inputVec.magnitude); // (�Ķ���� �̸�, �ݿ��� float ��) // magnitude: ������ ������ ũ�� ��
        // ��������Ʈ ����
        if (inputVec.x != 0)
        {
            spriter.flipX = inputVec.x < 0;
        }
    }

}
