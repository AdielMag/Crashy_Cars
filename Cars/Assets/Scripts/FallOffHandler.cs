using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallOffHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CarController>())
        {
            StartCoroutine (other.GetComponent<CarController>().CarFallOff());
        }
    }
}
