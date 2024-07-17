using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // .. 프리펩들을 보관할 변수
    public GameObject[] prefabs; // 프리펩들을 저장할 배열 변수 선언

    // .. 풀 담당을 하는 리스트들
    List<GameObject>[] pools;

    // 프리펩과 리스트는 1대 1 관계이다 

    private void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for (int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }

    }

    // 게임 오브젝트를 반환하는 함수 선언
    public GameObject Get(int index)
    {
        GameObject select = null;

        // ... 선택한 풀의 비활성화 되어 있는 게임 오브젝트 접근
        // foreach: 배열, 리스트의 데이터를 순차적으로 접근하는 반박문
        foreach (GameObject item in pools[index])
        {
            // ... 발견하면 select 변수에 할당
            // activeSelf: 오브젝트가 비활성화인지 확인
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }
        // ... 못 찾았으면?
        if (!select)
        {
            // ... 새롭게 생성하고 select 변수에 할당
            // Instantiate: 원본 오브젝트를 복제하여 장면에 생성하는 함수
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);

        }
        return select;
    }
}
