using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

public class LevelManager : MonoBehaviour
{
    #region Singelton
    public static LevelManager instance;
    private void Awake()
    {
        if (instance && instance != this)
            Destroy(instance.gameObject);

        instance = this;
    }
    #endregion

    public int rewardCoins = 25;

    [Space]
    public PlayableDirector finishTimeline;

    float lastTimeSpawnedMoney, moneySpawnWaitTime = 3;

    ObjectPooler objPool;
    private void Start()
    {
        objPool = ObjectPooler.instance;

        for (int i = 0; i < 10; i++)
            SpawnMoneyCol();

    }

    private void Update()
    {
        if (Time.time > lastTimeSpawnedMoney + moneySpawnWaitTime && moneyColCount < 10)
            SpawnMoneyCol();
    }

    public Transform botsParent, winWindow;
    public CoinsIndicator coinIndic;
    [HideInInspector]
    public bool completed;
    public void LevelCompleted()
    {
        if (completed)
            return;


        completed = true;


        finishTimeline.Play();


        // Disable all other cars.
        botsParent.position += Vector3.right * 400;
        botsParent.gameObject.SetActive(false);

        // Make car move like a bot
        GameObject.FindGameObjectWithTag("Player")
            .GetComponent<CarController>().CarCompletedLevel();

        // Show Win Window
        winWindow.gameObject.SetActive(true);

        GameManager.instance.currentLevel++;

        // Add coins reward
        coinIndic.AddCoins(rewardCoins);
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

    /*
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position,radius);
    }
    */
}
