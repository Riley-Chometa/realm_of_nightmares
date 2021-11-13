using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopHeart : MonoBehaviour
{
    private bool canPurchase = false;
    public GameObject shopkeeper;
    public GameObject heartPrefab;
    public PlayerStatsComponent playerInfo;

    
    public GameObject floatingText;
    public GameObject pressE;
    private GameObject tempText;
    private GameObject tempE;
    private string printMe = "Heart";
    public Transform tm;
    private void Start() {
        playerInfo = GameObject.Find("PlayerStats").GetComponent<PlayerStatsComponent>();
    }
    private void Update() {
        if (canPurchase){
            if (Input.GetKeyDown("e")){
                playerInfo.modifyCoins(-10);
                playerInfo.modifyHealth(1);
                if (playerInfo.getCoins() < 10){
                    canPurchase = false;
                }
            }
        }
    }
    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag == "Player"){
            if (playerInfo.getCoins() >= 10){
                canPurchase = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player"){
            shopkeeper.GetComponent<ShopKeeper>().changeText("Heart Cost 10 Coins!");
            shopkeeper.GetComponent<ShopKeeper>().playDialog(1);
            tempText = Instantiate(floatingText, tm.position + new Vector3(0, 1.5f, 0), Quaternion.identity, tm);
            tempText.GetComponent<TextMesh>().text = printMe;
            tempText.GetComponent<TextMesh>().fontSize = 48;
            tempE = Instantiate(pressE, tm.position + new Vector3(0, -1, 0), Quaternion.identity, tm);
            tempE.GetComponent<Transform>().localScale = new Vector3(.2f, .2f, .2f);
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Player"){
            Destroy(tempText, 0.1f);
            Destroy(tempE, .01f);
        }
    }
}

