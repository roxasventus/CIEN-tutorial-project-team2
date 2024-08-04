using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Reposition : MonoBehaviour
{
    
    Collider2D coll;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
            return;

        // 정적 변수(instance)는 즉시 클래스에서 부를 수 있다
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;
        
        switch (transform.tag)
        {
            case "Ground":
                float diffX = playerPos.x - myPos.x;
                float diffY = playerPos.y - myPos.y;

                float dirX = diffX < 0 ? -1 : 1;
                float dirY = diffY < 0 ? -1 : 1;

                diffX = Mathf.Abs(diffX);
                diffY = Mathf.Abs(diffY); 

                
                if (Mathf.Abs(diffX - diffY) <= 0.1f)
                {
                    transform.Translate(Vector3.right * dirX * 47 + Vector3.up * dirY * 47);

                }
                
                else if (diffX > diffY)
                {

                    transform.Translate(Vector3.right * dirX * 47);
                }
                else if (diffX < diffY)
                {
                    transform.Translate(Vector3.up * dirY * 47);
                }
                else
                {
                    transform.Translate(Vector3.right * dirX * 47 + Vector3.up * dirY * 47);
                }
                break;
            case "Enemy":
                if (coll.enabled)
                {
                    Vector3 dist = playerPos - myPos;
                    transform.Translate(GetEnemyReposPoint(dist));
                }
                break;
        }

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (transform.tag != "CharBase" || !collision.CompareTag("NoSpawn"))
            return;

        Transform enemy = transform.parent;
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dist = playerPos - enemy.position;
        dist = dist.normalized;

        enemy.Translate(dist);
    }

    private Vector3 GetEnemyReposPoint(Vector3 dist)
    {
        Vector3 reposPoint = 2 * dist;
        float angle = 0f;
        float maxAngle = 360f;

        int layerToNotSpawnOn = LayerMask.NameToLayer("Wall");
        bool isReposValid = false;

        //find valid reposition position
        while(!isReposValid && angle < maxAngle)
        {
            reposPoint = dist + Quaternion.AngleAxis(angle, Vector3.forward) * dist;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(reposPoint, 0.8f);

            bool invalidColl = false;

            //find collision with walls
            foreach(Collider2D collider in colliders)
            {
                if(collider.gameObject.layer == layerToNotSpawnOn)
                {
                    invalidColl = true;
                    break;
                }
            }

            if (!invalidColl)
            {
                isReposValid = true;
            }

            angle++;
        }

        if (!isReposValid)
        {
            gameObject.SetActive(false);
        }

        return reposPoint;
    }
}
