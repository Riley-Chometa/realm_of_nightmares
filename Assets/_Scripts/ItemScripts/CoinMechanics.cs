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

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.name == "player"){
            Destroy(this.gameObject);
            smoke = Instantiate(smokeAnimation, tm.position, tm.rotation);
            Destroy(smoke, .75f);
        }
    }
}
