using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
Simple pick up and destroy coin script. Only currently used for showing collisions for pick ups. can be used generally. Coins currently have no value. it will be game dependent.
*/
public class CoinMechanics : MonoBehaviour
{   
    public Transform tm;
    GameObject smoke;
    void Start(){
        tm = GetComponent<Transform>();
    }
    public GameObject smokeAnimation;
    void OnCollisionEnter2D(Collision2D col){
        if (col.gameObject.name == "player"){
            Destroy(gameObject);
            smoke = Instantiate(smokeAnimation, tm.position, tm.rotation);
            Destroy(smoke, .75f);
            GameObject.FindWithTag("CoinUpdater").GetComponent<PlayerStats>().modifyCoins(1);
            GameObject.FindWithTag("CoinUpdater").GetComponent<PlayerStats>().modifyScore(10);
        }
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Collided with player");
        if (other.gameObject.name == "player"){
            Destroy(gameObject);
            smoke = Instantiate(smokeAnimation, tm.position, tm.rotation);
            Destroy(smoke, .75f);
            GameObject.FindWithTag("CoinUpdater").GetComponent<PlayerStats>().modifyCoins(1);
            GameObject.FindWithTag("CoinUpdater").GetComponent<PlayerStats>().modifyScore(10);
        }
    }
}
