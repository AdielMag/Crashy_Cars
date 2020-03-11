using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CarController : MonoBehaviour
{
    public float movementSpeed = 7;

    [Header("Inputs")]
    public DynamicJoystick joystick;
    public BotBrain bBrain;

    [HideInInspector]
    public Rigidbody rigidBdy;

    private void Start()
    {
        rigidBdy = GetComponent<Rigidbody>();

        rigidBdy.maxAngularVelocity = 1000000;
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
    public IEnumerator CarFallOff()
    {
        // Throw half of your money

        inGameCam.m_Follow = null;
        inGameCam.m_LookAt = null;

        yield return new WaitForSeconds(2);

        // UI transition

        // Set the position to close to where you fell
        transform.position = Vector3.zero;

        inGameCam.m_Follow = transform;
        inGameCam.m_LookAt = transform;
    }
    
}
