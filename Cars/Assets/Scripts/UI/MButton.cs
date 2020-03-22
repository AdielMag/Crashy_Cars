using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof (Button))]
public class MButton : MonoBehaviour
{
    public Color pressColor = Color.white;
    public float showScale = 1.2f;
    public float pressScale = 1.1f;

    [Header("Times")]
    public float showTime = 1;
    public float pressTime = .3f;

    Vector3 originalScale;
    Color originalColor;
    Color transparentColor;

    Image image;

    private void Awake()
    {
        originalScale = transform.localScale;

        Button m_Button = GetComponent<Button>();
        m_Button.onClick.AddListener(Press);

        if (!GetComponent<Image>())
            return;

        image = GetComponent<Image>();

        originalColor = image.color;
        transparentColor = originalColor;
        transparentColor.a = 0;
    }

    private void OnEnable()
    {
        Show();
    }

    void Show()
    {
        transform.localScale = Vector3.zero;

        Sequence mSeq = DOTween.Sequence();

        if (image)
        {
            image.color = transparentColor;

            mSeq.Append(image.DOColor(originalColor, showTime * .2f));
        }

        mSeq.Append(transform.DOScale(originalScale * showScale, showTime*.4f));
        mSeq.Append(transform.DOScale(originalScale, showTime * .6f));

    }

    bool cantPress;
    public void Press()
    {
        if(!cantPress)
            StartCoroutine(PressCoroutine());
    }

   IEnumerator PressCoroutine()
    {
        cantPress = true;

        Sequence mSeq = DOTween.Sequence();
        if (image)
        {
            mSeq.Append(image.DOColor(pressColor, pressTime * .33f));
            mSeq.Append(image.DOColor(originalColor, pressTime * .77f));
        }

        mSeq.Insert(0, transform.DOScale(originalScale * pressScale, pressTime * .3f));
        mSeq.Insert(pressTime * .3f, transform.DOScale(originalScale, pressTime * .7f));

        yield return mSeq.WaitForCompletion ();
        cantPress = false;
    }
}
