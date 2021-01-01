using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDirectionIndiactor : MonoBehaviour
{
    [SerializeField]
    private Transform[] Targets;

    [Space]
    private LayerMask _layerMask;
    [SerializeField]
    private int castSize = 20;

    private Transform _player;

    private Transform[] _indicators;

    private bool _cast;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;

        int indicatorsCount = LevelManager.instance.botsParent.childCount;

        _indicators = new Transform[indicatorsCount];

        _indicators[0] = transform.GetChild(0);

        for (int i = 1; i < indicatorsCount; i++)
        {
            _indicators[i] =
                Instantiate(transform.GetChild(0).gameObject, transform).transform;
        }

        _cast = Targets.Length == 0;
    }

    private void LateUpdate()
    {
        transform.position = _player.position;

        if (_cast)
        {
            Collider[] hits =
                Physics.OverlapSphere(transform.position, castSize, _layerMask);

            for (int i = 0; i < _indicators.Length; i++)
            {
                if (_indicators[i].transform == _player)
                    return;

                if (i < hits.Length)
                {
                    _indicators[i].gameObject.SetActive(true);
                    RotateIndicator(_indicators[i], hits[i].transform);
                }
                else
                    _indicators[i].gameObject.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < Targets.Length; i++)
            {
                if (Targets[i].gameObject.activeSelf)
                {

                    RotateIndicator(_indicators[i], Targets[i].transform);

                    ScaleIndicator(_indicators[i], Targets[i].transform);
                }
                else
                    _indicators[i].gameObject.SetActive(false);
            }
        }
    }

    private void RotateIndicator(Transform indicator,Transform target)
    {
        indicator.LookAt(target, Vector3.up);
    }

    private void ScaleIndicator(Transform indicator, Transform target)
    {
        float distance =
            Vector3.Distance(transform.position, target.position);

        float size = 17.5f / distance;

        size = Mathf.Clamp(size,.25f,1.25f);

        indicator.GetChild(0).localScale = Vector3.one * size;
    }
}
