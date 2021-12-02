using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartScript : MonoBehaviour
{
private PlayerStatsComponent player;
private GameObject target;

    void Update() {
        target = GameObject.FindGameObjectWithTag("Player");
        if (Mathf.Abs(Vector2.Distance(transform.position, target.transform.position)) < 3) {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, 2.0f * Time.deltaTime);
        }
    }
 void OnTriggerEnter2D(Collider2D other) {

         if (other.gameObject.tag == "Player") {
         player = GameObject.Find("PlayerStats").GetComponent<PlayerStatsComponent>();
         if (this.name == "NewHeart") {
             if (player.getMaxHealth() < 15) {
                 player.modifyMaxHealth(1);
             }
         }
        
        //Destroy(this.gameObject);
        player.modifyHealth(1);
     }
 }

}
