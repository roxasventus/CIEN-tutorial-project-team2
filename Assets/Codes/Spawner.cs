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
        }
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
