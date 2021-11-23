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
    public int startScore = 0;
    public Text playerScoreText; 

    // variables related to player keys
    public int numKeys = 0;
    private int startKeys = 0;
    public Text keyText;

    // Variables related to player bombs
    public int numBombs = 0;
    private int startBombs = 0;
    public Text bombText; 

    /*
    private void Start() {
           coinPurseText = GetComponent<Text>();
           coinPurseText.text = "Coins: " + coinPurse;
    }
    */

    private void Start()
    {
        modifyCoins(startCoins);
        modifyKeys(startKeys);
        modifyScore(startScore);
        modifyBombs(startBombs);
        
    }
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

    public void modifyBombs(int amount)
    {
        numBombs += amount;
        bombText.text = "Bombs: " + numBombs;
    }

    public int getNumBombs()
    {
        return this.numBombs;
    }

}
