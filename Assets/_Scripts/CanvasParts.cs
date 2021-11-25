using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasParts : MonoBehaviour
{
    [SerializeField]
    private GameObject staminaBar;
    [SerializeField]
    private GameObject pickUpBar;
    [SerializeField]
    private GameObject coinCounter;
    [SerializeField]
    private GameObject scoreCounter;
    [SerializeField]
    private GameObject keyCounter;
    [SerializeField]
    private GameObject bombCounter;
    [SerializeField]
    private GameObject playerHealth;

    // private void Start() {
        // staminaBar = this.transform.GetChild(0).gameObject;
        // coinCounter = this.transform.GetChild(1).gameObject;
        // scoreCounter = this.transform.GetChild(2).gameObject;
        // keyCounter = this.transform.GetChild(3).gameObject;
        // pickUpBar = this.transform.GetChild(4).gameObject;
        // bombCounter = this.transform.GetChild(5).gameObject;
        // playerHealth = this.transform.GetChild(6).gameObject;
    // }

    public GameObject GetStaminaBar(){
        //Debug.Log("Got Component Stamina:");
        return staminaBar;
    }

    public GameObject GetPickUpBar(){
        // Debug.Log("Got Component PickUpBar");
        return pickUpBar;
    }

    public GameObject GetCoinCounter(){
        // Debug.Log("Got Component CoinCounter");
        return coinCounter;
    }

    public GameObject GetScoreCounter(){
        // Debug.Log("Got Component ScoreCounter");
        return scoreCounter;
    }

    public GameObject GetKeyCounter(){
        // Debug.Log("Got Component KeyCounter");
        return keyCounter;
    }

    public GameObject GetBombCounter(){
        // Debug.Log("Got Component BombCounter");
        return bombCounter;
    }

    public GameObject GetPlayerHealth(){
        // Debug.Log("Got Component PlayerHealth");
        return playerHealth;
    }
}
