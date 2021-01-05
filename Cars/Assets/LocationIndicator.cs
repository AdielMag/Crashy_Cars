using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationIndicator : MonoBehaviour
{
    private RectTransform _indicator;

    private delegate void UpdateFunction();
    private UpdateFunction m_Update;

    private Vector2 screenMeasures;

    private Camera _cam;

    private void Start()
    {
        _cam = Camera.main;

        m_Update = Empty;

        SetIndicatorImage();

        screenMeasures = new Vector2(Screen.width - _indicator.rect.width,
            Screen.height - _indicator.rect.height);
    }

    private void SetIndicatorImage()
    {
        Transform indicatorsParent = GameObject
            .FindGameObjectWithTag("MainCanvas").transform.GetChild(7);

        GameObject obj = Instantiate(indicatorsParent.GetChild(0).gameObject);

        obj.transform.SetParent(indicatorsParent);

        _indicator = obj.GetComponent<RectTransform>();

        _indicator.localScale = Vector3.one;

        _indicator.localRotation = Quaternion.Euler(Vector3.zero);

        _indicator.gameObject.SetActive(true);
    }

    private void LateUpdate()
    {
        m_Update.Invoke();
    }

    void OnBecameVisible()
    {
        _indicator.gameObject.SetActive(false);

        m_Update = Empty;
    }

    void OnBecameInvisible()
    {
        _indicator.gameObject.SetActive(true);

        m_Update = UpdateIndicator;
    }

    private void Empty() { }

    public Vector3 sd;
    private void UpdateIndicator()
    {
        _indicator.localPosition = TargetPos();
        _indicator.localEulerAngles = TargetEuler();
    }

    private Vector3 TargetPos()
    {
        Vector3 screenPos = _cam.WorldToScreenPoint(transform.position);

        screenPos.x -= screenMeasures.x / 2;
        screenPos.y -= screenMeasures.y / 2;

        sd = screenPos;

        screenPos.x = Mathf.Clamp(screenPos.x, -screenMeasures.x / 2, screenMeasures.x/2);
        screenPos.y = Mathf.Clamp(screenPos.y, -screenMeasures.y / 2, screenMeasures.y/2);

        screenPos.z = 0;

        return screenPos;
    }
    private Vector3 TargetEuler()
    {
        Vector3 localPos = _indicator.localPosition;

        Vector3 targetDir = Vector3.zero;

        if (localPos.y > screenMeasures.y / 2)
            targetDir.z = 180;
        else if (localPos.y < screenMeasures.y / 2)
            targetDir.z = 0;
        else if (localPos.x > screenMeasures.x / 2)
            targetDir.z =90;
        else
            targetDir.z = -90;

        return targetDir;
    }
}
