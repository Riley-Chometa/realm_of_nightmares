using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class StatCanvasScript : MonoBehaviour
{
    [SerializeField]
    Text currentEnemies;

    [SerializeField]
    Text currentGoldCoins;

    [SerializeField]
    Text currentSilverCoins;

    [SerializeField]
    Text currentBronzeCoins;

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

    void Start() {
        SetText();
    }

    void SetText() {

        GameObject stats = GameObject.Find("StatTracker");

        int currentE = stats.GetComponent<StatTracker>().getCurrentEnemies();
        int currentGold = stats.GetComponent<StatTracker>().getCurrentGold();
        int currentSilver = stats.GetComponent<StatTracker>().getCurrentSilver();
        int currentBronze = stats.GetComponent<StatTracker>().getCurrentBronze();

        int totalE = stats.GetComponent<StatTracker>().getTotalEnemies();
        int totalG = stats.GetComponent<StatTracker>().getTotalGold();
        int totalS = stats.GetComponent<StatTracker>().getTotalSilver();
        int totalB = stats.GetComponent<StatTracker>().getTotalBronze();

        currentEnemies.text = "Enemies Killed: " + currentE;
        currentGoldCoins.text = "Gold Coins Collected: " + currentGold;
        currentSilverCoins.text = "Silver Coins Collected: " + currentSilver;
        currentBronzeCoins.text = "Bronze Coins Collected: " + currentBronze;

        totalEnemies.text = "Total Enemies Killed: " + totalE;
        totalGold.text = "Total Gold Coins Collected: " + totalG;
        totalSilver.text = "Total Silver Coins Collected: " + totalS;
        totalBronze.text = "Total Bronze Coins Collected: " + totalB;


    }
    

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Destroy(thisCanvas);
        }
    }
}
