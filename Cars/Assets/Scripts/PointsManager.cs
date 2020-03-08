using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class PointsManager : MonoBehaviour
{
    public int points;
    public int pointsNeeded;

    [Space]
    public Slider slider;

    public void AddPoints(int amount = 1)
    {
        points += amount;

        UpdateIndicator();

        if (points >= pointsNeeded)
            LevelManager.instance.LevelCompleted();
    }

    private void UpdateIndicator()
    {
        float targetValue = (float)points / (float)pointsNeeded;

        slider.DOValue(targetValue, 1.5f);
    }
}
