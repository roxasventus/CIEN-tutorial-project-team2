using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    // RectTransform은 transform과 다르게 변수를 선언해줘야 함
    RectTransform rect;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        // 주의! 월드 좌표와 스크린 좌표는 서로 다르기 때문에 월드 좌표 상의 플레이어 위치를 바로 사용하면 안된다 -> (rect.position = GameManager.instance.player.transform.position) 사용하면 안됨
        // WorldToScreenPoint: 월드 상의 오브젝트 위치를 스크린 좌표로 변환
        rect.position = Camera.main.WorldToScreenPoint(GameManager.instance.player.transform.position);
    }
}
