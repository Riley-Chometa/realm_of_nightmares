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
    // [SerializeField]
    // private GameObject healthCounter;
    [SerializeField]
    private GameObject bombCounter;
    [SerializeField]
    private GameObject healthGrid;
    [SerializeField]
    private GameObject playerHeart;
    [SerializeField]
    private GameObject playerShield;
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

    // variables related to bombs
    public int numBombs = 0;
    private int startBombs = 0;
    public Text bombText;

    private int currentHealth;
    private int maxHealth = 10;
    public GameObject player;
    private PlayerMovement playerController;

    private List<GameObject> hearts = new List<GameObject>();
    private bool shieldActive;
    private void Start()
    {   
        currentHealth = maxHealth;
        playerController = player.GetComponent<PlayerMovement>();
        coinPurseText = coinCounter.GetComponent<Text>();
        playerScoreText = scoreCounter.GetComponent<Text>();
        keyText = keyCounter.GetComponent<Text>();
        bombText = bombCounter.GetComponent<Text>();
        modifyCoins(startCoins);
        modifyKeys(startKeys);
        modifyScore(startScore);
        modifyBombs(startBombs);
        shieldActive = false;
        modifyCoins(startCoins);
        modifyKeys(startKeys);
        modifyScore(startScore);
        Debug.Log(maxHealth);
        for (int i = 0; i <= maxHealth - 1; i++) {
            GameObject newHeart = Instantiate(playerHeart) as GameObject;
            Debug.Log(newHeart);
            newHeart.transform.SetParent(healthGrid.transform);
            hearts.Add(newHeart);
        }
        
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

    public void modifyBombs(int amount)
    {
        numBombs += amount;
        bombText.text = "Bombs: " + numBombs;
    }

    public int getNumBomb()
    {
        return this.numBombs;
    }
    public void modifyHealth(int amount){
        if (shieldActive && amount < 0) {
            this.deactivateShield();
            return;
        }
        if (currentHealth + amount <= maxHealth) {

            currentHealth += amount;
            if (amount < 0 && hearts.Count != 0) {
                GameObject heartToRemove = hearts[hearts.Count - 1];
                hearts.Remove(heartToRemove);
                Destroy(heartToRemove);
            }
            if (amount > 0) {
                GameObject newHeart = Instantiate(playerHeart) as GameObject;
                newHeart.transform.SetParent(healthGrid.transform);
            }
            
        }
        else {
            currentHealth = maxHealth;
        }
        if (currentHealth <= 0){
            currentHealth = 0;
            Destroy(healthGrid);
            //Debug.Log(playerController);
            playerController.playerDie();
        }
        //healthText.text = "Health: " + currentHealth;
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
    public int getCurrentHealth() {
        return this.currentHealth;
    }

    public void activateShield() {
        if (shieldActive != true) {
            GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
            GameObject shield = Instantiate(playerShield, player[0].transform.position, player[0].transform.rotation) as GameObject;
        }
        shieldActive = true;
    }
    public void deactivateShield() {
        shieldActive = false;
        Destroy(GameObject.Find("Shield(Clone)"));
    }
}
