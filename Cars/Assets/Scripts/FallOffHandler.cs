using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallOffHandler : MonoBehaviour
{
    private float fallDuration = 2;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CarController>())
        {
            other.GetComponent<CarController>()
                .m_CarFallOff.Invoke(fallDuration);
        }
    }
}
