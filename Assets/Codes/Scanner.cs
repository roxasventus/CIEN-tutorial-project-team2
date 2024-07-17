using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    // 범위, 레이어, 스캔 결과 배열, 가장 가까운 목표를 담을 변수 선언
    public float scanRange;
    public LayerMask targetLayer;
    public RaycastHit2D[] targets;
    public Transform nearestTarget;

    private void FixedUpdate()
    {
        // CircleCastAll: 원형의 캐스트를 쏘고 모든 결과를 반환하는 함수
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer); // (캐스팅 시작 위치, 원의 반지름, 캐스팅 방향, 캐스팅 길이, 대상 레이어)
        // 가장 가까운 목표 변수를 업데이트
        nearestTarget = GetNearest();
    }

    // 가장 가까운 것을 찾는 함수 추가
    Transform GetNearest()
    {
        Transform result = null;
        float diff = 100;

        foreach (RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;
            // Distance(A, B): 벡터 A와 B의 거리를 계산해주는 함수
            float curDiff = Vector3.Distance(myPos, targetPos);

            if (curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;
            }

        }

        return result;
    }

}
