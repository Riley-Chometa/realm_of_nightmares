using System.Runtime.CompilerServices;
using System.Numerics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class RoomFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    //the minimum room width and height that will be allowed
    [SerializeField]
    private int minRoomWidth = 4, minRoomHeight = 4;
    //the width and height of the entire dungeon, this is the starting size before it start to get cut into pieces
    [SerializeField]
    private int dungeonWidth = 20, dungeonHeight = 20;
    // the maximum number of rooms allowed
    [SerializeField]
    private int maxRooms = 20;
    //the number of tiles required between rooms
    [SerializeField]
    [Range(0,10)]
    private int offset = 1;
    // boolean to determine whether a random walk will be used or not,
    // if false the rooms will be more square, if true they will be more random
    [SerializeField]
    private bool randomWalkRooms = false;
    //the spawn point for the player, this will be set to be the first rooms center point
    private Vector2 spawnPoint = Vector2.zero;
    private Dungeon dungeon; 
    [SerializeField]
    private GameObject Player;
    [SerializeField]
    private GameObject DungeonEndPoint;
    [SerializeField]
    private GameObject EnemySpawner;
    private Corridor corridor;

    public void GenerateDungeon()
    {
        CreateRooms();
    }

    protected override void RunProceduralGeneration()
    {
        CreateRooms();
    }

    private void CreateRooms()
    {
        tileMapVisualizer.Clear();
        var roomsList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(
                    new BoundsInt((Vector3Int) startPosition, new Vector3Int(dungeonWidth, dungeonHeight, 0)), 
                    minRoomWidth, minRoomHeight, maxRooms);
        
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

        if (randomWalkRooms)
        {
            floor = CreateRoomsRandomly(roomsList);
        }
        else{
            floor = CreateSimpleRooms(roomsList);
        }

        List<Vector2Int> roomCenters = new List<Vector2Int>();
        foreach (var room in roomsList)
        {
            roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
        }
        HashSet<Vector2Int> corridors = ConnectRooms(roomCenters);
        floor.UnionWith(corridors);
        

        tileMapVisualizer.PaintFloorTiles(floor);
        WallGenerator.CreateWalls(floor, tileMapVisualizer);
        SpawnPlayer();       
    }

    private void SpawnPlayer()
    {
        foreach (var room in this.dungeon.rooms)
        {
            if (room.roomType == RoomType.Spawn)
                this.Player.transform.position = new Vector3(room.roomBounds.center.x, room.roomBounds.center.y, -1);
            else if (room.roomType == RoomType.End)
                this.DungeonEndPoint.transform.position = new Vector3(room.roomBounds.center.x, room.roomBounds.center.y, -1);
            else if (room.roomType == RoomType.Normal)
                Instantiate(this.EnemySpawner,new Vector3(room.roomBounds.center.x, room.roomBounds.center.y, -1),UnityEngine.Quaternion.identity);
        } 
    }
    private HashSet<Vector2Int> CreateRoomsRandomly(List<BoundsInt> roomsList)
    {
        Debug.Log("Hello");
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        for (int i = 0; i < roomsList.Count; i++)
        {
            var roomBounds = roomsList[i];
            var roomCenter = new Vector2Int(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y));
            spawnPoint = new Vector2(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y));
            var roomFloor = RunRandomWalk(randomWalkParameters, roomCenter);
            foreach (var position in roomFloor)
            {
                if (position.x >= (roomBounds.xMin + offset) && position.x <= (roomBounds.xMax - offset) && 
                    position.y >= (roomBounds.yMin - offset) && position.y <= (roomBounds.yMax - offset))
                {
                    floor.Add(position);
                }
            }
            
        }
        return floor;
    }

    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        var currentRoomCenter = roomCenters[Random.Range(0,roomCenters.Count)];
        roomCenters.Remove(currentRoomCenter);

        while(roomCenters.Count > 0)
        {
            Vector2Int closest = FindClosestPointTo(currentRoomCenter, roomCenters);
            roomCenters.Remove(closest);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
            currentRoomCenter = closest;
            corridors.UnionWith(newCorridor);
        }
        return corridors;
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        
        var position = currentRoomCenter;
        corridor.Add(position);
        while (position.y != destination.y)
        {
            if (destination.y > position.y)
            {
                position += Vector2Int.up;
            }
            else if (destination.y < position.y)
            {
                position += Vector2Int.down;
            }
            corridor.Add(position);

        }
        while (position.x != destination.x)
        {
            if (destination.x > position.x)
            {
                position += Vector2Int.right;
            }
            else if (destination.x < position.x)
            {
                position += Vector2Int.left;
            }
            corridor.Add(position);
        }
        return corridor;
    }

    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    {
        Vector2Int closest = Vector2Int.zero;
        float distance = float.MaxValue;
        foreach (var position in roomCenters)
        {
            float currentDistance = Vector2.Distance(position, currentRoomCenter);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closest = position;
            }
        }
        return closest;
    }

    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        this.dungeon = new Dungeon();
        foreach (var room in roomsList)
        {
            this.dungeon.AddRoom();
            for (int column = offset; column < room.size.x-offset; column++)
            {
                for (int row = offset; row < room.size.y-offset; row++)
                {
                    Vector2Int position = (Vector2Int) room.min + new Vector2Int(column, row);
                    floor.Add(position);
                    this.dungeon.lastRoom.addTile(position);
                }
            }
            this.dungeon.AddRoomBounds(room);
            
            // GameObject tempText = new GameObject("Text");
            // UnityEngine.UI.Text temp = tempText.AddComponent<UnityEngine.UI.Text>();
            // temp.text = this.dungeon.rooms.Count.ToString();
            // Instantiate(temp, new UnityEngine.Vector3(this.dungeon.lastRoom.GetCenterTile().x, this.dungeon.lastRoom.GetCenterTile().y, -1),UnityEngine.Quaternion.identity);
            
            
            Debug.Log(this.dungeon.lastRoom.GetCenterTile());
        }
        this.dungeon.SpawnRooms();
        Debug.Log(this.dungeon.rooms.Count);
        return floor;
    }
}


/**
This class will be used to control Dungeon specific things, such as which types of rooms have/will be created etc.
*/
public class Dungeon
{
    private HashSet<Room> Rooms;
    private Room LastRoom;
    public Room lastRoom 
    { 
        get { return this.LastRoom; }
    }
    private bool HasSpawnRoom = false;
    private bool HasEndRoom = false;
    private bool HasStronkRoom = false;
    private int Difficulty;
    public int difficulty {get; set;}
    public HashSet<Room> rooms {get { return this.Rooms;}}
    
    public Dungeon()
    {
        this.Rooms = new HashSet<Room>();
    }

    public void AddRoom()
    {
        this.LastRoom = new Room(this);
        this.rooms.Add(this.LastRoom);
    }

    public void AddRoomBounds(BoundsInt roomBounds)
    {
        this.LastRoom.roomBounds = roomBounds;
    }
    public void SpawnRooms()
    {
        this.rooms.First().roomType = RoomType.Spawn;
        this.rooms.Last().roomType = RoomType.End;
        foreach (var room in this.rooms)
        {
            room.SpawnRoom();
        }
    }
}

/**
Author: Riley Chometa
This method will be used to track room specific items such as FloorTiles and item drops
*/
public class Room
{
    private HashSet<RoomTile> floor;
    private int MaxSpawners;
    private RoomType RoomType;
    private bool PlayerHasEntered = false;
    private BoundsInt RoomBounds;
    public BoundsInt roomBounds {get; set;}
    public RoomType roomType {get; set;}
    public Dungeon Dungeon;


    public Room(HashSet<RoomTile> floor, RoomType roomType, Dungeon dungeon)
    {
        this.floor = floor;
        this.RoomType = roomType;
        this.Dungeon = dungeon;
        //SpawnRoom();
    }

    public Room(RoomType roomType, Dungeon dungeon): 
        this(new HashSet<RoomTile>(), roomType, dungeon) {}

    public Room(Dungeon dungeon):
        this(new HashSet<RoomTile>(), RoomType.Normal, dungeon) {}

    public void addTile(Vector2Int position)
    {
        this.floor.Add(new RoomTile(position));
    }

    public Vector2Int GetCenterTile()
    {
        return (Vector2Int) Vector3Int.RoundToInt(this.roomBounds.center);
    }

    public void SpawnRoom()
    {
        //will change later
        this.MaxSpawners = 1;
        switch (this.RoomType)
        {
            case RoomType.Normal:
                MakeNormalRoom();
                break;
            case RoomType.Spawn:
                MakeSpawnRoom();
                break;
            case RoomType.End:
                MakeEndRoom();
                break;
            default:
                break;
        }
    }

    private void MakeNormalRoom()
    {
        // var position = this.roomBounds.center;
        // this.Dungeon.Player.transform.position = new Vector3(position.x, position.y, -1);
    }

    private void MakeSpawnRoom()
    {

    }

    private void MakeEndRoom()
    {

    }
}

public class Corridor
{
    HashSet<RoomTile> floor;

    public Corridor()
    {
        floor = new HashSet<RoomTile>();
    }

    public void AddTile(Vector2Int position)
    {
        this.floor.Add(new RoomTile(position));
    }
}

/**
Author: Riley Chometa
The Class will be used to track tile specific items
*/
public class RoomTile
{
    private Vector2Int Position;
    private bool HasObject;
    private GameObject TileObject;

    public Vector2Int position 
    {
        get { return this.Position; }
    }

    public RoomTile(Vector2Int position)
    {
        this.Position = position;
        this.HasObject = false;
    }

}

public enum RoomType {
    Normal, Spawn, End
}