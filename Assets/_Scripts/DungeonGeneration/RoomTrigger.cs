using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    private GameObject DungeonGenerator;
    private GameObject Spawner;
    private bool entered = false;
    private void Start() {
        this.DungeonGenerator = GameObject.Find("RoomsFirstDungeonGenerator");
    }

    public void SetSpawner(GameObject spawner)
    {
        this.Spawner = spawner;
    }
    
    

    private void OnTriggerEnter2D(Collider2D other) {
        if (!this.entered){
            this.DungeonGenerator.SendMessage("ToggleDoorsOn");
            this.Spawner.SendMessage("StartSpawning");
            this.entered = true;
        }
    }
}
