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
    private float timeToSpawn = 4;

    [SerializeField]
    private bool startSpawning = false;


    private int currentEnemies = 0;
    private float currentTimeToSpawn;                       // Trackers to calculate when to spawn
    private int maxSpawnFlag = 0;


    // Update is called once per frame
    void Update()
    {
        if (startSpawning) {
            if (currentTimeToSpawn > 0) {                       // Reduce timer
                currentTimeToSpawn -= Time.deltaTime;
            }   
        else {                                              // Timer is at 0 - Spawn new enemy
            SpawnObject();
            currentTimeToSpawn = timeToSpawn;
            }
        }
    }

    public void ToggleDoorsOff()
    {
        GameObject.Find("RoomsFirstDungeonGenerator").SendMessage("ToggleDoorsOff");
    }

    public void StartSpawning()
    {
        this.startSpawning = true;
    }

    public void SpawnObject() {

        if (maxEnemies != 0 && maxSpawnFlag != 1) {                                                                      // If maximum enemy count isn't reached, spawn enemy

            GameObject child = Instantiate(Enemy, transform.position, UnityEngine.Quaternion.identity) as GameObject;
            //child.transform.parent = transform;
            currentEnemies += 1;

            if (currentEnemies == maxEnemies) {
                maxSpawnFlag = 1;
            }
        }
    }

    public void SetMaxEnemies(int num) {
        maxEnemies = num;
    }

    public void SetTimetoSpawn(int time) {
        timeToSpawn = 1;
    }
}
