using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RammIndicator : MonoBehaviour
{
    [SerializeField]
    private Transform _player;

    private void Start()
    {
        _player =
            GameObject.FindGameObjectWithTag("Player")
            .GetComponent<CarController>().mCar.transform;
    }

    private void LateUpdate()
    {
        transform.position = TargetPos();
        transform.rotation = TargetEuler();
    }

    Vector3 TargetPos() {
        return _player.position - Vector3.up * .95f;
    }
    Quaternion TargetEuler()
    {
        Vector3 euler =
            new Vector3(0, _player.localRotation.eulerAngles.y, 0);

        return Quaternion.Euler(euler);
    }
}
