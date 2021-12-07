using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerStatsComponent : MonoBehaviour
{
private CanvasParts canvasParts;
    private GameObject coinCounter;
    private GameObject scoreCounter;
    private GameObject keyCounter;
    // [SerializeField]
    // private GameObject healthCounter;
    private GameObject bombCounter;
    
    private GameObject healthGrid;
    private GameObject backgroundGrid;
    [SerializeField]
    private GameObject playerHeart;
    // [SerializeField]
    // private GameObject backgroundHeart;
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
    private int NthHit;
    public GameObject player;
    private PlayerMovement playerController;

    private List<GameObject> hearts = new List<GameObject>();
    //private List<GameObject> backgroundHearts = new List<GameObject>();
    private bool shieldActive;
    private GameObject shield;
    private void Start()
    {   
        canvasParts = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasParts>();
        coinCounter = canvasParts.GetCoinCounter();
        scoreCounter = canvasParts.GetScoreCounter();
        keyCounter = canvasParts.GetKeyCounter();
        bombCounter = canvasParts.GetBombCounter();
        healthGrid = canvasParts.GetPlayerHealth();
        //backgroundGrid = canvasParts.GetBackgroundHealth();
        currentHealth = maxHealth;
        playerController = player.GetComponent<PlayerMovement>();
        coinPurseText = coinCounter.GetComponent<Text>();
        playerScoreText = scoreCounter.GetComponent<Text>();
        keyText = keyCounter.GetComponent<Text>();
        bombText = bombCounter.GetComponent<Text>();
        coinPurse = startCoins;
        modifyKeys(startKeys);
        modifyScore(startScore);
        modifyBombs(startBombs);
        shieldActive = false;
        modifyCoins(startCoins);
        modifyKeys(startKeys);
        modifyScore(startScore);
        //Debug.Log(maxHealth);
        NthHit = 0;
        SetHeartPrefabs();
        
    }

    private void SetHeartPrefabs()
    {
        for (int i = 0; i <= maxHealth - 1; i++) {
            GameObject newHeart = Instantiate(playerHeart) as GameObject;
            
            newHeart.transform.SetParent(healthGrid.transform);
            newHeart.transform.localScale = new Vector3(0.7f, 0.7f, 1);
            hearts.Add(newHeart);
        }
        // for (int i = 0; i <= maxHealth - 1; i++) {
        //     GameObject newHeart = Instantiate(backgroundHeart) as GameObject;
        //     //Debug.Log(newHeart);
        //     newHeart.transform.SetParent(backgroundGrid.transform);
        //     backgroundHearts.Add(newHeart);
        // }
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
        try{
        if (shieldActive && amount < 0) {
            this.deactivateShield();
            return;
        }
        if (currentHealth + amount <= maxHealth) {
            if (amount < 0) {
                NthHit -= amount;
            }
            GameObject heartToRemove = hearts[hearts.Count - 1];
            if (NthHit >= 4) {
                NthHit = 0;
                if (amount < 0 && hearts.Count != 0) {
                    hearts.Remove(heartToRemove);
                    Destroy(heartToRemove);
                    currentHealth += amount;
                } 
            }
            if (NthHit != 4 && amount < 0) {
                Vector3 scaleChange = new Vector3(-0.25f, -0.25f, -0.25f);
                heartToRemove.transform.localScale += scaleChange;
            }   
            
            if (amount > 0) {
                Vector3 scale = new Vector3(0.7f, 0.7f, 1.0f);
                if (NthHit != 0) {
                    hearts[hearts.Count - 1].transform.localScale = scale;
                    NthHit = 0;
                }
                else {
                    if (currentHealth < maxHealth) {
                        for (int i = 0; i<amount;i++)
                    {
                        //print("HEALED");
                        GameObject newHeart = Instantiate(playerHeart) as GameObject;
                        newHeart.transform.SetParent(healthGrid.transform);
                        hearts.Add(newHeart);
                        newHeart.transform.localScale = new Vector3(0.7f, 0.7f, 1);
                        hearts.Add(newHeart);
                        currentHealth += amount;
                        if (currentHealth > maxHealth) {
                            currentHealth = maxHealth;
                        }
                    }
                } 
            }

                // if (NthHit != 4) {
                //     GameObject newHeart = Instantiate(playerHeart) as GameObject;
                //     newHeart.transform.SetParent(healthGrid.transform);
                //     newHeart.transform.localScale = scale;
                //     hearts.Add(newHeart);
                // }
                
            }
            
        }
        else {
            currentHealth = maxHealth;
            hearts[hearts.Count - 1].transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            NthHit = 0;
        }
        if (currentHealth <= 0){
            currentHealth = 0;
            //Destroy(healthGrid);
            //Debug.Log(playerController);
            playerController.playerDie();
        }
        float enemySpeed;
        if (currentHealth < 5){
                enemySpeed = .014f;
                
            }
        else {
                enemySpeed = .022f;
            }
        player.GetComponent<PickUpItems>().lowHealthLightEffect(currentHealth);
        Unit[] enemies = FindObjectsOfType(typeof(Unit)) as Unit[];
            foreach (Unit enemy in enemies){
                enemy.setSpeed(enemySpeed);
            }
                
        //healthText.text = "Health: " + currentHealth;
        }
        catch{}
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
            shield = Instantiate(playerShield, player[0].transform.position, player[0].transform.rotation) as GameObject;
        }
        shieldActive = true;
    }
    public void deactivateShield() {
        shieldActive = false;
        Destroy(shield);
    }
}
