using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class IOModeManager : LevelManager
{
    [Header("UI")]
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private GameObject _tutorial;
    [SerializeField] private Slider completionBarPreFill;
    [SerializeField] private Slider completionBar;

    private List<Transform> currentBots;

    override public void Start()
    {
        int currentLevel = GameManager.instance.currentLevel;

        levelText.text = "Level " + currentLevel.ToString();

        Transform layout= Instantiate(levelLayouts[currentLevel - 1]).transform;

        botsParent = layout.GetChild(0);

        currentBots = new List<Transform>();
        for (int i = 0; i < botsParent.childCount; i++)
            currentBots.Add(botsParent.GetChild(i));

        StartCoroutine(DisableBotsAfterFixedUpdate());

        if (PrefsManager.instance.GetPref(PrefsManager.Pref.FirstTime) == true)
        {
            StartCoroutine(ShowTutorial());
        }

        base.Start();
    }

    public override bool CompletedLevel(Transform takenBot,bool playerRammed =true)
    {
        if (currentBots.Contains(takenBot))
        {
            currentBots.Remove(takenBot);

            if (playerRammed)
            {
                float precentage =
                    (float)(botsParent.childCount - currentBots.Count)
                    / (float)botsParent.childCount;

                UpdateCompletionPrecentage(precentage);
            }
        }
        else
            Debug.LogError("The rammed bot is not listed! - check why");

        if (currentBots.Count == 0)
        {
            completionBar.GetComponent<UiTweener>().Disable();
            return true;
        }
        else
            return false;
    }

    private void UpdateCompletionPrecentage(float precentage)
    {
        completionBarPreFill.value = precentage;
        completionBar.DOValue(precentage, 2).SetEase(Ease.InOutFlash);
    }

    IEnumerator DisableBotsAfterFixedUpdate()
    {
        // Used to let the controllers update ones and place the cars correctly
        yield return new WaitForFixedUpdate();
        botsParent.gameObject.SetActive(false);
    }

    public void EnableBots()
    {
        botsParent.gameObject.SetActive(true);
    }

    public void ChangeTimeScale(float target)
    {
        Time.timeScale = target;
    }

    public void SetNotFirstTime()
    {
        PrefsManager.instance.ChangePref(PrefsManager.Pref.FirstTime, false);
    }

    IEnumerator ShowTutorial()
    {
        yield return new WaitForSeconds(3.2f);
        _tutorial.SetActive(true);
        ChangeTimeScale(0);
    }

    public override void PlayerLost()
    {
        completionBar.GetComponent<UiTweener>().Disable();

        lostWindow.gameObject.SetActive(true);
    }

    public void LowerFloor() // Used at the end to make cool ending effect
    {
        Transform floor = botsParent.parent.GetChild(1);

        floor.DOMoveY(floor.position.y - 10, 2.25f).SetEase(Ease.Linear);
    }
}
