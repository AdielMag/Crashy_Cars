using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class CarController : MonoBehaviour
{
    public float movementSpeed = 7;

    [Header("Inputs")]
    public DynamicJoystick joystick;
    public BotBrain bBrain;

    [HideInInspector]
    public Rigidbody rigidBdy;

    PointsManager pointsMan;

    private void Start()
    {
        rigidBdy = GetComponent<Rigidbody>();

        pointsMan = GetComponent<PointsManager>();


        rigidBdy.maxAngularVelocity = 1000000;

        startPos = transform.position;
    }

    private void Update()
    {
        if (joystick)
            Movement(new Vector3(joystick.Vertical, 0, -joystick.Horizontal));
        else if (bBrain)
            Movement(new Vector3(bBrain.moveDirection.y, 0, -bBrain.moveDirection.x));
    }

    void Movement(Vector3 input)
    {
        rigidBdy.angularVelocity = input * (input.normalized.magnitude * movementSpeed);
    }

    [Space]
    public CinemachineVirtualCamera inGameCam;
    public CinemachineVirtualCamera fallCam;

    Vector3 startPos;

    public IEnumerator CarFallOff()
    {
        Vector3 lastPos = Vector3.zero + (transform.position - Vector3.zero)/1.5f;
        pointsMan.ThrowAllPoints(lastPos);

        float duration = 2;

        if (!joystick)
        {
            yield return new WaitForSeconds(duration);
            transform.position = startPos;
        }
        else
        {
            inGameCam.m_Follow = null;
            inGameCam.m_LookAt = null;

            yield return new WaitForSeconds(.4f);

            fallCam.enabled = true;
            inGameCam.enabled = false;

            // Throw half of your money


            yield return new WaitForSeconds(duration);

            // Set the position to close to where you fell
            transform.position = startPos;

            fallCam.enabled = false;
            inGameCam.enabled = true;

            inGameCam.m_Follow = transform;
            inGameCam.m_LookAt = transform;
        }
    }

    bool cantHitCars;
    IEnumerator HitCar()
    {
        cantHitCars = true;
        yield return new WaitForSeconds(1);
        cantHitCars = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<CarController>())
        {
            if (Physics.Raycast(transform.position, rigidBdy.velocity.normalized, 3))
                rigidBdy.AddForce(-collision.relativeVelocity / 4, ForceMode.Impulse);
            else
                rigidBdy.AddForce(collision.relativeVelocity / 1.5f, ForceMode.Impulse);

            if (rigidBdy.velocity.magnitude > 7 && !cantHitCars)
            {
                StartCoroutine(HitCar());
                collision.transform.GetComponent<CarController>().CarGotHit();
            }
        }
    }

    public void CarGotHit()
    {
        pointsMan.ThrowAllPoints(transform.position);
    }
}
