using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int per = 0;
    public float shotspeed = 7f;
    bool flashing = false;

    Rigidbody2D rigid;
    Vector3 dir;
    DamageFlash damageFlash;
    

    public void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        damageFlash = GetComponent<DamageFlash>(); 
    }

    public void Init(int per, Vector3 dir)
    {
        if (rigid == null)
        {
            return;
        }

        this.per = per;
        this.dir = dir;

        if (per >= 0)
        {
            rigid.velocity = this.dir * shotspeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }

        else
        {
            // ���� ���� �ϳ��� �پ��鼭 -1�� �Ǹ� ��Ȱ��ȭ
            per--;

            if (per < 0)
            {
                rigid.velocity = Vector3.zero;
                gameObject.SetActive(false);
            }
        }
    }

    // ����ü ����
    private void OnTriggerExit2D(Collider2D collision)
    {

        if (!collision.CompareTag("Area"))
            return;
     
        gameObject.SetActive(false);

    }

    private void Update()
    {
        if (!GameManager.instance.isLive || flashing)
            return;
        flashing = true;
        damageFlash.CallDamageFlash();
        flashing = false;
        
    }
}
