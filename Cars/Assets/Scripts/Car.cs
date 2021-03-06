﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Car : MonoBehaviour
{
    public Transform target;
    public float yOffset;

    public Wheel[] wheels;

    Transform mesh;

    private Transform _carVFX;
    private TrailCollision _trailCol;

    private float _origSize;

    [HideInInspector]
    public CarController cCon;
    bool isBot;

    private void OnDisable()
    {
        if (cCon)
        {
            cCon.m_CarFallOff -= CarFallOff;
            cCon.m_TakeDown -= TakedownScale;
        }
    }

    private void Awake()
    {
        mesh = transform.GetChild(0);

        _carVFX = transform.GetChild(2);

        if (!target)
            return;

        cCon = target.GetComponent<CarController>();
        isBot = !cCon.joystick;

        targetRot = Quaternion.LookRotation(Vector3.up, Vector3.up);

        cCon.bBrain = GetComponent<BotBrain>();

        if (isBot)
            target.GetComponent<PointsManager>().dollar = transform.GetChild(1);

        cCon.mCar = this;

        cCon.m_CarFallOff += CarFallOff;
        cCon.m_TriedToRamm += TriedToRam;
        cCon.m_TakeDown += TakedownScale;

        _trailCol = _carVFX.GetChild(0)
            .GetChild(2).GetComponent<TrailCollision>();

        _trailCol.cCon = cCon;

        _origSize = transform.localScale.x;

        foreach (Wheel wheel in wheels)
            wheel.SetScaleModifier(transform.localScale.x);
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

        transform.rotation =
            Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * 7);
    }

    Vector3 currentSwivel,targetSwivel;
    private void CarSwivel()
    {
        targetSwivel.x = 90 - rotationDelta;
        currentSwivel.x = Mathf.Lerp(currentSwivel.x, targetSwivel.x, Time.deltaTime * 5);

        if (!isGrounded)
            targetSwivel.z = cCon.rigidBdy.velocity.y * 4;
        else
            targetSwivel.z = Quaternion.LookRotation(-hit.normal).eulerAngles.z;

        currentSwivel.z = Mathf.Lerp(currentSwivel.z, targetSwivel.z, Time.deltaTime * 7);

        mesh.localEulerAngles = currentSwivel;
    }

    void HandleSkidMarks()
    {
        bool lowVelocity = cCon.rigidBdy.velocity.magnitude < 8;

        // Skid marks
        if (!lowVelocity && (currentSwivel.x > 93 || currentSwivel.x < 87))
            foreach (Wheel wheel in wheels)
                wheel.HandleSkidMarks(true);
        else
            foreach (Wheel wheel in wheels)
                wheel.HandleSkidMarks(false);

        // Smoke
        if (!lowVelocity && (currentSwivel.x > 96f || currentSwivel.x < 84f))
            foreach (Wheel wheel in wheels)
                wheel.HandleSmoke(true);
        else
            foreach (Wheel wheel in wheels)
                wheel.HandleSmoke(false);

        HandleDriftVibrations(!lowVelocity && (currentSwivel.x > 95 || currentSwivel.x < 85));
    }

    public GameObject indic;
    float lastTimeVibrated;
    void HandleDriftVibrations(bool state)
    {
        if (isBot||!state||!isGrounded)
            return;

        if (Time.time > lastTimeVibrated + .1f)
        {
            lastTimeVibrated = Time.time;
            Vibration.VibratePop();
        }
        
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
    private bool isGrounded;
    public LayerMask groundLayerMask;
    void CheckGrounded()
    {
        if (Physics.Raycast(
            transform.position + Vector3.up * 1f,
            -Vector3.up, out hit, 2.7f + cCon.carValue * .3f, groundLayerMask))
        {
            isGrounded = true;
        }
        else
            isGrounded = false;
    }

    private void CarFallOff(float duration)
    {
        StartCoroutine(RotateCarAfterFallOFf(duration));
    }

    IEnumerator RotateCarAfterFallOFf(float duration)
    {
        yield return new WaitForSeconds(duration + .1f);
        currentSwivel = targetSwivel = new Vector3(90 - rotationDelta, 0, 0);
    }

    private void TriedToRam(float cooldown,bool success)
    {
        StartCoroutine(ShowFireTrails(cooldown / 3));   
    }

    IEnumerator ShowFireTrails(float cooldown)
    {
        _trailCol.enabled = true;

        ParticleSystem[] trails =
            _carVFX.GetChild(0).GetComponentsInChildren<ParticleSystem>();

        for (int i = 0; i < trails.Length; i++)
            trails[i].Play(true);

        yield return new WaitForSeconds(cooldown);

        for (int i = 0; i < trails.Length; i++)
            trails[i].Stop(true);

        _trailCol.enabled = false;
        _trailCol.LastPointExists = false;
    }

    private void TakedownScale(int size, float duration)
    {
        transform.DOScale(_origSize + (.05f * size), duration);

        yOffset += size < 3 ? .0115f : .0225f;

        _carVFX.GetChild(1).GetComponent<ParticleSystem>().Play(true);
    }
}
