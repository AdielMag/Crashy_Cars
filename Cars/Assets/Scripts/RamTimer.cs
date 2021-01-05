using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RamTimer : MonoBehaviour
{
    private Image _image;
    private Color _origColor,_transparentColor;


    private void Start()
    {
        _image = GetComponent<Image>();

        _origColor = _transparentColor = _image.color;

        _transparentColor.a = 0;

        _image.color = _transparentColor;

        GameObject.FindGameObjectWithTag("Player")
            .GetComponent<CarController>()
            .m_TriedToRamm += StartCooldown;
    }

    public void StartCooldown(float time,bool succesful)
    {
        _image.fillAmount = 1;

        _image.DOColor(_origColor, .1f)
            .OnComplete(() => _image.DOFillAmount(0, time * .9f)
            .OnComplete(() => _image.DOColor(_transparentColor, .1f)));
    }
}
