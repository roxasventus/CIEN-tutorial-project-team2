using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Spinner : MonoBehaviour
{
    private bool spinning = false;
    private float spinSpeed = 100.0f;

    SpriteRenderer spriter;
    Animator anim;
    Boss2_Atk bossAtk;
    Transform warningSign;
    Transform circleEdge;
    Transform spinInner;
    Transform attractingField;

    private void Awake()
    {
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        bossAtk = GetComponentInParent<Boss2_Atk>();
        warningSign = GetComponentsInChildren<Transform>()[1];
        circleEdge = GetComponentsInChildren<Transform>()[2];
        spinInner = GetComponentsInChildren<Transform>(true)[3];
        attractingField = GetComponentsInChildren<Transform>(true)[4];
    }

    private void Update()
    {
        if (!spinning)
            return;

        transform.Rotate(Vector3.back * spinSpeed * Time.deltaTime);
        spinInner.Rotate(Vector3.back * spinSpeed * Time.deltaTime);
    }

    public void CallSpinNShoot()
    {
        StartCoroutine(SpinNShoot());
    }

    IEnumerator SpinNShoot()
    {
        yield return new WaitForSeconds(bossAtk.spinWaitTime);

        //Start spinning
        warningSign.gameObject.SetActive(false);
        circleEdge.gameObject.SetActive(false);
        spinInner.gameObject.SetActive(true);
        attractingField.gameObject.SetActive(true);

        anim.SetTrigger("Spin");
        spinning = true;

        gameObject.tag = "Trap";

        //Brighten effects
        Color temp = spriter.color;
        temp.a = 0f;
        spriter.color = temp;

        temp = spinInner.GetComponent<SpriteRenderer>().color;
        temp.a = 0f;
        spinInner.GetComponent<SpriteRenderer>().color = temp;

        for(float timer = 0f; timer < 0.8f; timer += 0.1f)
        {
            temp = spriter.color;
            temp.a += 1f / 8;
            spriter.color = temp;

            temp = spinInner.GetComponent<SpriteRenderer>().color;
            temp.a += 1f / 8;
            spinInner.GetComponent<SpriteRenderer>().color = temp;

            yield return new WaitForSeconds(0.1f);
        }

        //Shoot in random directions
        for(float timer = 0f; timer < bossAtk.spinDuration - 1.6f; timer += bossAtk.shootInterval)
        {
            for(int i = 0; i < 4; i++)
                Shoot();

            AudioManager.instance.PlaySfx(AudioManager.Sfx.Boss2Atk2Swirl);

            yield return new WaitForSeconds(bossAtk.shootInterval);
        }

        //Fade effects
        for (float timer = 0f; timer < 0.8f; timer += 0.1f)
        {
            temp = spriter.color;
            temp.a -= 1f / 8;
            spriter.color = temp;

            temp = spinInner.GetComponent<SpriteRenderer>().color;
            temp.a -= 1f / 8;
            spinInner.GetComponent<SpriteRenderer>().color = temp;

            yield return new WaitForSeconds(0.1f);
        }

        //EndSpinning
        Dead();
    }

    private void Shoot()
    {
        Vector3 dir = Random.insideUnitCircle.normalized;

        Transform enemyBullet = GameManager.instance.pool.Get(9).transform;
        enemyBullet.position = transform.position;

        enemyBullet.rotation = Quaternion.FromToRotation(Vector3.left, dir);

        enemyBullet.GetComponent<EnemyBullet>().Init(0, dir);

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Boss2Atk2Shoot);
    }

    public void Dead()
    {
        Destroy(gameObject);
    }
}
