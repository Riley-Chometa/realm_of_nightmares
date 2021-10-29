using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSpawnDungeon : MonoBehaviour
{
    [SerializeField]
    private RoomFirstDungeonGenerator generator;
    // Start is called before the first frame update
    void Start()
    {
        generator.GenerateDungeon();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player")
        {
            generator.GenerateDungeon();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
