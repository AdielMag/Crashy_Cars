using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PopUp : MonoBehaviour
{
    public float scaleShowPunch = 1.2f;

    [Header("Times")]
    public float showTime = 1;
    public float onTime = 1;
    public float hideTime = 1;

    [Space]
    public PopUp nextPopUp;

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

        Hide();
    }

    public void Hide()
    {
        StartCoroutine(HideCoroutine());
    }

    IEnumerator HideCoroutine()
    {
        yield return new WaitForSeconds(onTime);

        Sequence mSeq = DOTween.Sequence();

        mSeq.Append(transform.DOScale(originalScale * scaleShowPunch, hideTime * .4f));
        mSeq.Append(transform.DOScale(Vector3.zero, hideTime * .6f));

        yield return mSeq.WaitForCompletion();

        if (nextPopUp)
            nextPopUp.gameObject.SetActive(true);

        gameObject.SetActive(false);
    }
}
