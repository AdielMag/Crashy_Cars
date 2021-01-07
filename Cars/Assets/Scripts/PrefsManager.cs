using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefsManager : MonoBehaviour
{
    public static PrefsManager instance;
    private void Awake()
    {
        instance = this;
    }

    public enum Pref { Sound, Vibration , FirstTime, CurrentCar,Coins}

    public void ChangePref(Pref type, bool state = true, int num = 0)
    {
        if (type == Pref.Vibration)
            PlayerPrefs.SetInt("CanVibrate", state ? 0 : 1);
        else if (type == Pref.FirstTime)
            PlayerPrefs.SetInt("FirstTime", state ? 0 : 1);
        else if (type == Pref.CurrentCar)
            PlayerPrefs.SetInt("CurrentCar", num);
        else if (type == Pref.Coins)
            PlayerPrefs.SetInt("Coins", num);
    }

    public bool GetPref(Pref type)
    {
        bool target;

        if (type == Pref.Vibration)
            target = PlayerPrefs.GetInt("CanVibrate") == 0 ? true : false;
        else if (type == Pref.FirstTime)
        {
            int i = PlayerPrefs.GetInt("FirstTime");
            target = i == 0 ? true : false;
        }
        else
            target = false;

        return target;
    }
    public int GetNumPref(Pref type)
    {
        int target;

        if (type == Pref.CurrentCar)
        {
            target = PlayerPrefs.GetInt("CurrentCar");
            target = target < 1 ? 1 : target;
        }
        else if (type == Pref.Coins)
            target = PlayerPrefs.GetInt("Coins");
        else
            target = 0;

        return target;
    }

    public int GetCurrentLevel()
    {
        if (PlayerPrefs.GetInt("CurrentLevel") == 0)
        {
            PlayerPrefs.SetInt("CurrentLevel", 1);
        }
        return PlayerPrefs.GetInt("CurrentLevel");
    }

    public void SetCurrentLevel(int level)
    {
        PlayerPrefs.SetInt("CurrentLevel", level);
    }
}
