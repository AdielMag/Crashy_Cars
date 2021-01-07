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

    public override bool CompletedLevel(Transform takenBot, bool playerRammed = true)
    {
        throw new System.NotImplementedException();
    }

    public override void PlayerLost()
    {
        throw new System.NotImplementedException();
    }
}
