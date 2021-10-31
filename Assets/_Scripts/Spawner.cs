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

    [Header("Enemy Spawner")]                               // Added a header to the inspector - Neat!

    public GameObject Enemy;

    private int maxEnemies = 5;                             // Maximum allowable enemies on screen at once

    private int maxSpawnFlag = 0;

    private int currentEnemies = 0;

    private float timeToSpawn = 4;
    private float currentTimeToSpawn;                       // Trackers to calculate when to spawn



    // Update is called once per frame
    void Update()
    {
    if (currentTimeToSpawn > 0) {                       // Reduce timer
        currentTimeToSpawn -= Time.deltaTime;
        }   
    else {                                              // Timer is at 0 - Spawn new enemy
        SpawnObject();
        currentTimeToSpawn = timeToSpawn;
        }
    }

    public void SpawnObject() {

        if (maxEnemies != 0 && maxSpawnFlag != 1) {                                                                      // If maximum enemy count isn't reached, spawn enemy
            GameObject child = Instantiate(Enemy, transform.position, transform.rotation) as GameObject;
            child.transform.parent = transform;

            currentEnemies += 1;

            if (currentEnemies == maxEnemies) {
                maxSpawnFlag = 1;
            }
        }
    }

    public void EnemyRemoval() {                                                                            // Enemy has died - Readjust the count
        maxEnemies -= 1;
        if (maxEnemies == 0) {
            Destroy(this);
        }
    }
}
