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
    private GameObject target;

    void Update() {
        target = GameObject.FindGameObjectWithTag("Player");
        if (Mathf.Abs(Vector2.Distance(transform.position, target.transform.position)) < 2) {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, 3.0f * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player"){
            Destroy(this.gameObject);
            if (gameObject.tag == "GoldCoin") {
                GameObject.Find("StatTracker").GetComponent<StatTracker>().currentGoldCoins += 1;
                //print("GOLD COINS: " + GameObject.Find("StatTracker").GetComponent<StatTracker>().currentGoldCoins);
            }
            if (gameObject.tag == "SilverCoin") {
                GameObject.Find("StatTracker").GetComponent<StatTracker>().currentSilverCoins += 1;
                //print("SILVER COINS: " + GameObject.Find("StatTracker").GetComponent<StatTracker>().currentSilverCoins);
            }
            if (gameObject.tag == "BronzeCoin") {
                GameObject.Find("StatTracker").GetComponent<StatTracker>().currentBronzeCoins += 1;
                //print("BRONZE COINS: " + GameObject.Find("StatTracker").GetComponent<StatTracker>().currentBronzeCoins);
            }
            
            smoke = Instantiate(smokeAnimation, tm.position, tm.rotation);
            Destroy(smoke, .75f);
        }
    }
}
