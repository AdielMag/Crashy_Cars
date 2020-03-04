using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsManager : MonoBehaviour
{
    public int points;

    public void AddPoints(int amount)
    {
        points += amount;
    }
    public void ResetPoints()
    {
        points = 0;
    }
}
