﻿using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.AI;



public class PointsManager : MonoBehaviour
{
    public int points;
    public int pointsNeeded;

    [Space]
    public MoneyIndicator moneyIndicator;
    public Transform dollar;

    ObjectPooler objPool;

    bool isBot;
    private void Start()
    {
        isBot = !GetComponent<CarController>().joystick;

        if (dollar)
            dollar.localScale = Vector3.zero;

        objPool = ObjectPooler.instance;
    }

    public void AddPoints(int amount = 1)
    {
        points += amount;

        UpdateIndicator();

        if (!isBot && points >= pointsNeeded && LevelManager.instance)
        {
            LevelManager.instance.LevelCompleted();
        }
    }

    private void UpdateIndicator()
    {
        float targetValue = (float)points / (float)pointsNeeded;

        if(moneyIndicator)
            moneyIndicator.UpdateUI(targetValue);
        else if(dollar)
        {
            dollar.DOScale(1 * targetValue, 2);
        }
    }

    public void ThrowPoints(float amount,Vector3 lastPos)
    {
        for (int i = Mathf.RoundToInt(points * amount); i > 0; i--)
        {
            Transform moneyCollectable =
                objPool.SpawnFromPool("Money Collectable",
                transform.position, Quaternion.identity).transform;


            Vector3 targetPos = RandomNavmeshLocation(lastPos);

            Sequence mSeq = DOTween.Sequence();

            float time = 1;
            mSeq.Append(moneyCollectable.DOJump(targetPos, Random.Range(1, 3f), 1, time));
        }

        points -= Mathf.RoundToInt(points * amount);

        UpdateIndicator();
    }

    float radius = 10;
    public Vector3 RandomNavmeshLocation(Vector3 castPos)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += castPos;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }
}
