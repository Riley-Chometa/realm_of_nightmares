using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawnTemp : MonoBehaviour
{

    public GameObject enemyToRespawn;

    float timer = 0;
     public void CreateNewEnemy(Vector3 Position){
<<<<<<< HEAD
        while (timer < 15.0f){
=======
        while (timer < 5.0f){
>>>>>>> master
            timer += Time.fixedDeltaTime;
        }
        Instantiate(enemyToRespawn, Position, Quaternion.identity);
    }
    
}
