using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartScript : MonoBehaviour
{
    private PlayerStatsComponent player;
 void OnTriggerEnter2D(Collider2D other) {
     if (other.gameObject.tag == "Player") {
         player = GameObject.Find("PlayerStats").GetComponent<PlayerStatsComponent>();
         if (this.name == "NewHeart") {
             if (player.getMaxHealth() < 15) {
                 player.modifyMaxHealth(1);
                 player.modifyHealth(1);
             }
             Destroy(this.gameObject);
             return;
         }
         if (player.getHealth() < player.getMaxHealth()){
             Destroy(this.gameObject);
             player.modifyHealth(1);
         }
         
     }
 }

}
