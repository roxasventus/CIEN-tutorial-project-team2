using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // 데미지, 관통, 부메랑 여부
    public float damage;
    public int per;
    public float shotspeed = 15f; // bullet's shot speed - sw

    [Header("Boomerang")]
    public bool isBoomerang = false; // default is false - sw
    public int boomerangRotationSpeed;
    public float decRate; //decreasing rate of shotspeed - sw

    [Header("MagicCircle")]
    public bool isMagicCircle = false; // default is false - sw
    public int magicCircleRotationSpeed;
    //public int interval;

    Rigidbody2D rigid;
    Vector3 dir;
    //GameObject magicCircle;

    public void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;
        this.dir = dir;

        // 관통이 -1(무한)보다 큰 것에 대해서는 속도 적용
        if (per > -1)
        {
            // 속력을 곱해주어 총알이 날아가는 속도 증가시키기
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
        if (isMagicCircle == true)
        {
            transform.Rotate(Vector3.forward * magicCircleRotationSpeed * Time.deltaTime);
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
            // 관통 값이 하나씩 줄어들면서 -1이 되면 비활성화
            per--;

            if (per == -1)
            {
                rigid.velocity = Vector3.zero;
                gameObject.SetActive(false);
            }
        }
    }

    // 투사체 삭제
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isMagicCircle)
        {
            // 근접 무기는 관련 없음
            if (!collision.CompareTag("Area") || per == -100)
                return;
            // 원거리 무기일때
            gameObject.SetActive(false);
        }
    }

    IEnumerator StopForSeconds(float duration)
    {
        // 현재 속도를 저장하고 속도를 0으로 설정
        Vector2 originalVelocity = rigid.velocity;
        rigid.velocity = Vector2.zero;

        // 회전을 멈추기 위해 각속도를 0으로 설정
        //rigid.angularVelocity = 0f;

        // duration 동안 대기
        yield return new WaitForSeconds(duration);

        // 원래 속도로 복구
        rigid.velocity = originalVelocity;

        gameObject.SetActive(false);
    }

}
