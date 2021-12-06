using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StatCanvasScript : MonoBehaviour
{
    // [SerializeField]
    // Text currentEnemies;

    // [SerializeField]
    // Text currentGoldCoins;

    // [SerializeField]
    // Text currentSilverCoins;

    [SerializeField]
    Text totalScore;

    [SerializeField]
    Text totalEnemies;

    [SerializeField]
    Text totalGold;

    [SerializeField]
    Text totalSilver;

    [SerializeField]
    Text totalBronze;

    [SerializeField]
    GameObject thisCanvas;
    [SerializeField]
    private bool ShowSecretMessage = false;

    void Start() {
        SetText();
    }

    void SetText() {

        GameObject stats = GameObject.Find("StatTracker");
        

        // int currentE = stats.GetComponent<StatTracker>().getCurrentEnemies();
        // int currentGold = stats.GetComponent<StatTracker>().getCurrentGold();
        // int currentSilver = stats.GetComponent<StatTracker>().getCurrentSilver();
        // int currentBronze = stats.GetComponent<StatTracker>().getCurrentBronze();

        int totalE = stats.GetComponent<StatTracker>().getTotalEnemies();
        int totalG = stats.GetComponent<StatTracker>().getTotalGold();
        int totalS = stats.GetComponent<StatTracker>().getTotalSilver();
        int totalB = stats.GetComponent<StatTracker>().getTotalBronze();

        // currentEnemies.text = "Enemies Killed: " + currentE;
        // currentGoldCoins.text = "Gold Coins Collected: " + currentGold;
        // currentSilverCoins.text = "Silver Coins Collected: " + currentSilver;
        // currentBronzeCoins.text = "Bronze Coins Collected: " + currentBronze;
        ShowSecretMessage = GameObject.Find("GameMode").GetComponent<GameMode>().IsAustralianMode();
        if (ShowSecretMessage)
            GameObject.Find("SecretMessage").GetComponent<Text>().text = totalE == 0 ? 
                                                                    "Well it looks like you were just here to fuck spiders...":
                                                                    "Well it looks like you're not here to fuck spiders!";
        else 
            GameObject.Find("SecretMessage").GetComponent<Text>().text = "";
        totalEnemies.text = "Total Enemies Killed: " + totalE;
        totalGold.text = "Total Gold Coins Collected: " + totalG;
        totalSilver.text = "Total Silver Coins Collected: " + totalS;
        totalBronze.text = "Total Bronze Coins Collected: " + totalB;
        totalScore.text = "Total Score: " + GameObject.Find("PlayerStats").GetComponent<PlayerStatsComponent>().getScore();

    }
    

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Destroy(thisCanvas);
            Destroy(GameObject.FindGameObjectWithTag("Player"));
            Destroy(GameObject.Find("PlayerStats"));
            SceneManager.LoadScene("MainMenu");
        }
    }
}
