using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItems : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Coins"){
            Debug.Log("Found a Coins");
        }
        if (other.gameObject.tag == "PickUps"){
            Debug.Log("Found a pickup!");
        }
    }
}
