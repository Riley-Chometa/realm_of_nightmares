using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionTextScript : MonoBehaviour
{
    private Text levelText;
    //private GameObject generator;
    // Start is called before the first frame update
    void Start()
    {
        this.levelText = gameObject.GetComponent<Text>();
        //this.generator = GameObject.Find("RoomsFirstDungeonGenerator");
    }   

    // Update is called once per frame
    void FixedUpdate()
    {
        this.levelText.text = "Level " + RoomFirstDungeonGenerator.level;
    }
}
