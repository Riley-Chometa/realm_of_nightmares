using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatCanvasScript : MonoBehaviour
{
    [SerializeField]
    GameObject currentEnemies;
    [SerializeField]
    GameObject currentGoldCoins;
    [SerializeField]
    GameObject currentSilverCoins;
    [SerializeField]
    GameObject currentBronzeCoins;
    [SerializeField]
    GameObject thisCanvas;
    

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Destroy(thisCanvas);
        }
    }
}
