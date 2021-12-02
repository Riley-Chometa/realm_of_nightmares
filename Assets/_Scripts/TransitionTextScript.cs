using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionTextScript : MonoBehaviour
{
    private Text levelText;
    private bool inStore = false;
    //private GameObject generator;
    // Start is called before the first frame update
    void Start()
    {
        this.levelText = gameObject.GetComponent<Text>();
        //this.generator = GameObject.Find("RoomsFirstDungeonGenerator");
    }   
    private void FixedUpdate() {
        this.levelText.text = "Level " + GameObject.Find("RoomsFirstDungeonGenerator").GetComponent<RoomFirstDungeonGenerator>().level;
    }

    public void toggleInStore()
    {
        inStore = !inStore;
    }
}
