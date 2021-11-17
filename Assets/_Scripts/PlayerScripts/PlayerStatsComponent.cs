using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerStatsComponent : MonoBehaviour
{

    [SerializeField]
    private GameObject coinCounter;
    [SerializeField]
    private GameObject scoreCounter;
    [SerializeField]
    private GameObject keyCounter;
    [SerializeField]
    private GameObject healthCounter;
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

    private int currentHealth;
    private int maxHealth = 10;
    public Text healthText;
    public GameObject player;
    private PlayerMovement playerController;
    private void Start()
    {   
        currentHealth = maxHealth;
        playerController = player.GetComponent<PlayerMovement>();
        coinPurseText = coinCounter.GetComponent<Text>();
        playerScoreText = scoreCounter.GetComponent<Text>();
        keyText = keyCounter.GetComponent<Text>();
        healthText = healthCounter.GetComponent<Text>();
        healthText.text = "Health: " + currentHealth;
        modifyCoins(startCoins);
        modifyKeys(startKeys);
        modifyScore(startScore);
        
    }
    public void modifyCoins(int amount)
    {
        coinPurse += amount;
        coinPurseText.text = "Coins: " + coinPurse;
    }

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

    public void modifyHealth(int amount){
        if (currentHealth + amount <= maxHealth){
            currentHealth += amount;
        }
        else {
            currentHealth = maxHealth;
        }
        if (currentHealth <= 0){
            currentHealth = 0;
            playerController.playerDie();
        }
        healthText.text = "Health: " + currentHealth;
    }

    public int getHealth(){
        return this.currentHealth;
    }

    public void modifyMaxHealth(int amount){
        maxHealth += amount;
    }
    public int getMaxHealth(){
        return this.maxHealth;
    }
}
