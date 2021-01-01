using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RammIndicator : MonoBehaviour
{
    [SerializeField]
    private Transform _playerCar;


    private Transform _spriteParent;
    private CarController _cCon;

    private void Start()
    {
        _playerCar =
            GameObject.FindGameObjectWithTag("Player").transform;

        _cCon = _playerCar.GetComponent<CarController>();

        _playerCar = _cCon.mCar.transform;

        _spriteParent = transform.GetChild(0);
    }

    private void LateUpdate()
    {
        transform.position = TargetPos();
        transform.forward = TargetForward();

        _spriteParent.localScale = new Vector3(_cCon.currentVelocityPrecentage, 1, 1);
    }

    Vector3 TargetPos() {
        return _playerCar.position - Vector3.up * .925f;
    }
    Vector3 TargetForward()
    {
        return -_playerCar.up;
    }
}
