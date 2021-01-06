using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IOModeManager : LevelManager
{
    private List<Transform> currentBots;

    public GameObject _tutorial;

    override public void Start()
    {
        base.Start();

        currentBots = new List<Transform>();
        for (int i = 0; i < botsParent.childCount; i++)
            currentBots.Add(botsParent.GetChild(i));

        if (PrefsManager.instance.GetPref(PrefsManager.Pref.FirstTime))
        {
            _tutorial.SetActive(true);
            ChangeTimeScale(0);
        }
    }

    public override bool CompletedLevel(Transform takenBot)
    {
        if (currentBots.Contains(takenBot))
            currentBots.Remove(takenBot);
        else
            Debug.LogError("The rammed bot is not listed! - check why");

        if (currentBots.Count == 0)
            return true;
        else
            return false;
    }

    public void ChangeTimeScale(float target)
    {
        Time.timeScale = target;
    }

    public void SetNotFirstTime()
    {
        PrefsManager.instance.ChangePref(PrefsManager.Pref.FirstTime, false);
    }
}
