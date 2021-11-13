using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopHeart : MonoBehaviour
{
    private bool canPuchase = false;
    private void Update() {
        
    }
    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag == "Player"){
            var player = other.gameObject.GetComponent<PlayerMovement>();
            
        }
    }
}

