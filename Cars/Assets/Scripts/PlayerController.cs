using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 7;

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
        Movement(new Vector3(tMan.input.y, 0, -tMan.input.x));
    }

    void Movement(Vector3 input)
    {
        rigidBdy.angularVelocity = input * (input.normalized.magnitude * movementSpeed);
    }
}
