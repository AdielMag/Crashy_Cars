using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 7;

    public DynamicJoystick joystick;

    TouchManager tMan;
    [HideInInspector]
    public Rigidbody rigidBdy;

    private void Start()
    {
        rigidBdy = GetComponent<Rigidbody>();
        tMan = GetComponent<TouchManager>();

        rigidBdy.maxAngularVelocity = 1000000;

    }

    private void Update()
    {
#if UNITY_EDITOR
        Movement(new Vector3(tMan.input.y, 0, -tMan.input.x));
#else
        Movement(new Vector3(joystick.Vertical, 0, -joystick.Horizontal));
#endif

    }

    void Movement(Vector3 input)
    {
        rigidBdy.angularVelocity = input * (input.normalized.magnitude * movementSpeed);
    }
}
