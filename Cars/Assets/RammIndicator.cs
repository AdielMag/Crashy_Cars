using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RammIndicator : MonoBehaviour
{
    [SerializeField]
    private Transform _player;

    private Transform _spriteParent;
    private Rigidbody _rigidbody;

    private float _carSpeed;

    private void Start()
    {
        _player =
            GameObject.FindGameObjectWithTag("Player").transform;

        _rigidbody = _player.GetComponent<Rigidbody>();

        _player = _player.GetComponent<CarController>().mCar.transform;

        _spriteParent = transform.GetChild(0);
    }

    private void LateUpdate()
    {
        transform.position = TargetPos();
        transform.forward = TargetForward();

        _carSpeed =
            Mathf.Lerp(_carSpeed,
            _rigidbody.velocity.magnitude / 15,
            Time.deltaTime * 5);

        _carSpeed = Mathf.Clamp01(_carSpeed);

        _spriteParent.localScale = new Vector3(_carSpeed, 1, 1);
    }

    Vector3 TargetPos() {
        return _player.position - Vector3.up * .925f;
    }
    Vector3 TargetForward()
    {
        return -_player.up;
    }
}
