using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatTracker : MonoBehaviour
{

    // public static StatTracker Instance;

    // void Awake ()   
    //    {
    //     if (Instance == null)
    //     {
    //         DontDestroyOnLoad(gameObject);
    //         Instance = this;
    //     }
    //     else if (Instance != this)
    //     {
    //         Destroy (gameObject);
    //     }
    //   }
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

    // public void SaveStats()
    // {
    //     StatTracker.Instance.totalEnemies = totalEnemies;
    //     StatTracker.Instance.totalSpawners = totalSpawners;
    //     StatTracker.Instance.totalGoldCoins = totalGoldCoins;
    //     StatTracker.Instance.totalSilverCoins = totalSilverCoins;
    //     StatTracker.Instance.totalBronzeCoins = totalBronzeCoins;
    //     }
}
