using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItems : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Coins"){
            Debug.Log("Found a Coins");
            Destroy(other.gameObject);
            // smoke = Instantiate(smokeAnimation, tm.position, tm.rotation);
            // Destroy(smoke, .75f);
            GameObject.FindWithTag("CoinUpdater").GetComponent<PlayerStats>().modifyCoins(1);
            GameObject.FindWithTag("CoinUpdater").GetComponent<PlayerStats>().modifyScore(10);
        }
        if (other.gameObject.tag == "PickUps"){
            Debug.Log("Found a pickup!");
        }
    }
}
