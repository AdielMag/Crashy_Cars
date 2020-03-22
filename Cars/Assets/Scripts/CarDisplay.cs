using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDisplay : MonoBehaviour
{

    Vector3 origRot;

    private void Awake()
    {
        origRot = transform.eulerAngles;
    }

    private void OnEnable()
    {
        transform.eulerAngles = origRot;
    }

    private void FixedUpdate()
    {
        transform.Rotate(Vector3.forward, 1f);
    }
}
