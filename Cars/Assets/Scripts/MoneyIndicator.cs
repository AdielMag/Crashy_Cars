using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MoneyIndicator : MonoBehaviour
{
    public Text precentageText;

    Slider slider;

    private void Start()
    {
        slider=GetComponent<Slider>();
    }

    private void Update()
    {
        precentageText.text = Mathf.RoundToInt ((slider.value * 100)).ToString() + "%";
    }

    public void UpdateUI(float precentage)
    {
        slider.DOValue(precentage, 1.5f);
    }
}
