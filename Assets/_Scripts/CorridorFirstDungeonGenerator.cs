using System;
using System.Collections.Generic;
using System.Numerics;
using System.Collections;
using UnityEngine;
using System.Linq;
using Vector2 = UnityEngine.Vector2;

public class CorridorFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    // the max length of each corridor and how many total corridors will be spawned
    [SerializeField]
    private int corridorLength = 14, corridorCount = 5;
    //the percentage of room floors there will be compared to how many corridor pieces there will be
    [SerializeField]
    [Range(0.1f,1)]
    public float roomPercent = 0.8f;
    //the maximum number of rooms to be created
    [SerializeField]
    private int maxRooms = 5;
    //the players spawn point which will be set to the center of the first room generated
    private Vector2 spawnPoint = Vector2.zero;

    protected override void RunProceduralGeneration()
    {
        CorridorFirstGeneration();
    }

    private void CorridorFirstGeneration()
    {
        spawnPoint = Vector2.zero;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        HashSet<Vector2Int> potentialRoomPositions = new HashSet<Vector2Int>();


        CreateCorridors(floorPositions, potentialRoomPositions);

        HashSet<Vector2Int> roomPositions = CreateRooms(potentialRoomPositions);
        List<Vector2Int> deadEnds = FindAllDeadEnds(floorPositions);
        CreateRoomsAtDeadEnd(deadEnds, roomPositions);

        floorPositions.UnionWith(roomPositions);

        tileMapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tileMapVisualizer);
        SpawnPlayer(spawnPoint);
    }

    private void CreateRoomsAtDeadEnd(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomFloors)
    {
        foreach (var position in roomFloors)
        {
            if (roomFloors.Contains(position) == false)
            {
                var room = RunRandomWalk(randomWalkParameters, position);
                roomFloors.UnionWith(room);
            }
        }
    }

    private List<Vector2Int> FindAllDeadEnds(HashSet<Vector2Int> floorPositions)
    {
        List<Vector2Int> deadEnds = new List<Vector2Int>();
        foreach (var position in floorPositions)
        {
            int neighboursCount = 0;
            foreach (var direction in Direction2D.cardinalDirectionsList)
            {
                if (floorPositions.Contains(position + direction))
                    neighboursCount++;
                
            }
            if (neighboursCount == 1)
                    deadEnds.Add(position);
            
        }
        return deadEnds;
    }

    private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomPositions)
    {
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
        int roomToCreateCount = Mathf.RoundToInt(potentialRoomPositions.Count * roomPercent);

        List<Vector2Int> roomsToCreate = potentialRoomPositions.OrderBy(x => Guid.NewGuid()).Take(roomToCreateCount).ToList();
        int numRooms = 0;
        foreach (var roomPosition in roomsToCreate)
        {
            if (spawnPoint == Vector2.zero)
                spawnPoint = (Vector2) roomPosition;
            var roomFloor = RunRandomWalk(randomWalkParameters, roomPosition);
            roomPositions.UnionWith(roomFloor);
            numRooms++;
            if (numRooms >= maxRooms)
                break;
        }
        return roomPositions;
    }

    private void CreateCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> potentialRoomPositions)
    {
        var currentPosition = startPosition;
        potentialRoomPositions.Add(currentPosition);

        for (int i = 0; i<corridorCount;i++)
        {
            var corridor = ProceduralGenerationAlgorithms.RandomWalkCorridor(currentPosition, corridorLength);
            currentPosition = corridor[corridor.Count-1];
            potentialRoomPositions.Add(currentPosition);
            floorPositions.UnionWith(corridor);
        }
    }
}
