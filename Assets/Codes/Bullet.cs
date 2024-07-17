using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // 데미지, 관통 여부
    public float damage;
    public int per;

    Rigidbody2D rigid;

    public void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;

        // 관통이 -1(무한)보다 큰 것에 대해서는 속도 적용
        if (per > -1)
        {
            // 속력을 곱해주어 총알이 날아가는 속도 증가시키기
            rigid.velocity = dir * 15f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || per == -1)
        {
            return;
        }
        // 관통 값이 하나씩 줄어들면서 -1이 되면 비활성화
        per--;

        if (per == -1)
        {
            rigid.velocity = Vector3.zero;
            gameObject.SetActive(false);
        }
    }

}
