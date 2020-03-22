using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCurrentCar : MonoBehaviour
{
    private void Start()
    {
        int carNum = PrefsManager.instance.GetNumPref(PrefsManager.Pref.CurrentCar);

        ShowCurrentCar(carNum);
    }
    void ShowCurrentCar(int num)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (num - 1 == i)
                transform.GetChild(i).gameObject.SetActive(true);
            else
                transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
