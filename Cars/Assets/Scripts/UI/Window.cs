using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Window : MonoBehaviour
{
    public float scaleShowPunch = 1.2f;

    [Header("Times")]
    public float showTime = 1;

    Vector3 originalScale;
    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();

        originalScale = transform.localScale;
    }

    private void OnEnable()
    {
        Show();
    }

    void Show()
    {
        transform.localScale = Vector3.zero;


        Sequence mSeq = DOTween.Sequence();

        mSeq.Append(transform.DOScale(originalScale * scaleShowPunch, showTime * .4f));
        mSeq.Append(transform.DOScale(originalScale, showTime * .6f));
    }

    public void Hide()
    {
        StartCoroutine(HideCoroutine());
    }

    IEnumerator HideCoroutine()
    {
        Sequence mSeq = DOTween.Sequence();

        mSeq.Append(transform.DOScale(originalScale * scaleShowPunch, showTime * .4f));
        mSeq.Append(transform.DOScale(Vector3.zero, showTime * .6f));

        yield return mSeq.WaitForCompletion();

        gameObject.SetActive(false);
    }
}
