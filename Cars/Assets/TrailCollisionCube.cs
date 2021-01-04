using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailCollisionCube : MonoBehaviour
{
    [HideInInspector]
    public TrailCollision _collisionHandler;

    private void OnTriggerEnter(Collider other)
    {
        _collisionHandler.Colided(other.GetComponent<CarController>());
    }
}
