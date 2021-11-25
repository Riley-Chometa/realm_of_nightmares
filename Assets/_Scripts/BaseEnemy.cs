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
    public Animator anim;
    

    // Set starting Variables.
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.transform.SetParent(this.transform.GetChild(0));
        healthBar.setMaxHealth(maxHealth);
        
    }

    // Take the damage from the player. run hit animation and run death if true.
    public void TakeDamage(int damage){
        currentHealth -= damage;
        healthBar.setHealth(currentHealth);
        if (anim != null) {
            anim.SetBool("hit", true);
        }
        //print("DAMAGE");
        if (currentHealth <= 0){
            death();
        }
    }

    // Animation calls calls this function after the hit timeframe has finished.
    // meant to reset the enemy animator back to idle state.
    public void stopTakingDamage(){
        anim.SetBool("hit", false);
    }

    // The beginning aspects of the enemy characters death. The character changes layers to unaffect the player. Stop collisions with the player and start the death animation.
    void death(){
        gameObject.layer = 7;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 7;
        Destroy(gameObject.GetComponent<CapsuleCollider2D>());
        if (anim != null) {
            anim.SetTrigger("death");
        }
        else {
            deathDestroy();
        }
        if (gameObject.name.Equals("Spawner(Clone)"))
        {
            GameObject.Find("StatTracker").GetComponent<StatTracker>().currentSpawners += 1;
            print("SPAWNERS KILLED: " + GameObject.Find("StatTracker").GetComponent<StatTracker>().currentSpawners);
            GameObject.Find("RoomsFirstDungeonGenerator").SendMessage("ToggleDoorsOff");
        }
        else {
            GameObject.Find("StatTracker").GetComponent<StatTracker>().currentEnemies += 1;
            print("ENEMIES KILLED: " + GameObject.Find("StatTracker").GetComponent<StatTracker>().currentEnemies);
        }
        GameObject.Find("PlayerStats").GetComponent<PlayerStatsComponent>().modifyScore(100);
        dropItem();
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
            if (isTest) {
                    temp.transform.SetParent(GameObject.Find("SpawnedParent").transform);
            }
        }
    }
}
