using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopGoldHeart : MonoBehaviour
{
    private bool canPurchase = false;
    public GameObject shopkeeper;

    public PlayerStatsComponent playerInfo;

    
    public GameObject floatingText;
    public GameObject pressE;
    private GameObject tempText;
    private GameObject tempE;
    private string printMe = "Increase Max Health";
    public Transform tm;
    private AudioSource AudioSource;
    public AudioClip clip;
    private void Start() {
        AudioSource = GetComponent<AudioSource>();
        playerInfo = GameObject.Find("PlayerStats").GetComponent<PlayerStatsComponent>();
    }
    private void Update() {
        if (canPurchase){
            if (Input.GetKeyDown("e")){
                playerInfo.modifyCoins(-100);
                playerInfo.modifyMaxHealth(1);
                AudioSource.PlayOneShot(clip);
                if (playerInfo.getCoins() < 100){
                    canPurchase = false;
                }
                shopkeeper.GetComponent<ShopKeeper>().changeText("Purchase Complete!");
            }
        }
    }
    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag == "Player"){
            if (playerInfo.getCoins() >= 100 && playerInfo.getMaxHealth() < 15){
                canPurchase = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player"){
            shopkeeper.GetComponent<ShopKeeper>().startDialog(1,"Cost 100 Coins!" );
            shopkeeper.GetComponent<ShopKeeper>().changeText("Heart Cost 100 Coins!");
            // shopkeeper.GetComponent<ShopKeeper>().playDialog(1);
            tempText = Instantiate(floatingText, tm.position + new Vector3(0, 1.5f, 0), Quaternion.identity, tm);
            tempText.GetComponent<TextMesh>().text = printMe;
            tempText.GetComponent<TextMesh>().fontSize = 18;
            tempText.GetComponent<Transform>().localScale = new Vector3(.15f, .15f, .15f);
            tempE = Instantiate(pressE, tm.position + new Vector3(0, -1, 0), Quaternion.identity, tm);
            tempE.GetComponent<Transform>().localScale = new Vector3(.2f, .2f, .2f);
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Player"){
            Destroy(tempText, 0.1f);
            Destroy(tempE, .01f);
            shopkeeper.GetComponent<ShopKeeper>().stopDialog();
            canPurchase = false;
        }
    }
}
