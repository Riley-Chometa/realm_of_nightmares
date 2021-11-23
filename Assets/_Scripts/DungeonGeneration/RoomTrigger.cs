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

    public void SetBounds(BoundsInt bounds)
    {
        gameObject.transform.GetComponent<BoxCollider2D>().size = new Vector2((float)(bounds.xMax-bounds.xMin)-9.0f, (float)(bounds.yMax-bounds.yMin)-9.0f);
    }
    
    

    private void OnTriggerEnter2D(Collider2D other) {
        if (!this.entered && other.gameObject.tag == "Player"){
            this.DungeonGenerator.SendMessage("ToggleDoorsOn");
            this.Spawner.SendMessage("StartSpawning");
            this.entered = true;
        }
    }
}
