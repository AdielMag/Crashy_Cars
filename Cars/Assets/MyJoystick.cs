using UnityEngine;
using UnityEngine.EventSystems;

public class MyJoystick : FloatingJoystick
{
    [SerializeField] private RectTransform _ramTimer = null;
    [SerializeField] private RectTransform _firstTouchIndicator = null;

    protected override void Start()
    {
        base.Start();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (_firstTouchIndicator.gameObject.activeInHierarchy)
            _firstTouchIndicator.gameObject.SetActive(false);

        _ramTimer.anchoredPosition =
            ScreenPointToAnchoredPosition(eventData.position);

        base.OnPointerDown(eventData);
    }
}
