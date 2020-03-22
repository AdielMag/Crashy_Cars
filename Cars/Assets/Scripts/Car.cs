using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public Transform target;
    public float yOffset;

    public Wheel[] wheels;

    Transform mesh;

    CarController cCon;

    private void Start()
    {
        mesh = transform.GetChild(0);

        if (!target)
            return;

        cCon = target.GetComponent<CarController>();

        targetRot = Quaternion.LookRotation(Vector3.up, Vector3.up);

        cCon.bBrain = GetComponent<BotBrain>();

        if(transform.childCount>1)
            target.GetComponent<PointsManager>().dollar = transform.GetChild(1);

    }

    private void Update()
    {
        if (target)
            transform.position = target.position + Vector3.up * yOffset;

        if (!cCon || !cCon.rigidBdy)
            return;

        TransformRotation(
            new Vector3(
                cCon.rigidBdy.angularVelocity.x,
                0,
                cCon.rigidBdy.angularVelocity.z));

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
            if (direction.normalized.magnitude > 0f)
            {
                targetRot = Quaternion.LookRotation(hit.normal, direction);
                WheelsRotation(direction);
            }
            else
            {
                targetRot = Quaternion.LookRotation(hit.normal, transform.up);
                WheelsRotation(transform.up);
            }
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * 7);

    }

    Vector3 currentSwivel,targetSwivel;
    private void CarSwivel()
    {
        targetSwivel.x = 90 - rotationDelta;
        currentSwivel.x = Mathf.Lerp(currentSwivel.x, targetSwivel.x, Time.deltaTime * 5);

        if (!isGrounded)
            targetSwivel.z = cCon.rigidBdy.velocity.y * 4;
        else
            targetSwivel.z = 0;

        currentSwivel.z = Mathf.Lerp(currentSwivel.z, targetSwivel.z, Time.deltaTime * 7);

        mesh.localEulerAngles = currentSwivel;
    }

    void HandleSkidMarks()
    {
        if (cCon.rigidBdy.velocity.magnitude < 7)
            return;

        // Skid marks
        if (currentSwivel.x > 94 || currentSwivel.x < 86)
            foreach (Wheel wheel in wheels)
                wheel.HandleEmission(true);
        else
            foreach (Wheel wheel in wheels)
                wheel.HandleEmission(false);
    }

    void WheelsRotation(Vector3 direction)
    {
        wheels[0].transform.forward = -direction;
        wheels[1].transform.forward = -direction;

         foreach (Wheel wheel in wheels)
             wheel.actualWheel.Rotate(0, 0, -cCon.rigidBdy.velocity.magnitude * 1.5f);
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
    public LayerMask groundLayerMask;
    void CheckGrounded()
    {
        if (Physics.Raycast(transform.position + Vector3.up * .5f, -Vector3.up, out hit, 1.9f,groundLayerMask))
            isGrounded = true;
        else
            isGrounded = false;
    }
}
