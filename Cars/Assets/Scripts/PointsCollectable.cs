using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PointsCollectable : MonoBehaviour,IPooledObject
{
    public Collider col;

    Vector3 origScale;

    private void Awake()
    {
        origScale = transform.localScale;
    }

    public void OnObjectSpawn()
    {
        if (!col.enabled)
            col.enabled = true;

        if (origScale != Vector3.zero)
            transform.localScale = origScale;

        StartCoroutine(Enable());
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<PointsManager>().AddPoints();

        StartCoroutine (Disable());
    }

    IEnumerator Enable()
    {
        col.enabled = false;

        transform.DOPunchScale(Vector3.one, 1f);

        yield return new WaitForSeconds(1);

        col.enabled = true;

    }

    IEnumerator Disable()
    {
        //transform.DOPunchScale(Vector3.one, 1.5f);
        transform.DOScale(Vector3.zero, .5f);

        col.enabled = false;

        yield return new WaitForSeconds(.5f);

        if (MoneyModeManager.instance != null)
            MoneyModeManager.instance.moneyColCount--;

        gameObject.SetActive(false);
    }
}
