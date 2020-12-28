using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;
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
    ObjectPooler objPool;

    private void Awake()
    {
        m_CarFallOff = new UnityEvent();
    }

    private void Start()
    {
        rigidBdy = GetComponent<Rigidbody>();
        pointsMan = GetComponent<PointsManager>();
        objPool = ObjectPooler.instance;

        rigidBdy.maxAngularVelocity = 1000000;
        startPos = transform.position;

        m_CarFallOff.AddListener(StartCarFallOff);
    }

    private void Update()
    {
        if (falling)
            return;

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

    public UnityEvent m_CarFallOff { get; set; }
    bool falling;
    void StartCarFallOff()
    {
        StartCoroutine(CarFallOff());
    }
    public IEnumerator CarFallOff()
    {
        falling = true;

        Vector3 lastPos = Vector3.zero + (transform.position - Vector3.zero)/1.5f;

        if (MoneyModeManager.instance) // Money gamem mode
        {
            if (!MoneyModeManager.instance.completed)
                pointsMan.ThrowPoints(.65f, lastPos);

            float duration = 2;

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

                yield return new WaitForSeconds(duration);

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
        else if (LastManStandingModeManager.instance)
        {
            if (!joystick)
            {
                gameObject.SetActive(false);

                // Bot has fallen. Update manager and check if player won
                LastManStandingModeManager.instance.BotHasFallen();
            }
            else
            {
                inGameCam.m_Follow = null;
                inGameCam.m_LookAt = null;

                // Player lost! update manager
                LastManStandingModeManager.instance.PlayerHasFallen();

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
        Vibration.VibratePeek();
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
            if (Physics.Raycast(transform.position, rigidBdy.velocity.normalized, 3))
                rigidBdy.AddForce(-collision.relativeVelocity / 4, ForceMode.Impulse);
            else
                rigidBdy.AddForce(collision.relativeVelocity / 1.5f, ForceMode.Impulse);

            if (rigidBdy.velocity.magnitude > 7 && !cantHitCars)
            {
                objPool.SpawnFromPool("HitVFX", collision.GetContact(0).point+ Vector3.up*1.5f, Quaternion.identity);
                StartCoroutine(HitCar());
                collision.transform.GetComponent<CarController>().CarGotHit();
            }
        }
    }

    public void CarGotHit()
    {
        pointsMan.ThrowPoints(.5f,transform.position);
    }

    public void CarCompletedLevel()
    {
        rigidBdy.isKinematic = true;
        rigidBdy.useGravity = false;

        enabled = false;

        pointsMan.moneyIndicator.gameObject.SetActive(false);
    }
}
