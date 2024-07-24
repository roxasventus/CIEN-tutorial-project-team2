using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    public float levelTime;

    float timer;
    public int level;

    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
        levelTime = GameManager.instance.maxGameTime / spawnData.Length;
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / levelTime), spawnData.Length - 1);

        if (timer > spawnData[level].spawnTime)
        {
            timer = 0f;
            Spawn();
        }
    }

    void Spawn()
    {
        Transform point = spawnPoint[Random.Range(1, spawnPoint.Length)];
        SpawnPoint spoint = point.GetComponent<SpawnPoint>();

        while (spoint.noSpawn)
        {
            point = spawnPoint[Random.Range(1, spawnPoint.Length)];
            spoint = point.GetComponent<SpawnPoint>();
        }

        GameObject enemy = GameManager.instance.pool.Get(0);
        enemy.transform.position = point.position;
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
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
