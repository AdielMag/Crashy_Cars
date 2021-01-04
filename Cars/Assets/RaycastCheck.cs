using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastCheck : MonoBehaviour
{
    public LayerMask mask;
    private void Update()
    {
        float distance = 10;
        if (Physics.Raycast(transform.position,
            transform.forward, distance, mask))
            Debug.Log("Hit");

        Debug.DrawLine(transform.position,
            transform.position + transform.forward * 10);
    }
}
