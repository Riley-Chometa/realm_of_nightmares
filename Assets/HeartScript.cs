using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartScript : MonoBehaviour
{
    private PlayerStatsComponent player;
 void OnTriggerEnter2D(Collider2D other) {
     if (other.gameObject.tag == "Player") {
         player = GameObject.Find("PlayerStats").GetComponent<PlayerStatsComponent>();
         if (player.getHealth() < player.getMaxHealth()){
             Destroy(this.gameObject);
             player.modifyHealth(1);
         }
         
     }
 }

}
