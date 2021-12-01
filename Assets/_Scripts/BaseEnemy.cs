using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{   
    // Health Variables
    [SerializeField]
    private int maxHealth = 100;
    [SerializeField]
    private int currentHealth;
    [SerializeField]
    public HealthBar healthBar;
    [SerializeField]
    private List<GameObject> itemDrops = new List<GameObject>();

    [SerializeField]
    private bool isTest = false;

    // Collider and animation components.
    public Collider2D mainCollider;
    private Animator anim;
    
    private bool canGetHit;
    private float hitTimerMax = .5f;
    private float hitTimerValue = 0;
    private Unit enemyController;
    public bool isAlive;

    public GameObject floatText;
    private Transform thisEnemy;

    // Set starting Variables.
    void Start()
    {
        thisEnemy = this.gameObject.GetComponent<Transform>();
        this.isAlive = true;
        enemyController = this.GetComponent<Unit>();
        anim = this.GetComponent<Animator>();
        canGetHit = true;
        currentHealth = maxHealth;
        healthBar.transform.SetParent(this.transform.GetChild(0));
        healthBar.setMaxHealth(maxHealth);
        
    }

    private void FixedUpdate() {
        if (hitTimerValue > 0){
            hitTimerValue -= Time.fixedDeltaTime;
        }
        else {
            canGetHit = true;
        }
    }
    // Take the damage from the player. run hit animation and run death if true.
    public void TakeDamage(int damage){
        if (canGetHit){
            // trigger floating text 
            if (floatText){
            showFloatingText(damage);
            }


            //
            canGetHit = false;
            hitTimerValue = hitTimerMax;
            currentHealth -= damage;
            healthBar.setHealth(currentHealth);
            if (enemyController != null)
                enemyController.setCanMove(false);
            if (anim != null) {
                anim.SetTrigger("getHit");
                // Debug.Log("Enemy Hit Bool Triggered");
                // Invoke("stopTakingDamage", .1f);
            }
            //print("DAMAGE");
            if (currentHealth <= 0){
                if (enemyController != null)
                    enemyController.setCanMove(false);
                death();
                Debug.Log("Past Death");
            
            }
        }
    }

    private void showFloatingText(int damage){
        var someText = Instantiate(floatText, thisEnemy.position, Quaternion.identity, thisEnemy);
        someText.GetComponent<TextMesh>().text = damage.ToString();
    }
    // Animation calls calls this function after the hit timeframe has finished.
    // meant to reset the enemy animator back to idle state.
    public void stopTakingDamage(){
        if (this.gameObject.name != "Spawner(Clone)"){

        
        enemyController.setCanMove(true);
        }
        // Debug.Log("Triggered Stop Taking Damage for Enemy");
    }

    // The beginning aspects of the enemy characters death. The character changes layers to unaffect the player. Stop collisions with the player and start the death animation.
    void death(){
        gameObject.layer = 7;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 7;
        Destroy(gameObject.GetComponent<CapsuleCollider2D>());
        if (anim != null) {
            anim.SetTrigger("death");
        }
        // else {
        //     deathDestroy();
        // }
        Debug.Log("before spawner toggle doors");
        if (gameObject.name.Equals("Spawner(Clone)"))
        {
            GameObject.Find("RoomsFirstDungeonGenerator").SendMessage("ToggleDoorsOff");
            Destroy(gameObject);
        }else {
            
            dropItem();
            gameObject.GetComponent<Unit>().SetDead();
            GameObject.Find("StatTracker").GetComponent<StatTracker>().currentEnemies += 1;
            print("ENEMIES KILLED: " + GameObject.Find("StatTracker").GetComponent<StatTracker>().currentEnemies);
        }
        GameObject.Find("PlayerStats").GetComponent<PlayerStatsComponent>().modifyScore(100);
    }

    // After the Death Animation finishes the event calls the deathDestroy function to remove the enemy object from the game world.
    void deathDestroy(){
        Destroy(gameObject);
    }

    void dropItem() {
        int numItems = itemDrops.Count;
        int itemToDrop = Random.Range(0, numItems);

        // Chance to drop nothing
        if (itemToDrop == numItems) {
            return;
        }
        else {
            GameObject temp = Instantiate(itemDrops[itemToDrop], transform.position, transform.rotation);
            if (!isTest) {
                    temp.transform.SetParent(GameObject.Find("SpawnedParent").transform);
            }
        }
    }

    // private void OnTriggerEnter2D(Collider2D other) {
        

    //     if (other.gameObject.tag == "trap"){
    //         this.TakeDamage(10);
    //     }
    //     // Debug.Log("Current Health: "+ this.currentHealth);
    // }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag == "trap"){
            this.TakeDamage(10);
        }
    }
}
