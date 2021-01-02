using UnityEngine;
using UnityEngine.EventSystems;

public class MyJoystick : FloatingJoystick
{
    [SerializeField]
    private RectTransform _ramTimer = null;

    protected override void Start()
    {
        base.Start();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        _ramTimer.anchoredPosition =
            ScreenPointToAnchoredPosition(eventData.position);

        base.OnPointerDown(eventData);
    }
}
