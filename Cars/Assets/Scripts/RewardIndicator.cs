using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RewardIndicator : MonoBehaviour
{
    Slider slider;
    ParticleSystem pSystem;
    private void Awake()
    {
        slider = GetComponentInChildren<Slider>();
        pSystem = GetComponentInChildren<ParticleSystem>();
    }

    private void OnEnable()
    {
        StartCoroutine(FillSlider());
    }

    IEnumerator FillSlider()
    {
        pSystem.Play();
        Sequence mSeq = DOTween.Sequence();

        mSeq.Append(slider.DOValue(0, 0f));
        mSeq.Append(slider.DOValue(1, 4f));

        mSeq.Play();

        yield return mSeq.WaitForCompletion();
        pSystem.Stop();
    }
}
