using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatTracker : MonoBehaviour
{

    // public static StatTracker Instance;

    // Total Stats
    // Enemy Stats
    private int totalEnemies = 0;


    // Coin Stats
    private int totalGoldCoins = 0;
    private int totalSilverCoins = 0;
    private int totalBronzeCoins = 0;


    // Projectile Stats
    private int totalProjectiles = 0;


    // Current Stats
    // Enemy Stats
    public int currentEnemies;


    // Coin Stats
    public int currentGoldCoins;
    public int currentSilverCoins;
    public int currentBronzeCoins;


    // Start is called before the first frame update
    void Start()
    {
        resetCounter();
    }

    public int getCurrentEnemies() {
        return currentEnemies;
    }

    public int getCurrentGold() {
        return currentGoldCoins;
    }

    public int getCurrentSilver() {
        return currentSilverCoins;
    }

    public int getCurrentBronze() {
        return currentBronzeCoins;
    }

    public int getTotalEnemies() {
        return totalEnemies + currentEnemies;
    }

    public int getTotalGold() {
        return totalGoldCoins + currentGoldCoins;
    }

    public int getTotalSilver() {
        return totalSilverCoins + currentSilverCoins;
    }

    public int getTotalBronze() {
        return totalBronzeCoins + currentBronzeCoins;
    }

    public void endOfLevel() {
        totalEnemies += currentEnemies;
        totalGoldCoins += currentGoldCoins;
        totalSilverCoins += currentSilverCoins;
        totalBronzeCoins += currentBronzeCoins;

        resetCounter();
    }

    void resetCounter() {
        currentEnemies = 0;
        currentGoldCoins = 0;
        currentSilverCoins = 0;
        currentBronzeCoins = 0;
    }
}
