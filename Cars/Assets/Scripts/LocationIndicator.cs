using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
enum IndicatorType
{
    Car,
    Explosion
}

public class LocationIndicator : MonoBehaviour
{
    [SerializeField] private IndicatorType type;

    private RectTransform _indicator;

    private delegate void UpdateFunction();
    private UpdateFunction m_Update;

    private Vector2 screenMeasures;

    private Camera _cam;

    private void Start()
    {
        if (!IOModeManager.instance)
        {
            enabled = false;
            return;
        }

        _cam = Camera.main;

        StartCoroutine(SetM_Update());

        SetIndicatorImage();

        screenMeasures = new Vector2(Screen.width - _indicator.rect.width,
            Screen.height - _indicator.rect.height);

        if (type == IndicatorType.Car)
        {
            CarController cCon;

            if (GetComponentInParent<Car>())
                cCon = GetComponentInParent<Car>().cCon;
            else
                cCon = transform.parent.GetComponentInParent<Car>().cCon;

            cCon.m_GotRammed += Disable;
        }

    }

    private Transform _indicatorsParent;
    private void SetIndicatorImage()
    {
        GameObject prefab;
        switch (type)
        {
            default:
                prefab = Resources.Load<GameObject>("CarOffScreenIndicator");
                break;
            case IndicatorType.Explosion:
                prefab = Resources.Load<GameObject>("ExplosionOffScreenIndicator");
                break;
        }

        Transform canvas =
            GameObject.FindGameObjectWithTag("MainCanvas").transform;

        for (int i = 0; i < canvas.childCount; i++)
            if (canvas.GetChild(i).name == prefab.transform.name + " Parent")
                _indicatorsParent = canvas.GetChild(i);

        if (_indicatorsParent == null)
        {
            _indicatorsParent =
                new GameObject(prefab.transform.name + " Parent").transform;
            _indicatorsParent.SetParent(canvas);

            _indicatorsParent.localRotation = Quaternion.Euler(Vector3.zero);
            _indicatorsParent.localPosition = Vector3.zero;
            _indicatorsParent.localScale = Vector3.one;
        }

        GameObject obj = Instantiate(prefab);

        obj.transform.SetParent(_indicatorsParent);

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
        if (!enabled)
            return;

        _indicator.gameObject.SetActive(false);

        m_Update = Empty;
    }

    void OnBecameInvisible()
    {
        if (!enabled)
            return;

        _indicator.gameObject.SetActive(true);

        m_Update = UpdateIndicator;
    }

    private void Empty() { }

    private void UpdateIndicator()
    {
        _indicator.localPosition = TargetPos();

        if (type == IndicatorType.Car)
            _indicator.localEulerAngles = TargetEuler();
    }

    private Vector3 TargetPos()
    {
        Vector3 screenPos = _cam.WorldToScreenPoint(transform.position);

        screenPos.x -= screenMeasures.x / 2;
        screenPos.y -= screenMeasures.y / 2;

        screenPos.x = Mathf.Clamp(screenPos.x, -screenMeasures.x / 2, screenMeasures.x/2);
        screenPos.y = Mathf.Clamp(screenPos.y, -screenMeasures.y / 2, screenMeasures.y/2);

        screenPos.z = 0;

        return screenPos;
    }
    private Vector3 TargetEuler()
    {
        Vector3 localPos = _indicator.localPosition;

        Vector3 targetDir = Vector3.zero;

        targetDir.z = Quaternion.LookRotation(
            _indicator.parent.forward, Vector3.zero - localPos).eulerAngles.z;

        return targetDir;
    }

    public float SignedAngle(Vector3 from, Vector3 to, Vector3 normal)
    {
        // angle in [0,180]
        float angle = Vector3.Angle(from, to);
        float sign = Mathf.Sign(Vector3.Dot(normal, Vector3.Cross(from, to)));
        return angle * sign;
    }

    private void Disable(Vector3 point)
    {
        enabled = false;

        _indicator.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        if (_indicator)
            _indicator.gameObject.SetActive(false);
    }

    IEnumerator SetM_Update()
    {
        m_Update = Empty;

        yield return new WaitForEndOfFrame();

        if (!GetComponent<MeshRenderer>().isVisible)
            m_Update = UpdateIndicator;
    }
}
