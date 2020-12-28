using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoneyModeManager : LevelManager
{
    private float lastTimeSpawnedMoney, moneySpawnWaitTime = 1.5f;

    public CoinsIndicator coinIndic;

    public new static MoneyModeManager instance;
    public override void Awake()
    {
        base.Awake();

        if (instance && instance != this)
            Destroy(instance.gameObject);

        instance = this;
    }

    public override void Start()
    {
        base.Start();

        for (int i = 0; i < 15; i++)
            SpawnMoneyCol();
    }
    public override void Update()
    {
        if (Time.time > lastTimeSpawnedMoney + moneySpawnWaitTime && moneyColCount < 25)
            SpawnMoneyCol();

        base.Update();
    }

    [HideInInspector]
    public int moneyColCount;
    void SpawnMoneyCol()
    {
        objPool.SpawnFromPool("Money Collectable", RandomNavmeshLocation(), Quaternion.identity);

        lastTimeSpawnedMoney = Time.time;
        moneyColCount++;
    }

    float radius = 50;
    public Vector3 RandomNavmeshLocation()
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }
}
