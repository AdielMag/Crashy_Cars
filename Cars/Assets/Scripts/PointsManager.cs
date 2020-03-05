using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsManager : MonoBehaviour
{
    public int points;
    public int pointsNeeded;

    bool isPlayer;
    private void Start()
    {
        isPlayer = GetComponent<PlayerController>() ? true : false;
    }

    public void AddPoints(int amount)
    {
        points += amount;

        if(points> pointsNeeded)
            LevelManager.instance.LevelCompleted(isPlayer);
    }
}
