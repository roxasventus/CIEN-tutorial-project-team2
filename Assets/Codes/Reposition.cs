using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
            return;

        // 정적 변수(instance)는 즉시 클래스에서 부를 수 있다
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;
        float diffX = Mathf.Abs(playerPos.x - myPos.x);
        float diffY = Mathf.Abs(playerPos.y - myPos.y);

        Vector3 playerDir = GameManager.instance.player.inputVec;
        float dirX = playerDir.x < 0 ? -1 : 1;
        float dirY = playerDir.y < 0 ? -1 : 1;

        switch (transform.tag)
        {
            case "Ground":
                // 두 오브젝트의 거리 차이에서, X축이 Y축보다 크면 수평 이동
                if (diffX > diffY)
                {
                    // Translate: 지정된 값만큼 현재 위치에서 이동
                    transform.Translate(Vector3.right * dirX * 40);
                }
                // 두 오브젝트의 거리 차이에서, Y축이 X축보다 크면 수직 이동
                if (diffX < diffY)
                {
                    // Translate: 지정된 값만큼 현재 위치에서 이동
                    transform.Translate(Vector3.up * dirY * 40);
                }
                break;
            case "Enemy":
                break;
        }

    }
}
