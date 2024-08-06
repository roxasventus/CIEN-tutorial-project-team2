using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int per = 0;
    public float shotspeed = 7f;

    Rigidbody2D rigid;
    Vector3 dir;

    public void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
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
            // 관통 값이 하나씩 줄어들면서 -1이 되면 비활성화
            per--;

            if (per < 0)
            {
                rigid.velocity = Vector3.zero;
                gameObject.SetActive(false);
            }
        }
    }

    // 투사체 삭제
    private void OnTriggerExit2D(Collider2D collision)
    {

        if (!collision.CompareTag("Area"))
            return;
     
        gameObject.SetActive(false);

    }
}
