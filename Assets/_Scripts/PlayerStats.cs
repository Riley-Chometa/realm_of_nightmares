using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    // variables related to player coins
    public int coinPurse = 0;
    private int startCoins = 0;
    public Text coinPurseText;

    // variables related to player score

    public int playerScore = 0;
    private int startScore = 0;
    public Text playerScoreText; 

    // variables related to player keys
    public int numKeys = 0;
    private int startKeys = 0;
    public Text keyText;

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

    public void modifyScore(int amount)
    {
        playerScore += amount;
        playerScoreText.text = "Score: " + playerScore;
    }

    public int getScore()
    {
        return this.playerScore;
    }

    public void modifyKeys(int amount)
    {
        numKeys += amount;
        keyText.text = "Keys: " + numKeys;
    }

    public int getNumKeys()
    {
        return this.numKeys;
    }

}
