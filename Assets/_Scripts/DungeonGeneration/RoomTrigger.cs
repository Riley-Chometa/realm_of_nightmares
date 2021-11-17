using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    private GameObject DungeonGenerator;
    private bool entered = false;
    private void Start() {
        this.DungeonGenerator = GameObject.Find("RoomsFirstDungeonGenerator");
    }
    
    

    private void OnTriggerEnter2D(Collider2D other) {
        if (!this.entered){
            this.DungeonGenerator.SendMessage("ToggleDoorsOn");
            this.entered = true;
        }
    }
}
