using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BotBrain : MonoBehaviour
{
    public LayerMask edgeLm;
    public LayerMask obstacleLM;
    public LayerMask carsLM;
    public LayerMask moneyLM;

    public float edgeRayLength = 12;
    float origEdgeRayLength;
    public float carsRadius = 14;
    public float moneyRadius = 7;

    // The number of point neede to chase a car
    float pointsToChase;

    [Space]
    public bool beingChased;
    public Transform chaseTarget;
    public Vector2 moveDirection { get; private set; }

    [Header("Chase Difficulty Variables")]

    [Tooltip("Precentage of time that " +
    "the bot will chase the player when encuountring him")]
    [SerializeField] private float _chasePrecentage = 40;
    [Tooltip("Cooldown between trying to set a new chase target")]
    [SerializeField] private float _chaseTryCooldown = 5;
    [Tooltip("Min angle that lets the bot to try" +
        " and ram the player - less is more accurate")]
    [SerializeField] private float _allowedAngleToRam = 35;
    [Tooltip("Min distance from target to allow ram")]
    [SerializeField] private float _allowedDistanceFromTargetToRam = 9;

    Car mCar;
    PointsManager pointsMan;
    Rigidbody rgdbdy;
    private void Start()
    {
        mCar = GetComponent<Car>();
        pointsMan = mCar.target.GetComponent<PointsManager>();
        rgdbdy = mCar.target.GetComponent<Rigidbody>();

        if (MoneyModeManager.instance != null)
            pointsToChase =
                Mathf.RoundToInt(GameObject.FindGameObjectWithTag("Player")
                .GetComponent<PointsManager>().pointsNeeded / 1.65f);

        // Set random direction
        moveDirection = RandomDirection(0, 359);

        origEdgeRayLength = edgeRayLength;

        currentChaseTryColldown = _chaseTryCooldown;
    }

     private void FixedUpdate()
    {
        Collisions();

        if (chaseTarget)
        {
            float distance =
                Vector3.Distance(transform.position, chaseTarget.position);
            if (distance > 38 || distance <3.5f)
                chaseTarget = null;
            else
            {
                moveDirection = ChaseTargetDirection();

                float angleToTarget =
                    Vector3.SignedAngle(transform.right,
                    transform.position - chaseTarget.position, Vector3.up);

                // Can ram?
                if (distance < _allowedDistanceFromTargetToRam 
                    && angleToTarget < _allowedAngleToRam)
                {
                    mCar.cCon.TryToRamm();
                    chaseTarget = null;
                }

                return;
            }
        }
            
        if (edgeHit.transform != null)
        {
            if (!cantChangeEdgeDir)
            {
                Vector3 dir = -(edgeHit.point - Vector3.zero);

                float angle = Vector3.SignedAngle(Vector3.right, -dir, Vector3.up);

                StartCoroutine(EdgeChangeDir(angle - 45, angle - 145));
            }
        }

        else if(obstacleHit.transform != null)
        {
            if (!cantChangeEdgeDir)
            {
                Vector3 dir = (edgeHit.point - Vector3.zero);

                float angle = Vector3.SignedAngle(Vector3.right, -dir, Vector3.up);

                StartCoroutine(EdgeChangeDir(angle - 45, angle - 145));
            }
        }

        else if (!cantChangeSwivelDir && !cantChangeEdgeDir)
        {
            float angle = Vector3.SignedAngle(Vector3.right, -transform.right, Vector3.up);

            StartCoroutine(SwivelChangeDir(angle - 55, angle - 125));
        }
    }

    Collider[] carsHit;
    Collider[] moneyHit;

    RaycastHit edgeHit;
    RaycastHit obstacleHit;

    Ray forwardRay,rightRay,leftRay;
    private void Collisions()
    {
        forwardRay = new Ray(transform.position, transform.right);

        edgeRayLength = Mathf.Lerp(0, origEdgeRayLength, rgdbdy.velocity.magnitude);
       // Debug.DrawLine(forwardRay.origin, forwardRay.origin +
       //     forwardRay.direction * rgdbdy.velocity.magnitude);

        Physics.Raycast(forwardRay, out edgeHit, edgeRayLength, edgeLm);
        Physics.Raycast(forwardRay.origin + Vector3.up * .1f, forwardRay.direction, out obstacleHit, edgeRayLength, obstacleLM);

        carsHit = Physics.OverlapSphere(transform.position, carsRadius, carsLM);
        moneyHit = Physics.OverlapSphere(transform.position, moneyRadius, moneyLM);

        if (timeToWaitUntilNextChaseTry < Time.time
            && !chaseTarget && carsHit.Length >= 0)
            SetCurrentChaseTarget();
    }

    private float timeToWaitUntilNextChaseTry;
    private float currentChaseTryColldown;
    private void SetCurrentChaseTarget()
    {
        timeToWaitUntilNextChaseTry =
            Time.time + currentChaseTryColldown;

        if (_chasePrecentage < Random.Range(0, 101))
        {
            _chasePrecentage *= 1.1f;
            currentChaseTryColldown /= 1.25f;
            return;
        }
        else
            currentChaseTryColldown = _chaseTryCooldown;


        foreach (Collider car in carsHit)
        {
            if (car.transform.tag == "Player")
            {
                chaseTarget = car.transform;
                return;
            }
            if (chaseTarget == null && car.transform != mCar.cCon.transform)
                chaseTarget = car.transform;
        }

        if (moneyHit.Length > 0)
            chaseTarget = moneyHit[0].transform;
    }

    Vector2 ChaseTargetDirection()
    {
        Vector2 targetDir = new Vector2(chaseTarget.position.x - transform.position.x,
                     chaseTarget.position.z - transform.position.z).normalized;

        return targetDir;
    }
     
    Vector2 RandomDirection(float min,float max)
    {
        float angle = Random.Range(min, max);

        var a = (90 + angle) * Mathf.Deg2Rad;
        var dir = new Vector2(-Mathf.Cos(a), Mathf.Sin(a)).normalized;


        Debug.DrawLine(transform.position,
            transform.position + new Vector3(dir.x, 0, dir.y), Color.red, 5);

        min =(90+min)* Mathf.Deg2Rad;
        max = (90 + max) * Mathf.Deg2Rad;

        Debug.DrawLine(transform.position,
            transform.position +
            new Vector3(-Mathf.Cos(min),0, Mathf.Sin(min)).normalized, Color.black, 5);
        Debug.DrawLine(transform.position,
            transform.position +
            new Vector3(-Mathf.Cos(max),0, Mathf.Sin(max)).normalized, Color.black, 5);

        return dir / (pointsMan.points >= pointsToChase ? 1 : 1.4f);
    }

    Vector3 currentDir()
    {
        float angle = transform.localEulerAngles.y;

        var a = (90 + angle) * Mathf.Deg2Rad;
        var dir = new Vector2(-Mathf.Cos(a), Mathf.Sin(a)).normalized;

        return dir;
    }

    // Coutoutins
    bool cantChangeEdgeDir;
    IEnumerator EdgeChangeDir(float min, float max)
    {
        float dirChangeTime = 1;
        DOTween.To(() => moveDirection, x => moveDirection = x,
            RandomDirection(min, max), dirChangeTime);

        cantChangeEdgeDir = true;
        yield return new WaitForSecondsRealtime(dirChangeTime);

        cantChangeEdgeDir = false;
    }

    bool cantChangeSwivelDir;
    IEnumerator SwivelChangeDir(float min, float max)
    {
        float dirChangeTime = 1f;
        DOTween.To(() => moveDirection, x => moveDirection = x,
            RandomDirection(min, max), dirChangeTime);

        cantChangeSwivelDir = true;
        yield return new WaitForSecondsRealtime(dirChangeTime);

        cantChangeSwivelDir = false;
    }

    // Difficulty variables

    // Ramm Min angle (the lower the more accurate)
}
