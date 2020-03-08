using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotBrain : MonoBehaviour
{
    public LayerMask edgeObstacleLM;
    public LayerMask carsMoneyLM;

    public float edgeObstRadius = 12;
    public float carsMoneyRadius = 10;

    [Header("Debugging")]
    public Vector2 moveDirection;
    public Transform edgeObstTarget, carsMoneyTarget;

    private void Start()
    {
        // Set random direction
        moveDirection = RandomDirection(-90, -60);
    }

    private void FixedUpdate()
    {
        Collisions();

        #region Debugging
        if (edgeObstHit.Length > 0)
            edgeObstTarget = edgeObstHit[0].transform;
        else
            edgeObstTarget = null;
        if (carsMoneyHit.Length > 0)
        {
            if (carsMoneyHit[0].transform != transform)
                carsMoneyTarget = carsMoneyHit[0].transform;
        }
        else
            carsMoneyTarget = null;
        #endregion
    }

    Collider[] edgeObstHit,carsMoneyHit;
    void Collisions()
    {
        edgeObstHit = Physics.OverlapSphere(transform.position, edgeObstRadius, edgeObstacleLM);
        carsMoneyHit = Physics.OverlapSphere(transform.position, carsMoneyRadius, carsMoneyLM);
    }


    Vector2 RandomDirection(float min,float max)
    {
        float angle = Random.Range(min, max);

        var a = (90 + angle) * Mathf.Deg2Rad;
        var dir = new Vector2(-Mathf.Cos(a), Mathf.Sin(a)).normalized;

        return dir;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, edgeObstRadius);
        Gizmos.DrawWireSphere(transform.position, carsMoneyRadius);
    }
}
