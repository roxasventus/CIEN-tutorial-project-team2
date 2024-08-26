using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTracker : MonoBehaviour
{
    void Update()
    {
        RotateTowards();
    }

    void RotateTowards()
    {
        // 마우스 위치 가져오기
        Vector3 mousePosition = Input.mousePosition;

        // 마우스 위치를 월드 좌표로 변환
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // 오브젝트 위치와 마우스 위치 간의 방향 계산
        Vector3 direction = mousePosition - transform.position;

        // 방향 벡터를 각도로 변환
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 오브젝트 회전
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
