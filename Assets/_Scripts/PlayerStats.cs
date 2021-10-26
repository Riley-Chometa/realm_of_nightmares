using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public int coinPurse = 0;
    private int startCoins = 0;
    public Text coinPurseText;

    // private void Start() {
    //     coinPurseText = GetComponent<Text>();
    //     coinPurseText.text = "Coins: " + coinPurse;
    // }
    public void modifyCoins(int amount)
    {
        coinPurse += amount;
        //coinText.GetComponent<Text>() = "Coins: " + coinPurse;
        coinPurseText.text = "Coins: " + coinPurse;
    }

    // private void Update() {
    //     if (startCoins = coinPurse)
    // }

    public int getCoins()
    {
        return this.coinPurse;
    }

}
