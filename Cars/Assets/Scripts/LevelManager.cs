﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public abstract class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    virtual public void Awake()
    {
        if (instance && instance != this)
            Destroy(instance.gameObject);

        instance = this;
    }

    [Space]
    public PlayableDirector finishTimeline;
    public Transform botsParent, winWindow;

    [HideInInspector]
    public ObjectPooler objPool;

    virtual public void Start()
    {
        objPool = ObjectPooler.instance;
    }
    virtual public void Update()
    {
    }

    [HideInInspector]
    public bool completed;
    public void LevelCompleted()
    {
        if (completed)
            return;


        completed = true;


        finishTimeline.Play();


        // Disable all other cars.
        botsParent.position += Vector3.right * 400 + Vector3.up * 100;
        botsParent.gameObject.SetActive(false);

        // Make car move like a bot
        GameObject.FindGameObjectWithTag("Player")
            .GetComponent<CarController>().CarCompletedLevel();

        GameManager.instance.currentLevel++;

        ObjectPooler.instance.HideCollectables();

        enabled = false;
    }
}
