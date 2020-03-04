using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    TrailRenderer trail;
    Vector3 origPos,targetPos;

    [HideInInspector]
    public Transform actualWheel;

    private void Start()
    {
        actualWheel = transform.GetChild(0);

        trail = actualWheel.GetComponentInChildren<TrailRenderer>();

        origPos = transform.localPosition;
    }

    private void Update()
    {
        if (grounded())
        {
            targetPos = Vector3.up * (-hit.distance + .49f);

            targetPos.y = Mathf.Clamp(targetPos.y, -.1f, 0);
        }
        else
            targetPos = Vector3.zero;

        actualWheel.localPosition = Vector3.Lerp(actualWheel.localPosition, targetPos, Time.deltaTime * 7);
    }

    public void HandleEmission(bool state)
    {
        if (hit.point != null)
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
        if (Physics.Raycast(transform.position+Vector3.up*.3f, -Vector3.up, out hit, .9f))
            return true;

        return false;
    }
}
