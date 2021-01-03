using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RamTimerSprite : MonoBehaviour
{
    private Transform _player;

    private Material _material;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;

        _material = GetComponent<SpriteRenderer>().sharedMaterial;

        CarController cCon = GameObject.FindGameObjectWithTag("Player")
            .GetComponent<CarController>();

        cCon.m_TriedToRamm += StartCooldown;
  

        _material.SetFloat("_Arc2", 360);
    }

    public float sd;
    private void LateUpdate()
    {
        transform.position = _player.position - Vector3.up;
    }


    Tween _tween;
    private void StartCooldown(float time, bool succesful)
    {
        _material.SetFloat("_Arc2", 0);

        _tween.SetEase(Ease.Linear);

        _tween = _material.DOFloat(360, "_Arc2", time);
    }


}