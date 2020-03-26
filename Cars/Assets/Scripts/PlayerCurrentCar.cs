using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCurrentCar : MonoBehaviour
{
    public bool inGame;

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
            {
                Transform car = transform.GetChild(i);
                car.gameObject.SetActive(true);

                if (inGame)
                    transform.GetChild(transform.childCount - 1).SetParent(car);
            }
            else
                transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
