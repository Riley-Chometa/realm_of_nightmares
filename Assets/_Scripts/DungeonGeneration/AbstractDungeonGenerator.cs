using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractDungeonGenerator : MonoBehaviour
{
    [SerializeField]
    protected TileMapVisualizer tileMapVisualizer = null;
    [SerializeField]
    protected Vector2Int startPosition = Vector2Int.zero;
   
    // public GameObject player;

    // public GameObject tempEnemy;
    // public GameObject tempEnd;
    // public GameObject tempCoin;
    // public GameObject tempKey;
    // public GameObject tempLock;



    public void GenerateDungeon()
    {
        tileMapVisualizer.Clear();
        RunProceduralGeneration();
    }

    // public void SpawnPlayer(Vector2 newLocation)
    // {
    //     // foreach(GameObject enemy in  GameObject.FindGameObjectsWithTag("Enemy"))
    //     // {
    //     //     Destroy(enemy);
    //     // }
    //     // foreach(GameObject coin in  GameObject.FindGameObjectsWithTag("Coins"))
    //     // {
    //     //     Destroy(coin);
    //     // }
    //     // Instantiate(player, new Vector3(newLocation.x, newLocation.y, -1), player.transform.rotation);
    //     player.transform.position = new Vector3(newLocation.x, newLocation.y, -1);
    //     // Instantiate(tempEnemy,new Vector3(newLocation.x+5, newLocation.y, -1),Quaternion.identity);
    //     // tempEnemy.transform.position = new Vector3(newLocation.x+5, newLocation.y, -1);
    //     // tempEnd.transform.position = new Vector3(newLocation.x-5, newLocation.y, -1);
    //     // Instantiate(tempCoin, new Vector3(newLocation.x, newLocation.y+5, -1),Quaternion.identity);        
    //     // Instantiate(tempKey, new Vector3(newLocation.x, newLocation.y-5, -1),Quaternion.identity);
    //     // Instantiate(tempLock, new Vector3(newLocation.x-5, newLocation.y-5, -1),Quaternion.identity);


    // }

    protected abstract void RunProceduralGeneration();
}
