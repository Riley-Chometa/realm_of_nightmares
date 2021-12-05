using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PickUpItems : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement playerOperator;
    [SerializeField]
    private Transform playerTransform;
    // Audio Variables.
    [SerializeField]
    private AudioClip coinPickUp;

    [SerializeField]
    private AudioClip torchPickup;
    [SerializeField]
    private AudioSource audioSource;
    // Light Variables
    [SerializeField]
    private Light2D playerLightRadius;
    private float radiusTimer = 0;
    private float timerValue = 2.5f;
    private float intensityDecrement = .01f;
    [SerializeField]
    private float lightIntensityIncrement = .25f;
    [SerializeField]
    private float minLightIntensity = .40f;
    private float currentIntensity;

    private float minInnerRadius = 1.8f;
    
    private float minOuterRadius = 7.50f;
    [SerializeField]
    private float currInnerRadius;
    [SerializeField]
    private float currOuterRadius;    

    public GameObject playerStats;
    private PlayerStatsComponent playerstatsmodifier;
    private void Start() {
        GameObject tempStats = GameObject.Find("PlayerStats");
        playerstatsmodifier = tempStats.GetComponent<PlayerStatsComponent>();
        currentIntensity = minLightIntensity;
        currInnerRadius = minInnerRadius;
        currOuterRadius = minOuterRadius;
    }

// for light radius change
    private void FixedUpdate() {
        if (radiusTimer <= 0){
            if (currentIntensity > minLightIntensity){
                currentIntensity -= intensityDecrement;
                currOuterRadius -= intensityDecrement;
                currInnerRadius -= intensityDecrement;
                if (currentIntensity < minLightIntensity || currOuterRadius < minOuterRadius || currInnerRadius < minInnerRadius){
                    currentIntensity = minLightIntensity;
                    currInnerRadius = minInnerRadius;
                    currOuterRadius = minOuterRadius;
                }
                playerLightRadius.intensity = currentIntensity;
                playerLightRadius.pointLightInnerRadius = currInnerRadius;
                playerLightRadius.pointLightOuterRadius = currOuterRadius; 
                radiusTimer = timerValue;
            }
        }
        radiusTimer -= Time.fixedDeltaTime;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "BronzeCoin"){
            audioSource.PlayOneShot(coinPickUp);
            playerstatsmodifier.modifyCoins(5);
            playerstatsmodifier.modifyScore(10);
        }
        else if (other.gameObject.tag == "SilverCoin"){
            audioSource.PlayOneShot(coinPickUp);
            playerstatsmodifier.modifyCoins(10);
            playerstatsmodifier.modifyScore(25);
        }
        else if (other.gameObject.tag == "GoldCoin"){
            audioSource.PlayOneShot(coinPickUp);
            playerstatsmodifier.modifyCoins(25);
            playerstatsmodifier.modifyScore(50);
        }
        if (other.gameObject.tag == "torch"){
            Destroy(other.gameObject);
            audioSource.PlayOneShot(torchPickup);
            currentIntensity+= lightIntensityIncrement;
            currInnerRadius+= lightIntensityIncrement;
            currOuterRadius += lightIntensityIncrement;
            if (currentIntensity > 1){
                currentIntensity = 1.0f;
            }
            radiusTimer = timerValue;
            playerLightRadius.intensity = currentIntensity;
            playerLightRadius.pointLightInnerRadius = currInnerRadius;
            playerLightRadius.pointLightOuterRadius = currOuterRadius;
        }
        else if (other.gameObject.tag == "trap"){
            playerOperator.getHit();
        }
        else if (other.gameObject.tag == "Bombs")
        {
            Destroy(other.gameObject);
            // Debug.Log("Found a Bomb");
            audioSource.PlayOneShot(coinPickUp);
            playerstatsmodifier.modifyBombs(1);
            //Destroy(GetComponent<Transform>().GetChild(0).gameObject); //Destroys the Light 2D child object 
        }
        // use this for inventory type storing itmes.

        else if (other.gameObject.tag == "PickUps"){
            
        }

        else if (other.gameObject.tag == "Shield") {
            playerstatsmodifier.activateShield();
            Destroy(other.gameObject);
        }
    }

    public void lowHealthLightEffect(int health){
        if (health < 5){
            playerLightRadius.color = Color.red;
        }
        if (health == 1){
            playerLightRadius.intensity = minLightIntensity;
            playerLightRadius.pointLightInnerRadius = minInnerRadius;
            playerLightRadius.pointLightOuterRadius = minOuterRadius;
        }
        else if (health >= 2 && health < 5){
                currentIntensity -= intensityDecrement* 5;
                currOuterRadius -= intensityDecrement* 5;
                currInnerRadius -= intensityDecrement*5;
                if (currentIntensity < minLightIntensity || currOuterRadius < minOuterRadius || currInnerRadius < minInnerRadius){
                    currentIntensity = minLightIntensity;
                    currInnerRadius = minInnerRadius;
                    currOuterRadius = minOuterRadius;
                }
                playerLightRadius.intensity = currentIntensity;
                playerLightRadius.pointLightInnerRadius = currInnerRadius;
                playerLightRadius.pointLightOuterRadius = currOuterRadius; 
        }
        else {
            playerLightRadius.color = Color.white;
        }
        
    }
    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag == "trap"){
            playerOperator.getHit();
        }
        else if (other.gameObject.tag == "BasicBow" || other.gameObject.tag == "FireBow" || other.gameObject.tag == "FirePotion"){
            playerOperator.canPickUp = true;
            playerOperator.itemToPickUp = other.gameObject;
        }
        else if (other.gameObject.tag == "sword1" 
        || other.gameObject.tag == "sword2" 
        || other.gameObject.tag == "sword3" 
        || other.gameObject.tag == "sword4"
        || other.gameObject.tag == "sword5"
        || other.gameObject.tag == "sword6"){
            playerOperator.canPickUp = true;
            playerOperator.itemToPickUp = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "BasicBow" || other.gameObject.tag == "FireBow" || other.gameObject.tag == "FirePotion"){
            playerOperator.canPickUp = false;
        }
    }
    
}
