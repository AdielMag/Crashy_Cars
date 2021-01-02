using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RammIndicator : MonoBehaviour
{
    [SerializeField]
    private Transform _playerCar;

    [Space]
    [SerializeField]
    private Color _cooldownColor;
    [SerializeField]
    private Color  _succesColor;

    private Transform _spriteParent;
    private SpriteRenderer _spriteRen;
    private CarController _cCon;

    private Color _origColor;

    private void Start()
    {
        _playerCar =
            GameObject.FindGameObjectWithTag("Player").transform;

        _cCon = _playerCar.GetComponent<CarController>();

        _playerCar = _cCon.mCar.transform;

        _spriteParent = transform.GetChild(0);
        _spriteRen = _spriteParent.GetComponentInChildren<SpriteRenderer>();
        _origColor = _spriteRen.color;

        _cCon.m_TriedToRamm += TriedToRamm;
    }

    private void LateUpdate()
    {
        transform.position = TargetPos();
        transform.forward = TargetForward();

        _spriteParent.localScale =
            new Vector3(_cCon.currentVelocityPrecentage, 1, 1);
    }

    Vector3 TargetPos() {
        return _playerCar.position - Vector3.up * .925f;
    }
    Vector3 TargetForward()
    {
        return -_playerCar.up;
    }

    private void TriedToRamm(float cooldown,bool succesful)
    {
        Color targetColor = succesful ? _succesColor : _cooldownColor;

        _spriteRen.DOColor(targetColor, cooldown * .1f)
            .OnComplete(() => _spriteRen.DOColor(_cooldownColor, cooldown * .7f)
            .OnComplete(() => _spriteRen.DOColor(_origColor, cooldown * .2f)));
    }
}
