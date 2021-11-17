using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Aidan von Holwede
// 11213321
// ajv782
// CMPT 306
// Cody Phillips
// A1


// Script for an enemy
public class EnemyScript : MonoBehaviour
{

    public int currentHealth;
    public int maxHealth;
    public HealthBar healthBar;                         // The enemy's healthbar
    private Rigidbody2D rb;                             // The collision detection

    private float xvelocity = 0;
    private float yvelocity = 0;

    [SerializeField]
    private Text healthText;                            // Displays current HP to UI

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = Random.Range(100, 300);                     // Generates random maximum health value
        currentHealth = maxHealth;
        healthBar.setMaxHealth(currentHealth);                  // Sets up the healthbar using the maximum health value
        // xvelocity = Random.Range(-2f, 2f);
       //  yvelocity = Random.Range(-2f, 2f);                      // Calculates random directions and speeds for the enemies to move 
        healthText.text = "Current Health: " + maxHealth;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        xvelocity = 0f;
        yvelocity = 0f;                                         // On collision, hault object (not implemented)

    }

    private void FixedUpdate() {
        rb.velocity = new Vector2(xvelocity, yvelocity);        // Updates the enemy's position
    }

    void IncreaseHealth(int newHealth) {                        // Function to increase the enemy's original maxHealth
        maxHealth = newHealth;
        currentHealth = newHealth;
        healthText.text = "Current Health: " + newHealth;
        healthBar.setMaxHealth(newHealth);                      // Also updates the healthBar to the new maximum
    }

    public void Damage(int damage) {                                   // The enemy takes damage
        print("Dealt " + damage + " damage!");
        currentHealth -= damage;
        healthBar.setHealth(currentHealth);
        healthText.text = "Current Health: " + currentHealth;
        
        if (currentHealth <= 0) {                               // If enemy is dead, remove enemy and destroy
            print("Enemy is dead");
            SendMessageUpwards("EnemyRemoval");
            Destroy(gameObject);
        }
    }
}
