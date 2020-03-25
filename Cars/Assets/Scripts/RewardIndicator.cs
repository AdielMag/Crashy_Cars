using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RewardIndicator : MonoBehaviour
{
    public Image plus25;

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

    Sequence mSeq;
    IEnumerator FillSlider()
    {
        InvokeRepeating("Vibrate", 0, .05f);

        pSystem.Play();
        mSeq = DOTween.Sequence();

        mSeq.Append(slider.DOValue(0, 0f));
        mSeq.Append(slider.DOValue(1, 4f));

        Color tranparent = plus25.color;
        tranparent.a = 0;
        plus25.color = tranparent;

        mSeq.Insert(0,plus25.DOColor(new Color(tranparent.r, tranparent.g, tranparent.b, 1), .5f));
        mSeq.Insert(3.5f,plus25.DOColor(tranparent, .5f));

        plus25.transform.localScale = Vector3.one;
        mSeq.Insert(0, plus25.transform.DOPunchScale(Vector3.one,1f,5,.3f));

        mSeq.Play();

        yield return mSeq.WaitForCompletion();

        pSystem.Stop();
        CancelInvoke();   
    }

    private void OnDisable()
    {
        mSeq.Kill();
        StopAllCoroutines();
        CancelInvoke();
    }

    void Vibrate()
    {
        Vibration.VibratePop();
    }
}
