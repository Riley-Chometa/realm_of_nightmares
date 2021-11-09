using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PickUpItems : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement playerOperator;
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
    private float timerValue = 3.0f;
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


    private void Start() {
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
        if (other.gameObject.tag == "Coins"){
            Debug.Log("Found a Coins");
            Destroy(other.gameObject);
            // smoke = Instantiate(smokeAnimation, tm.position, tm.rotation);
            // Destroy(smoke, .75f);
            audioSource.PlayOneShot(coinPickUp);
            GameObject.FindWithTag("CoinUpdater").GetComponent<PlayerStats>().modifyCoins(1);
            GameObject.FindWithTag("CoinUpdater").GetComponent<PlayerStats>().modifyScore(10);
        }
        else if (other.gameObject.tag == "torch"){
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
        // use this for inventory type storing itmes.

        else if (other.gameObject.tag == "PickUps"){
            
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag == "trap"){
            playerOperator.getHit();
        }
        else if (other.gameObject.tag == "BasicBow"){
            playerOperator.canPickUp = true;
            playerOperator.itemToPickUp = other.gameObject;
        }
        else if (other.gameObject.tag == "FireBow")
        {
            playerOperator.canPickUp = true;
            playerOperator.itemToPickUp = other.gameObject;

        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "BasicBow"){
            playerOperator.canPickUp = false;
        }
    }
}
