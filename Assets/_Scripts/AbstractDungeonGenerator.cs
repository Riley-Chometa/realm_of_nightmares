using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractDungeonGenerator : MonoBehaviour
{
    [SerializeField]
    protected TileMapVisualizer tileMapVisualizer = null;
    [SerializeField]
    protected Vector2Int startPosition = Vector2Int.zero;
   
    public GameObject player;


    public void GenerateDungeon()
    {
        tileMapVisualizer.Clear();
        RunProceduralGeneration();
    }

    public void SpawnPlayer(Vector2 newLocation)
    {
       // Instantiate(player, new Vector3(newLocation.x, newLocation.y, -1), player.transform.rotation);
        player.transform.position = new Vector3(newLocation.x, newLocation.y, -1);
    }

    protected abstract void RunProceduralGeneration();
}
