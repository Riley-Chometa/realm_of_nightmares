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

    // Collider and animation components.
    public Collider2D mainCollider;
    private Animator anim;
    
    private bool canGetHit;

    // Set starting Variables.
    void Start()
    {
        anim = this.GetComponent<Animator>();
        canGetHit = true;
        currentHealth = maxHealth;
        healthBar.transform.SetParent(this.transform.GetChild(0));
        healthBar.setMaxHealth(maxHealth);
        
    }

    // Take the damage from the player. run hit animation and run death if true.
    public void TakeDamage(int damage){
        if (canGetHit){
            canGetHit = false;
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
    }

    // Animation calls calls this function after the hit timeframe has finished.
    // meant to reset the enemy animator back to idle state.
    public void stopTakingDamage(){
        anim.SetBool("hit", false);
        canGetHit = true;
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
            GameObject.Find("RoomsFirstDungeonGenerator").SendMessage("ToggleDoorsOff");
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
            temp.transform.SetParent(GameObject.Find("SpawnedParent").transform);
        }

    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Collided with enemy000");

        if (other.gameObject.tag == "trap"){
            this.TakeDamage(10);
        }
        Debug.Log("Current Health: "+ this.currentHealth);
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag == "trap"){
            this.TakeDamage(10);
        }
    }
}
