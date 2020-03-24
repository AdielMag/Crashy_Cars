using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public Transform carIndicator;
    public Transform buyEquipIndicator;

    public CoinsIndicator coinsIndicator;

    int currentCarNum =1;

    int carPrice = 200;

    private void Start()
    {
        CheckAndShowCarStatus(currentCarNum);
    }

    public void SelectCar(int carNum)
    {
        if (currentCarNum != carNum)
            ShowSelectedCar(carNum);

        currentCarNum = carNum;
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
    void CheckAndShowCarStatus(int num)
    {
        buyEquipIndicator.gameObject.SetActive(false);
        buyEquipIndicator.gameObject.SetActive(true);

        if (BoughtCar(num))
        {
            buyEquipIndicator.GetChild(1).gameObject.SetActive(false);
            buyEquipIndicator.GetChild(2).gameObject.SetActive(true);

            if (PrefsManager.instance.GetNumPref(PrefsManager.Pref.CurrentCar) == currentCarNum)
                buyEquipIndicator.GetChild(3).gameObject.SetActive(true);
            else
                buyEquipIndicator.GetChild(3).gameObject.SetActive(false);
        }
        else
        {
            buyEquipIndicator.GetChild(1).gameObject.SetActive(true);
            buyEquipIndicator.GetChild(2).gameObject.SetActive(false);
            buyEquipIndicator.GetChild(3).gameObject.SetActive(false);
        }

    }

    bool BoughtCar(int num)
    {
        if (num == 1)
            return true;

        if (PlayerPrefs.GetInt("Car" + num.ToString()) == 1)
            return true;
        else
            return false;

    }

    public void BuyEquipCar()
    {
        if (BoughtCar(currentCarNum))
            EquipCar();
        else
        {
            if (coinsIndicator.coins < carPrice)
            {
                coinsIndicator.GetComponent<MButton>().Press();
                return;
            }
            BuyCar();
        }

        CheckAndShowCarStatus(currentCarNum);
    }

    void BuyCar()
    {
        PlayerPrefs.SetInt("Car" + currentCarNum.ToString(), 1);

        coinsIndicator.ChangeCoins(-carPrice);
    }
    void EquipCar()
    {
        PrefsManager.instance.ChangePref(PrefsManager.Pref.CurrentCar, true, currentCarNum);
    }
}

