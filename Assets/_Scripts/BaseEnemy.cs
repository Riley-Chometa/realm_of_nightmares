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

    // Collider and animation components.
    public Collider2D mainCollider;
    public Animator anim;

    // Set starting Variables.
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Take the damage from the player. run hit animation and run death if true.
    public void TakeDamage(int damage){
        currentHealth -= damage;
        anim.SetBool("hit", true);
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
        anim.SetTrigger("death");
    }

    // After the Death Animation finishes the event calls the deathDestroy function to remove the enemy object from the game world.
    void deathDestroy(){
        Destroy(gameObject);
    }
}