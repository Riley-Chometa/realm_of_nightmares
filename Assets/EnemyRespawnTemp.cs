using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawnTemp : MonoBehaviour
{

    public GameObject enemyToRespawn;

    float timer = 0;
     public void CreateNewEnemy(Vector3 Position){
        while (timer < 15.0f){
            timer += Time.fixedDeltaTime;
        }
        Instantiate(enemyToRespawn, Position, Quaternion.identity);
    }
    
}
