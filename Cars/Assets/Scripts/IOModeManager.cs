using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class IOModeManager : LevelManager
{
    private List<Transform> currentBots;

    [Header("UI")]
    [SerializeField] private GameObject _tutorial;
    [SerializeField] private Slider completionBarPreFill;
    [SerializeField] private Slider completionBar;

    override public void Start()
    {
        base.Start();

        currentBots = new List<Transform>();
        for (int i = 0; i < botsParent.childCount; i++)
            currentBots.Add(botsParent.GetChild(i));

        if (PrefsManager.instance.GetPref(PrefsManager.Pref.FirstTime) == true)
        {
            StartCoroutine(ShowTutorial());
        }
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

}
