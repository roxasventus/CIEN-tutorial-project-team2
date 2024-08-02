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

    // ������ �� �ѹ��� ����Ǵ� �����ֱ� Awake
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

    // ���� ���� �����Ӹ��� ȣ��Ǵ� �����ֱ� FixedUpdate
    void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        // �������ͷ� ������ �÷��̾ ����ġ�� ���� �����̴� ���� ���� �� �ִ�
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime; // Time.fixedDeltaTime: ���� ������ �ϳ��� �Һ��� �ð�
        rigid.MovePosition(rigid.position + nextVec); // rigid.position: ���� ��ġ 
    }

    // �������� ���� �Ǳ� �� ����Ǵ� �����ֱ� �Լ�
    private void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        // �ִϸ����Ϳ��� ������ �Ķ���� Ÿ�԰� ������ �Լ� �ۼ�
        anim.SetFloat("Speed", inputVec.magnitude); // (�Ķ���� �̸�, �ݿ��� float ��) // magnitude: ������ ������ ũ�� ��
        // ��������Ʈ ����
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
        // ���� �� �и��� �ʵ��� 
        rigid.bodyType = RigidbodyType2D.Kinematic;

        anim.SetTrigger("Dead");
        // duration ���� ���
        yield return new WaitForSeconds(4.2f);
        GameManager.instance.GameOver();

    }


}
