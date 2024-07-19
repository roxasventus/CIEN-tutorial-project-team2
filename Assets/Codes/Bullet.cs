using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // ������, ����, �θ޶� ����
    public float damage;
    public int per;
    public bool isBoomerang;
    public int BoomerangRotationSpeed;

    Rigidbody2D rigid;

    public void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;

        // ������ -1(����)���� ū �Ϳ� ���ؼ��� �ӵ� ����
        if (per > -1)
        {
            // �ӷ��� �����־� �Ѿ��� ���ư��� �ӵ� ������Ű��
            rigid.velocity = dir * 15f;
        }
    }

    private void Update()
    {
        if (isBoomerang == true)
        {
            transform.Rotate(Vector3.forward * BoomerangRotationSpeed * Time.deltaTime);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || per == -100)
        {
            return;
        }

        // �θ޶��̸� ���� �ε�ģ ����, ����� �Ѵ�.
        if (isBoomerang == true)
        {
            StartCoroutine(StopForSeconds(1.2f));
        }

        else
        {
            // ���� ���� �ϳ��� �پ��鼭 -1�� �Ǹ� ��Ȱ��ȭ
            per--;

            if (per == -1)
            {
                rigid.velocity = Vector3.zero;
                gameObject.SetActive(false);
            }
        }
    }

    // ����ü ����
    private void OnTriggerExit2D(Collider2D collision)
    {
        // ���� ����� ���� ����
        if (!collision.CompareTag("Area") || per == -100)
            return;
        // ���Ÿ� �����϶�
        gameObject.SetActive(false);
    }

    IEnumerator StopForSeconds(float duration)
    {
        // ���� �ӵ��� �����ϰ� �ӵ��� 0���� ����
        Vector2 originalVelocity = rigid.velocity;
        rigid.velocity = Vector2.zero;

        // ȸ���� ���߱� ���� ���ӵ��� 0���� ����
        //rigid.angularVelocity = 0f;

        // duration ���� ���
        yield return new WaitForSeconds(duration);

        // ���� �ӵ��� ����
        rigid.velocity = originalVelocity;

        gameObject.SetActive(false);
    }

}
