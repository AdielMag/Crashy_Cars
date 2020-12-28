using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastManStandingModeManager : LevelManager
{
    public new static LastManStandingModeManager instance;
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

    }
    public override void Update()
    {
        base.Update();
    }
        
    public void BotHasFallen()
    {
        int activeBots = 0;

        for (int i = 0; i < botsParent.childCount; i++)
        {
            if (botsParent.GetChild(i).gameObject.activeInHierarchy)
                activeBots++;
        }

        if (activeBots == 0)
            LevelCompleted();
    }

    public void PlayerHasFallen()
    {

    }
}
