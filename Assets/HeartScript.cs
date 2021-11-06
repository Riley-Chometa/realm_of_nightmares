using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartScript : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
 {
            if (other.gameObject.tag == "Player")
           {
                    Destroy(this.gameObject);
                    print("Heart Collected");
           }
 }

 void OnTriggerEnter2D(Collider2D other) {
     if (other.gameObject.tag == "Player") {
         Destroy(this.gameObject);
         print("Heart Collected");
     }
 }

}
