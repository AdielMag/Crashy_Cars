using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDirectionIndiactor : MonoBehaviour
{
    [SerializeField]
    private LayerMask _layerMask;
    [SerializeField]
    private int castSize = 20;

    private Transform _player;

    private Transform[] _indicators;

    private Transform[] _targets;


    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;

        int indicatorsCount = 6;

        _indicators = new Transform[indicatorsCount];

        _indicators[0] = transform.GetChild(0);
        for (int i = 1; i < indicatorsCount; i++)
        {
            _indicators[i] =
                Instantiate(transform.GetChild(0).gameObject, transform).transform;
        }
    }

    private void LateUpdate()
    {
        transform.position = _player.position;

        Collider[] hits = 
            Physics.OverlapSphere(transform.position, castSize, _layerMask);

        for(int i =0; i< _indicators.Length; i++)
        {
            if (i < hits.Length)
            {
                _indicators[i].gameObject.SetActive(true);
                RotateIndicator(_indicators[i], hits[i].transform);
            }
            else
                _indicators[i].gameObject.SetActive(false);
        }
    }

    private void RotateIndicator(Transform indicator,Transform target)
    {
        indicator.LookAt(target, Vector3.up);
    }
}
