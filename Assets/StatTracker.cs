using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatTracker : MonoBehaviour
{
    // Total Stats
    // Enemy Stats
    private int totalEnemies = 0;
    private int totalSpawners = 0;


    // Coin Stats
    private int totalGoldCoins = 0;
    private int totalSilverCoins = 0;
    private int totalBronzeCoins = 0;


    // Projectile Stats
    private int totalProjectiles = 0;


    // Current Stats
    // Enemy Stats
    public int currentEnemies;
    public int currentSpawners;


    // Coin Stats
    public int currentGoldCoins;
    public int currentSilverCoins;
    public int currentBronzeCoins;


    // Projectile Stats
    public int currentProjectiles;


    // Start is called before the first frame update
    void Start()
    {
        resetCounter();
    }

    public void endOfLevel() {
        totalEnemies += currentEnemies;
        totalSpawners += currentSpawners;
        totalGoldCoins += currentGoldCoins;
        totalSilverCoins += currentSilverCoins;
        totalBronzeCoins += currentBronzeCoins;
        totalProjectiles += currentProjectiles;

        resetCounter();
    }

    void resetCounter() {
        currentEnemies = 0;
        currentSpawners = 0;
        currentGoldCoins = 0;
        currentSilverCoins = 0;
        currentBronzeCoins = 0;
        currentProjectiles = 0;
    }
}
