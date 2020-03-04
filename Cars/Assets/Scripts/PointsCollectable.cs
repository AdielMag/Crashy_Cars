using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsCollectable : MonoBehaviour
{
    public Collider col;
    public ParticleSystem pSys;

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<PointsManager>().AddPoints(10);

        StartCoroutine (Disable());
    }

    IEnumerator Disable()
    {
        col.enabled = false;

        pSys.Stop();

        yield return new WaitForSeconds(1.5f);

        gameObject.SetActive(false);
    }
}
