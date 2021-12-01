using System.Numerics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using UnityEngine.SceneManagement;

public class AutoSpawnDungeon : MonoBehaviour
{
    [SerializeField]
    private RoomFirstDungeonGenerator generator;
    [SerializeField]
    private Animator animator;
    private static bool inStore = false;
    private GameObject Store;
    private GameObject player;
    private TransitionTextScript transitionText;
    // Start is called before the first frame update
    void Start()
    {
        transitionText = GameObject.Find("SpawnDungeon").GetComponentInChildren<TransitionTextScript>();
        //transitionText.SetLevelText();
        Store = GameObject.Find("StorePlayerSpawn");
        player = GameObject.Find("player");
        generator = GameObject.Find("RoomsFirstDungeonGenerator").GetComponent<RoomFirstDungeonGenerator>();
        generator.GenerateDungeon();
        
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player")
        {
            transitionText.toggleInStore();
            if (!inStore)
            {
                inStore = true;
                GameObject.Find("StatTracker").GetComponent<StatTracker>().endOfLevel();
                //SceneManager.LoadScene("TempStoreScene");
                player.transform.position = new Vector3(Store.transform.position.x, Store.transform.position.y, 0);
                animator.SetTrigger("StartTransition");
            }
            else if (inStore)
            {
                generator.level++;
                inStore = false;
                animator.SetTrigger("StartTransition");
                generator.GenerateDungeon();
                //transitionText.toggleInStore();
            }
            
            // generator.GenerateDungeon();
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}


