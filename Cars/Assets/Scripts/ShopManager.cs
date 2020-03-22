using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public Transform carIndicator;

    int currentCarNum;

    int carPrice = 200;
    
    public void SelectCar(int carNum)
    {
        if (currentCarNum == carNum)
            return;

        currentCarNum = carNum;

        ShowSelectedCar(carNum);
        CheckAndShowCarStatus(carNum);
    }
    void ShowSelectedCar(int num)
    {
        carIndicator.gameObject.SetActive(false);

        for (int i = 0; i < carIndicator.childCount; i++)
            carIndicator.GetChild(i).gameObject.SetActive(false);

        carIndicator.GetChild(num-1).gameObject.SetActive(true);

        carIndicator.gameObject.SetActive(true);
    }
    void CheckAndShowCarStatus(int num) { }

    public void BuyEquipCar() { }
}

