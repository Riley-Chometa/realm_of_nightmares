using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Aidan von Holwede
// 11213321
// ajv782
// CMPT 306
// Cody Phillips
// A1

public class Spawner : MonoBehaviour
{

    [Header("Enemy to be Spawned")]                               // Added a header to the inspector - Neat!
    [SerializeField]
    public GameObject Enemy;

    [Header("Enemy Characteristics")]
    [SerializeField]
    private int maxEnemies = 5;                             // Maximum allowable enemies on screen at once

    [SerializeField]
    private float timeToSpawn = 2;
    [SerializeField]
    private float activeTime = 10;
    private float currentTime = 0;

    [SerializeField]
    private bool startSpawning = false;


    private int currentEnemies = 0;
    private float currentTimeToSpawn;                       // Trackers to calculate when to spawn
    private int maxSpawnFlag = 0;
    private float spawnTime = 10.0f;


    // Update is called once per frame
    void Update()
    {
        if (startSpawning) {
            if (currentTime < activeTime) {
                if (currentTimeToSpawn > 0) {                       // Reduce timer
                currentTimeToSpawn -= Time.deltaTime;
                currentTime += Time.deltaTime;
            }   
            else {                                              // Timer is at 0 - Spawn new enemy
                SpawnObject();
                currentTimeToSpawn = timeToSpawn;
                }
            }
            else {
                //gameObject.GetComponent<FireDoorMechanics>().fireExtinguisher();
                ToggleDoorsOff();
                startSpawning = false;
            }
            if (spawnTime <= 0)
            {
                GameObject.Find("RoomsFirstDungeonGenerator").SendMessage("ToggleDoorsOff");
                Destroy(gameObject);
            }
            spawnTime -= Time.deltaTime;
        }

    }

    public void ToggleDoorsOff()
    {
        GameObject.Find("RoomsFirstDungeonGenerator").SendMessage("ToggleDoorsOff");
    }

    public void StartSpawning()
    {
        //GameObject.Find("RoomsFirstDungeonGenerator").SendMessage("ToggleDoorsOn");
        currentTime = 0;
        this.startSpawning = true;
    }

    public void SpawnObject() {
        GameObject child = Instantiate(Enemy, new Vector3(transform.position.x, transform.position.y, -1), UnityEngine.Quaternion.identity); //as GameObject;
    }

    public void SetMaxEnemies(int num) {
        maxEnemies = num;
    }

    public void SetTimetoSpawn(int time) {
        timeToSpawn = time;
    }

    public void SetActiveTime(int time) {
        activeTime = time;
    }
}
