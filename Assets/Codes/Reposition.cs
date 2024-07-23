using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

                // 두 오브젝트의 거리 차이에서, X축이 Y축보다 크면 수평 이동
                if (diffX > diffY)
                {
                    // Translate: 지정된 값만큼 현재 위치에서 이동
                    transform.Translate(Vector3.right * dirX * 47);
                }
                // 두 오브젝트의 거리 차이에서, Y축이 X축보다 크면 수직 이동
                if (diffX < diffY)
                {
                    // Translate: 지정된 값만큼 현재 위치에서 이동
                    transform.Translate(Vector3.up * dirY * 47);
                }
                break;
            case "Enemy":
                if (coll.enabled)
                {
                    Vector3 dist = playerPos - myPos;
                    Vector3 ran = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
                    transform.Translate(ran + dist * 2);
                }
                break;
        }

    }
}
