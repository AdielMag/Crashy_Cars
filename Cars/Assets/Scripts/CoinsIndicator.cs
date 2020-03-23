using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinsIndicator : MonoBehaviour
{
    PrefsManager prefMan;

    public int coins = 0;

    public TextMeshProUGUI numbers;

    private void Start()
    {
        prefMan = PrefsManager.instance;

        // Get the coins amount from prefs
        if (prefMan.GetPref(PrefsManager.Pref.FirstTime))
        {
            coins = 50;
            prefMan.ChangePref(PrefsManager.Pref.Coins, false, coins);

            prefMan.ChangePref(PrefsManager.Pref.FirstTime, false);
        }
        else
            coins = prefMan.GetNumPref(PrefsManager.Pref.Coins);

        numbers.text = coins.ToString();
    }

   
}
