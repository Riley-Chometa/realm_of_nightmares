using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shopWeaponBuy : MonoBehaviour
{
    private bool canPurchase = false;
    public GameObject shopkeeper;
    public Sprite[] swords;
    public PlayerStatsComponent playerInfo;
    private PlayerMovement playerController;

    private int PurchasePrice;
    private int level = 0;
    public GameObject floatingText;
    public GameObject pressE;
    private GameObject tempText;
    private GameObject tempE;
    private string printMe = "Damage: ";
    private int[] swordPickUpDamages = {25, 30, 40, 50, 75, 80};
    private int[] swordPrices = {150, 200, 250, 300, 350, 500};
    private int damage;
    public Transform tm;
    private SpriteRenderer spriteRenderer;
    private void Start() {
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        playerInfo = GameObject.Find("PlayerStats").GetComponent<PlayerStatsComponent>();
        playerController = GameObject.Find("player").GetComponent<PlayerMovement>();
        setWeaponVariables();
    }
    private void Update() {
        if (canPurchase){
            if (Input.GetKeyDown("e")){
                playerInfo.modifyCoins(-PurchasePrice);
                if (level > 5){
                playerController.setAttackDamage(swordPickUpDamages[5]);
                }
                else {
                    playerController.setAttackDamage(swordPickUpDamages[level]);
                }
                canPurchase = false;
                // Destroy(this.gameObject);
                shopkeeper.GetComponent<ShopKeeper>().changeText("Purchase Complete!");
                level++;
                if (level > 5){
                    Destroy(this.gameObject);
                }
                setWeaponVariables();

            }
        }
    }
    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag == "Player"){
            if (playerInfo.getCoins() >= PurchasePrice){
                canPurchase = true;
            }
        }
    }

    private void setWeaponVariables(){
        // var temp = GameObject.Find("RoomsFirstDungeonGenerator").GetComponent<RoomFirstDungeonGenerator>();
        // level = temp.getLevel();
        
        int playerDamage = playerController.getAttackDamage();
        if (level > 5){
            PurchasePrice = swordPrices[5];
            damage = swordPickUpDamages[5];
            spriteRenderer.sprite = swords[5];
        }
        else{
            Debug.Log("Called for changing sprites");
            PurchasePrice = swordPrices[level];
            damage = swordPickUpDamages[level];
            spriteRenderer.sprite = swords[level];  
        }
        damage = damage - playerDamage;
        if (damage >= 0){
            printMe = "+" + damage + " Damage!";
        }
        else {
            printMe = "-" + damage + " Damage....";
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // Debug.Log("Collided with player!");
        if (other.gameObject.tag == "Player"){
            shopkeeper.GetComponent<ShopKeeper>().startDialog(1,"Sword Costs " + PurchasePrice + " Coins!" );
            shopkeeper.GetComponent<ShopKeeper>().changeText("Sword Costs " + PurchasePrice + " Coins!");
            // shopkeeper.GetComponent<ShopKeeper>().playDialog(1);
            tempText = Instantiate(floatingText, tm.position + new Vector3(0, 1.5f, 0), Quaternion.identity, tm);
            tempText.GetComponent<TextMesh>().text = printMe;
            tempText.GetComponent<TextMesh>().fontSize = 24;
            if (damage < 0){
                tempText.GetComponent<TextMesh>().color = Color.red;
            }
            tempE = Instantiate(pressE, tm.position + new Vector3(0, -1, 0), Quaternion.identity, tm);
            tempE.GetComponent<Transform>().localScale = new Vector3(.1f, .1f, .1f);
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
