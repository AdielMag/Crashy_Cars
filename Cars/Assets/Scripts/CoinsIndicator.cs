using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class CoinsIndicator : MonoBehaviour
{
    PrefsManager prefMan;

    public int coins = 0;

    public int coinsToAdd = 0;

    public TextMeshProUGUI numbers;

    private void Start()
    {
        prefMan = PrefsManager.instance;

        // Get the coins amount from prefs
        if (prefMan.GetPref(PrefsManager.Pref.FirstTime))
        {
            coins = 200;
            prefMan.ChangePref(PrefsManager.Pref.Coins, false, coins);

            prefMan.ChangePref(PrefsManager.Pref.FirstTime, false);
        }
        else
            coins = prefMan.GetNumPref(PrefsManager.Pref.Coins);


        if (coinsToAdd > 0)
            ChangeCoins(coinsToAdd);
    }

    private void Update()
    {
        numbers.text = coins.ToString();
    }

    public void ChangeCoins(int amount)
    {
        if (prefMan == null)
            prefMan = PrefsManager.instance;
        prefMan.ChangePref(PrefsManager.Pref.Coins, false, coins + amount);

        DOTween.To(() => coins, x => coins = x, coins + amount, 3);
    }
}
