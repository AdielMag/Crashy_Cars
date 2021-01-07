using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MyJoystick : FloatingJoystick
{
    [SerializeField] private RectTransform _ramTimer = null;
    [SerializeField] private RectTransform _firstTouchIndicator = null;
    [SerializeField] private RectTransform _firstRaiseIndicator = null;

    protected override void Start()
    {
        base.Start();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        /*
        if (_firstTouchIndicator.gameObject.activeInHierarchy)
        {
            _firstTouchIndicator.gameObject.SetActive(false);
            StartCoroutine(ShowLiftFinger());
        }
        */

        _ramTimer.anchoredPosition =
            ScreenPointToAnchoredPosition(eventData.position);

        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        /*
        if (_firstRaiseIndicator.gameObject.activeInHierarchy)
            _firstRaiseIndicator.gameObject.SetActive(false);
        */

        base.OnPointerUp(eventData);
    }

    IEnumerator ShowLiftFinger()
    {
        yield return new WaitForSeconds(1);
        _firstRaiseIndicator.gameObject.SetActive(true);

    }
}
