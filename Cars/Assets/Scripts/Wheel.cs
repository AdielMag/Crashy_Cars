using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    TrailRenderer trail;
    Vector3 origPos,targetPos;

    private void Start()
    {
        trail = GetComponentInChildren<TrailRenderer>();

        origPos = transform.localPosition;
    }

    private void Update()
    {
        if (hit.transform != null)
            targetPos = origPos - Vector3.up * (hit.distance - .7f);
        else
            targetPos = origPos - Vector3.up * .05f;

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * 7);
    }

    public void HandleEmission(bool state)
    {
        if (grounded())
        {
            if (!state && trail.emitting)
                trail.emitting = false;
            else if (state && !trail.emitting)
                trail.emitting = true;
        }
        else if (trail.emitting)
            trail.emitting = false;
    }

    RaycastHit hit;
    bool grounded()
    {
        if (Physics.Raycast(origPos + Vector3.up * .65f, -Vector3.up, out hit, 1.2f))
            return true;

        return false;
    }
}
