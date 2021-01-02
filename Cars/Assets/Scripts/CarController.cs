﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CarController : MonoBehaviour
{
    [Space]
    public float movementSpeed = 7;
    public float currentVelocityPrecentage { get; private set; }

    [Header("Inputs")]
    public Joystick joystick;
    public BotBrain bBrain;

    [HideInInspector]
    public Rigidbody rigidBdy;
    [HideInInspector]
    public Car mCar;


    PointsManager pointsMan;
    ObjectPooler objPool;

    private void Awake()
    {
        if (joystick)
        {
            joystick.PointerUp = new UnityEvent();
            joystick.PointerUp.AddListener(TryToRamm);
        }
    }

    private void Start()
    {
        rigidBdy = GetComponent<Rigidbody>();
        pointsMan = GetComponent<PointsManager>();
        objPool = ObjectPooler.instance;

        rigidBdy.maxAngularVelocity = 1000000;
        startPos = transform.position;

        if (joystick)
            m_CarFallOff += StartCarFallOff;
    }

    private void Update()
    {
        if (falling)
            return;

        if (joystick)
            Movement(new Vector3(joystick.Vertical, 0, -joystick.Horizontal));
        else if (bBrain)
            Movement(new Vector3(bBrain.moveDirection.y, 0, -bBrain.moveDirection.x));

        currentVelocityPrecentage = CalculateCarCurrentSpeed();
    }

    void Movement(Vector3 input)
    {
        rigidBdy.angularVelocity = input * (input.normalized.magnitude * movementSpeed);
    }

    float CalculateCarCurrentSpeed()
    {
        float targetVelocity = 7;

        float carSpeed = rigidBdy.velocity.magnitude / targetVelocity;

        carSpeed = Mathf.Clamp01(carSpeed);

        return carSpeed;
    }

    [Space]
    public CinemachineVirtualCamera inGameCam;
    public CinemachineVirtualCamera fallCam;

    Vector3 startPos;

    public delegate void FallOffDelegate(float duration);
    public FallOffDelegate m_CarFallOff;
    bool falling;
    void StartCarFallOff(float duration)
    {
        StartCoroutine(CarFallOff(duration));
    }
    public IEnumerator CarFallOff(float duration)
    {
        falling = true;

        Vector3 lastPos = Vector3.zero + (transform.position - Vector3.zero) / 1.5f;

        if (LevelManager.instance) // Money gamem mode
        {
            // if (!MoneyModeManager.instance.completed)
            //     pointsMan.ThrowPoints(.65f, lastPos);

            if (!joystick)
            {
                yield return new WaitForSeconds(duration + .4f);
                transform.position = startPos;
            }
            else
            {
                inGameCam.m_Follow = null;
                inGameCam.m_LookAt = null;

                yield return new WaitForSeconds(.4f);

                fallCam.enabled = true;
                inGameCam.enabled = false;

                yield return new WaitForSeconds(duration - .3f);

                // Set the position to close to where you fell
                transform.position = startPos;

                fallCam.enabled = false;
                inGameCam.enabled = true;

                inGameCam.m_Follow = transform;
                inGameCam.m_LookAt = transform;
            }

            falling = false;

            objPool.SpawnFromPool("RespawnGlow",
                transform.position - Vector3.up * .25f,
                Quaternion.LookRotation(Vector3.up));
        }
        else
        {
            if (!joystick)
            {
                gameObject.SetActive(false);

                // Bot has fallen. Update manager and check if player won
                LevelManager.instance.BotHasFallen();
            }
            else
            {
                inGameCam.m_Follow = null;
                inGameCam.m_LookAt = null;

                // Player lost! update manager
                LevelManager.instance.PlayerHasFallen();

                yield return new WaitForSeconds(2);
                gameObject.SetActive(false);
            }
        }
    }

    bool cantHitCars;
    IEnumerator HitCar()
    {
        Vibration.VibratePeek();
        Vibration.VibratePeek();
        Vibration.VibratePeek();

        cantHitCars = true;
        yield return new WaitForSeconds(1);
        cantHitCars = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<CarController>())
        {
            /*
            if (Physics.Raycast(transform.position, rigidBdy.velocity.normalized, 3))
                rigidBdy.AddForce(-collision.relativeVelocity / 4, ForceMode.Impulse);
            else
                rigidBdy.AddForce(collision.relativeVelocity / 1.5f, ForceMode.Impulse);
            */

            if (rigidBdy.velocity.magnitude > 4 && !cantHitCars)
            {
                objPool.SpawnFromPool
                    ("HitVFX",
                    collision.GetContact(0).point + Vector3.up * 1.5f,
                    Quaternion.identity);

                StartCoroutine(HitCar());

                collision.transform.GetComponent<CarController>().CarGotHit();
            }
        }
    }

    public void CarGotHit()
    {
        if (MoneyModeManager.instance != null)
            pointsMan.ThrowPoints(.5f, transform.position);
    }

    public void CarCompletedLevel()
    {
        rigidBdy.isKinematic = true;
        rigidBdy.useGravity = false;

        enabled = false;

        if (MoneyModeManager.instance != null)
            pointsMan.moneyIndicator.gameObject.SetActive(false);
    }

    public delegate void RammedDelegate(float cooldown,bool succes);
    public RammedDelegate m_TriedToRamm;

    float lastTimeRammed;
    public float timeToWaitBetweenRamms
    {
        get => timeToWaitBetweenRamms;
        set => timeToWaitBetweenRamms = 2;
    }

    private void TryToRamm() 
    {
        if (!CanRamm())
            return;

        bool succes = false;

        Vector3 forwardDir = mCar.transform.right.normalized;
           // new Vector3(joystick.Horizontal, 0, joystick.Vertical);
        Quaternion forwardRot = Quaternion.LookRotation(forwardDir);

        // Check if someone is in range of ramming
        Vector3 boxExtents = new Vector3(1.8f, 1.5f, .5f);
        Physics.BoxCast(
            transform.position - Vector3.up * .28f,
            boxExtents,
            forwardDir,
            out RaycastHit hit,
            forwardRot,
            10 * currentVelocityPrecentage,
            LayerMask.GetMask("Controllers"));

        if (hit.transform != null)
        {
            succes = true;
            // Make sure the target will be hitted (slow it down?)
            hit.transform.GetComponent<CarController>().GotRammed();

            // Launch target to the air
            // Increase car size
        }

        // Launch forward
        rigidBdy.AddForce(forwardDir.normalized
            * (35* currentVelocityPrecentage), ForceMode.Impulse);

        // Set cooldown
        lastTimeRammed = Time.time;

        m_TriedToRamm.Invoke(timeToWaitBetweenRamms, succes);
    }

    private bool CanRamm()
    {
        // If cooldown not passed
        if (Time.time < lastTimeRammed + timeToWaitBetweenRamms)
            return false;

        // Check if grounded
        if (!Physics.Raycast(transform.position, -Vector3.up, 5))
            return false;

        return true;
    }

    public void GotRammed() { }
}
