using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;

public class SimpleRandomWalkDungeonGenerator : AbstractDungeonGenerator
{


    [SerializeField]
    protected SimpleRandomWalkSO randomWalkParameters;


    protected override void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floorPositions = RunRandomWalk(randomWalkParameters, startPosition);
        tileMapVisualizer.Clear();
        tileMapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tileMapVisualizer);
        SpawnPlayer((UnityEngine.Vector2)floorPositions.ElementAt(100));
    }
    protected HashSet<Vector2Int> RunRandomWalk(SimpleRandomWalkSO paramters, Vector2Int position)
    {
        var currentPosition = position;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        for (int i = 0; i < paramters.iterations; i++)
        {
            var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPosition, paramters.walkLength);
            floorPositions.UnionWith(path);
            if (paramters.startRandomlyEachIteration)
                currentPosition = floorPositions.ElementAt(Random.Range(0,floorPositions.Count));
        }
        return floorPositions;
    }

}
