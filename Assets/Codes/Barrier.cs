using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public int per;
    public int duration;
    private int Firstper;

    private void OnEnable()
    {
        if (GameManager.instance == null) {
            return;
        }
        GameManager.instance.isInvincible = true;
    }

    private void Start()
    {
        Firstper = per;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Enemy"))
        {
            return;
        }

        else
        {
            StartCoroutine(EnemyCollide(duration, collision));
        }
    }

    IEnumerator EnemyCollide(float duration, Collision2D collision)
    {

        // 관통 값이 하나씩 줄어들면서 -1이 되면 비활성화
        per--;
        yield return new WaitForSeconds(duration);
        if (per < 0)
        {
            GameManager.instance.isInvincible = false;
            gameObject.SetActive(false);
            per = Firstper;
        }
        
    }
}
