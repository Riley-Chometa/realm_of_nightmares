using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawnTemp : MonoBehaviour
{

    public GameObject enemyToRespawn;
    private Vector3 pos;
    // public float timer = 0;
     public void CreateNewEnemy(Vector3 Position){
        Invoke("createGuy", 2);
        pos = Position;
    
    }

    void createGuy(){
        Debug.Log("CreateGuy called");
        Instantiate(enemyToRespawn, pos, Quaternion.identity);
    }

    
}
