using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public Transform target;

    public Wheel[] wheels;

    Transform mesh;
    TouchManager tMan;
    PlayerController pCon;

    private void Start()
    {
        mesh = transform.GetChild(0);

        tMan = target.GetComponent<TouchManager>();
        pCon = target.GetComponent<PlayerController>();

    }

    private void Update()
    {
        transform.position = target.position;

        // TransformRotation(new Vector3(tMan.input.y, 0, -tMan.input.x));
        TransformRotation(
            new Vector3(
                pCon.rigidBdy.angularVelocity.x,
                0,
                pCon.rigidBdy.angularVelocity.z));

        CarSwivel();
        HandleSkidMarks();
    }
    private void FixedUpdate()
    {
        RotationDeltaHandler();
    }

    Quaternion targetRot;
    private void TransformRotation(Vector3 direction)
    {
        CheckGrounded();

        if (isGrounded)
        {
            if (direction.normalized.magnitude > 0)
            {
                targetRot = Quaternion.LookRotation(hit.normal, direction);
                FrontWheelsRotation(direction);
            }
            else
                targetRot = Quaternion.LookRotation(hit.normal, transform.up);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * 7);

    }

    Vector3 currentSwivel,targetSwivel;
    private void CarSwivel()
    {
        targetSwivel.x = 90 - rotationDelta;
        currentSwivel.x = Mathf.Lerp(currentSwivel.x, targetSwivel.x, Time.deltaTime * 5);

        if (!isGrounded)
            targetSwivel.z = pCon.rigidBdy.velocity.y * 4;
        else
            targetSwivel.z = 0;

        currentSwivel.z = Mathf.Lerp(currentSwivel.z, targetSwivel.z, Time.deltaTime * 7);

        mesh.localEulerAngles = currentSwivel;
    }

    void HandleSkidMarks()
    {
        if (pCon.rigidBdy.velocity.magnitude < 7)
            return;

        // Skid marks
        if (currentSwivel.x > 94 || currentSwivel.x < 86)
            foreach (Wheel wheel in wheels)
                wheel.HandleEmission(true);
        else
            foreach (Wheel wheel in wheels)
                wheel.HandleEmission(false);
    }

    void FrontWheelsRotation(Vector3 direction)
    {
        wheels[0].transform.forward = -direction;
        wheels[1].transform.forward = -direction;
    }

    // Data methods

    Vector3 lastRotation, currentRotation;
    float rotationDelta;
    void RotationDeltaHandler()
    {
        currentRotation = transform.localEulerAngles;

        rotationDelta = currentRotation.y - lastRotation.y;

        rotationDelta *= 3;

        rotationDelta = Mathf.Clamp(rotationDelta, -25, 25);

        lastRotation = currentRotation;
    }

    RaycastHit hit;
    bool isGrounded;
    void CheckGrounded()
    {
        if (Physics.Raycast(transform.position + Vector3.up * .5f, -Vector3.up, out hit, 1.5f))
            isGrounded = true;
        else
            isGrounded = false;
    }
}
