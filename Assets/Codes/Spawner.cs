using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Potion;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    public bool isBossSpawn;
    public enum SpawnerType { Mob, Boss }
    public SpawnerType spawnerType;
    public GameObject uiWarning;

    float timer;

    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
       
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        if (spawnerType == SpawnerType.Mob)
        {
            timer += Time.deltaTime;


            if (timer > spawnData[GameManager.instance.stageNum].spawnTime && GameManager.instance.isStageClear == false)
            {
                timer = 0f;
                Spawn();
            }
        }

        if (spawnerType == SpawnerType.Boss)
        {
            if (GameManager.instance.isStageClear == true && isBossSpawn == false && !GameManager.instance.enemyCleaner.activeSelf)
            {
                Spawn();
                isBossSpawn = true;
            }
        }



    }

    void Spawn()
    {
        if (spawnerType == SpawnerType.Mob)
        {
            Transform point = spawnPoint[UnityEngine.Random.Range(1, spawnPoint.Length)];
            SpawnPoint spoint = point.GetComponent<SpawnPoint>();

            while (spoint.noSpawn)
            {
                point = spawnPoint[UnityEngine.Random.Range(1, spawnPoint.Length)];
                spoint = point.GetComponent<SpawnPoint>();
            }

            GameObject enemy = GameManager.instance.pool.Get(0);
            enemy.transform.position = point.position;
            enemy.GetComponent<Enemy>().Init(spawnData[GameManager.instance.stageNum]);
        }


        if (spawnerType == SpawnerType.Boss)
        {
            Transform point = spawnPoint[UnityEngine.Random.Range(1, spawnPoint.Length)];
            SpawnPoint spoint = point.GetComponent<SpawnPoint>();

            while (spoint.noSpawn)
            {
                point = spawnPoint[UnityEngine.Random.Range(1, spawnPoint.Length)];
                spoint = point.GetComponent<SpawnPoint>();
            }

            GameObject enemy = GameManager.instance.pool.Get(9 + GameManager.instance.stageNum);
            enemy.transform.position = point.position;
            enemy.GetComponent<Boss>().Init(spawnData[GameManager.instance.stageNum]);

            AudioManager.instance.PlaySfx(AudioManager.Sfx.BossAppear);

            StartCoroutine(WarningCoroutine(uiWarning));
        }
    }

    IEnumerator WarningCoroutine(GameObject uiWarning)
    {

        float elapsedTime = 0f;
            // 깜빡거림의 간격 (0.5초로 설정)
        float blinkInterval = 0.5f;

    // 깜빡거림의 지속 시간 (2초 동안 깜빡거리게 설정)
        float duration = 2.0f;

        while (elapsedTime < duration)
        {
            // UI를 활성화/비활성화
            uiWarning.SetActive(!uiWarning.activeSelf);

            // blinkInterval만큼 대기
            yield return new WaitForSeconds(blinkInterval);

            // 경과 시간 업데이트
            elapsedTime += blinkInterval;
        }

        // 마지막으로 UI를 원래 상태로 되돌리기
        uiWarning.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        uiWarning.SetActive(false);


    }
}

[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;
}

