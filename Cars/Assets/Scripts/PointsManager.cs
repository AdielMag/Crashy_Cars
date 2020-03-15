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
    public Transform dollar;

    bool isBot;
    private void Start()
    {
        isBot = GetComponent<CarController>().bBrain;

        if (dollar)
            dollar.localScale = Vector3.zero;
    }

    public void AddPoints(int amount = 1)
    {
        points += amount;

        UpdateIndicator();

        if (!isBot && points >= pointsNeeded && LevelManager.instance)
            LevelManager.instance.LevelCompleted();
    }

    private void UpdateIndicator()
    {
        float targetValue = (float)points / (float)pointsNeeded;

        if(slider)
            slider.DOValue(targetValue, 1.5f);
        else if(dollar)
        {
            dollar.DOScale(1 * targetValue, 2);
        }
    }

    public void ThrowAllPoints() { }
}
