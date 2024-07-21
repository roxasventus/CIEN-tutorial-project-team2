using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // ������, ����, �θ޶� ����
    public float damage;
    public int per;
    public float shotspeed = 15f; // bullet's shot speed - sw

    [Header("Boomerang")]
    public bool isBoomerang = false; // default is false - sw
    public int boomerangRotationSpeed;
    public float decRate; //decreasing rate of shotspeed - sw

    Rigidbody2D rigid;
    Vector3 dir;

    public void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;
        this.dir = dir;

        // ������ -1(����)���� ū �Ϳ� ���ؼ��� �ӵ� ����
        if (per > -1)
        {
            // �ӷ��� �����־� �Ѿ��� ���ư��� �ӵ� ������Ű��
            rigid.velocity = this.dir * shotspeed;
            if (isBoomerang)
            {
                StartCoroutine(ShootBoomerang());
            }
        }
    }

    IEnumerator ShootBoomerang()
    {
        bool hitReturnPoint = false;
        float nextShotspeed = shotspeed;
        while(true)
        {
            nextShotspeed -= decRate;
            if (shotspeed <= 0 && !hitReturnPoint)
            {
                nextShotspeed = 0;
                hitReturnPoint = true;
                rigid.velocity = dir * nextShotspeed;
                yield return new WaitForSeconds(1.2f);
            }
            else
            {
                rigid.velocity = dir * nextShotspeed;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    private void Update()
    {
        if (isBoomerang == true)
        {
            transform.Rotate(Vector3.forward * boomerangRotationSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || per == -100)
        {
            return;
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
