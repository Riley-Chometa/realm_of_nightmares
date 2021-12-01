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
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name != "TempStoreScene")
        {
            generator.GenerateDungeon();
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player")
        {
            if (SceneManager.GetActiveScene().name == "MainDungeon")
            {
                
                GameObject.Find("StatTracker").GetComponent<StatTracker>().endOfLevel();
                SceneManager.LoadScene("TempStoreScene");
            }
            else if (SceneManager.GetActiveScene().name == "TempStoreScene")
            {
                SceneManager.LoadScene("MainDungeon");
                animator.SetTrigger("StartTransition");
                RoomFirstDungeonGenerator.level++;
                //generator.GenerateDungeon();
            }
            
            // generator.GenerateDungeon();
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}


