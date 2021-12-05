using System.IO;
using System.Net.Sockets;
using System.Globalization;
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
    [SerializeField]
    private int roomPrefabCount = 0;
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
    private HashSet<Vector2Int> floor;
    [SerializeField]
    private GameObject doorPrefab;
    private HashSet<GameObject> doors;
    [SerializeField]
    private GameObject RoomTriggerPrefab;
    public int level = 1;
    private GameObject ParentSpawn;
    [SerializeField]
    private GameObject FloorTrap;
    [SerializeField]
    private GameObject BarrelLight;
    [SerializeField]
    private GameObject Coin;
    [SerializeField]
    private GameObject aStar;
    [SerializeField]
    private GameObject Bomb;

    public void GenerateDungeon()
    {
        Destroy(GameObject.Find("Astar(Clone)"));
        CreateRooms();
    }

    // protected override void RunProceduralGeneration()
    // {
    //     CreateRooms();
    // }

    public int getLevel(){
        return level;
    }

    private void CreateRooms()
    {
        //level++;
        tileMapVisualizer.Clear();
        var roomsList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(
                    new BoundsInt((Vector3Int) startPosition, new Vector3Int(dungeonWidth, dungeonHeight, 0)), 
                    minRoomWidth, minRoomHeight);
        
        floor = new HashSet<Vector2Int>();
        doors = new HashSet<GameObject>();
        this.ParentSpawn = GameObject.Find("SpawnedParent");
        
        //this.maxRooms = difficulty;
        if (randomWalkRooms)
        {
            floor = CreateRoomsRandomly(roomsList);
        }
        else{
            floor = CreateSimpleRooms(roomsList);
        }
        ClearDungeon();
        List<Vector2Int> roomCenters = new List<Vector2Int>();
        foreach (var room in roomsList)
        {
            roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
        }
        HashSet<Vector2Int> corridors = ConnectRooms(roomCenters);
        floor.UnionWith(corridors);
        

        tileMapVisualizer.PaintFloorTiles(floor);
        WallGenerator.CreateWalls(floor, tileMapVisualizer);
        SpawnRoomsAssets();
        //System.Threading.Thread.Sleep(5000);
        GameObject a = Instantiate(this.aStar);//.SendMessage("ResetGrid");
        //a.SendMessage("ResetGrid");
    }


    private void ClearDungeon()
    {
        //GameObject.Find("Astar").SendMessage("ResetGrid");
        
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(enemy);
        }
        foreach (Transform child in this.ParentSpawn.transform)
        {
            Destroy(child.gameObject);
        }
        DestroyDoors();
        
    }

    private void SpawnRoomsAssets()
    {
        foreach (var room in this.dungeon.rooms)
        {
            if (room.roomType == RoomType.Spawn){
                this.Player.transform.position = new Vector3(room.roomBounds.center.x, room.roomBounds.center.y, -1);
                this.dungeon.startRoom = room;
                GameObject randomRoomPrefab = Instantiate(GetRandomStartRoomPrefab(), new Vector3(room.roomBounds.center.x, room.roomBounds.center.y, 0),UnityEngine.Quaternion.identity);
                randomRoomPrefab.transform.SetParent(this.ParentSpawn.transform);
            }
            else if (room.roomType == RoomType.End) {
                this.DungeonEndPoint.transform.position = new Vector3(room.roomBounds.center.x, room.roomBounds.center.y, -1);
                GameObject randomRoomPrefab = Instantiate(GetRandomEndRoomPrefab(), new Vector3(room.roomBounds.center.x, room.roomBounds.center.y, 0),UnityEngine.Quaternion.identity);
                randomRoomPrefab.transform.SetParent(this.ParentSpawn.transform);
            }
            else if (room.roomType == RoomType.Normal){
                GameObject spawner = Instantiate(this.EnemySpawner,new Vector3(room.roomBounds.center.x, room.roomBounds.center.y, 0),UnityEngine.Quaternion.identity);
                GameObject trigger = Instantiate(this.RoomTriggerPrefab,new Vector3(room.roomBounds.center.x, room.roomBounds.center.y, -1),UnityEngine.Quaternion.identity);
                trigger.SendMessage("SetSpawner", spawner);
                trigger.SendMessage("SetBounds", room.roomBounds);
                spawner.transform.SetParent(this.ParentSpawn.transform);
                trigger.transform.SetParent(this.ParentSpawn.transform);
                GameObject randomRoomPrefab = Instantiate(GetRandomRoomPrefab(), new Vector3(room.roomBounds.center.x, room.roomBounds.center.y,-1),UnityEngine.Quaternion.identity);
                randomRoomPrefab.transform.SetParent(this.ParentSpawn.transform);
            }
            //SpawnLights(room);
            //SpawnFloorTraps(room);
        } 
        ToggleDoorsOn();
        ToggleDoorsOff();
        SpawnCoins();
        
    }

    private GameObject GetRandomRoomPrefab()
    {
        
        int roomNumber = Random.Range(1,this.roomPrefabCount);
        
        String roomName = "RoomPrefabs/RoomPrefab" + roomNumber;
        if (Random.Range(1,100)<=level*3)
        {
            roomName += "lvl2";
        }
        GameObject loadedRoomPrefab = (GameObject)Resources.Load(roomName);
        if (loadedRoomPrefab == null)
        {
            throw new FileNotFoundException("RoomPrefab" + roomNumber + " not found, please check the Assets/Resources/RoomPrefabs/ folder to ensure the file exists.");
        }
        return loadedRoomPrefab;
    }

    private GameObject GetRandomStartRoomPrefab()
    {
        int roomNumber = Random.Range(1,4);
        GameObject loadedRoomPrefab = (GameObject)Resources.Load("RoomPrefabs/StartRoomPrefab" + roomNumber);
        if (loadedRoomPrefab == null)
        {
            throw new FileNotFoundException("startRoomPrefab" + roomNumber + " not found, please check the Assets/Resources/RoomPrefabs/ folder to ensure the file exists.");
        }
        return loadedRoomPrefab;
    }

    private GameObject GetRandomEndRoomPrefab()
    {
        int roomNumber = Random.Range(1,4);
        GameObject loadedRoomPrefab = (GameObject)Resources.Load("RoomPrefabs/EndRoomPrefab" + roomNumber);
        if (loadedRoomPrefab == null)
        {
            throw new FileNotFoundException("startRoomPrefab" + roomNumber + " not found, please check the Assets/Resources/RoomPrefabs/ folder to ensure the file exists.");
        }
        return loadedRoomPrefab;
    }

 

    private void ToggleDoorsOn()
    {
        DestroyDoors();
        foreach (Room room in this.dungeon.rooms)
        {
            foreach (Corridor corridor in room.corridors)
            {
                foreach (Door door in corridor.doors)
                {
                    GameObject tempDoor = Instantiate(this.doorPrefab,new Vector3(door.position.x, door.position.y, 0),UnityEngine.Quaternion.identity);
                    this.doors.Add(tempDoor);
                    tempDoor.transform.SetParent(gameObject.transform);
                }
            }
        }
    }

    private void SpawnCoins()
    {
        bool skipTile = false;
        foreach (Room room in this.dungeon.rooms)
        {
            foreach (Corridor corridor in room.corridors)
            {
                foreach (Door door in corridor.doors)
                {
                    skipTile = !skipTile;
                    if (!skipTile)//Random.Range(0,99) > 69)
                    {
                        GameObject objectToInstantiate = Random.Range(1,100) >5? this.Coin: this.Bomb;
                        GameObject coin = Instantiate(objectToInstantiate,new Vector3(door.position.x, door.position.y-0.5f, 0),UnityEngine.Quaternion.identity);
                        coin.transform.SetParent(this.ParentSpawn.transform);
                    }
                }
            }
        }
    }

    private void DestroyDoors()
    {
        foreach(Transform child in gameObject.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void ToggleDoorsOff()
    {
        foreach (GameObject door in this.doors)
        {
            door.SendMessage("fireExtinguisher");
        }
        this.doors.Clear();
    }

    private HashSet<Vector2Int> CreateRoomsRandomly(List<BoundsInt> roomsList)
    {
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
        Corridor currentCorridor = new Corridor(this.doorPrefab);
        
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
            if (!floor.Contains(position))
            {
                currentCorridor.AddTile(position);
            }

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
            if (!floor.Contains(position))
            {
                currentCorridor.AddTile(position);
            }
        }
        foreach (Room room in this.dungeon.rooms)
        {
            
            if (room.center == position || room.center == destination)
            {
                room.corridors.Add(currentCorridor);
                room.doors.Add(new Door(this.doorPrefab, FindClosestPointTo(room.center, currentCorridor.floor)));
            }
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

    private FloorTile FindClosestPointTo(Vector2Int currentRoomCenter, HashSet<FloorTile> tiles)
    {
        FloorTile closest = new FloorTile(Vector2Int.zero);
        float distance = float.MaxValue;
        foreach (var tile in tiles)
        {
            float currentDistance = Vector2.Distance(tile.position, currentRoomCenter);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closest = tile;
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
        }
        this.dungeon.SpawnRooms();
        return floor;
    }
}

public class Door
{
    public Vector2 position;
    public FloorTile tile;
    public GameObject doorPrefab;
    public bool open;

    public Door(GameObject doorPrefab, FloorTile tile)
    {
        this.position = new Vector2(tile.position.x+0.5f, tile.position.y+1.25f);
        this.tile = tile;
        this.doorPrefab = doorPrefab;
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
    public Room startRoom;
    
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
        this.LastRoom.center = Vector2Int.RoundToInt((Vector2) roomBounds.center); //new Vector2Int(int.Parse(roomBounds.center.x.ToString()), roomBounds.center.y);
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
    private HashSet<FloorTile> Floor;
    private int MaxSpawners;
    private RoomType RoomType;
    private bool PlayerHasEntered = false;
    private BoundsInt RoomBounds;
    public BoundsInt roomBounds {get; set;}
    public RoomType roomType {get; set;}
    public Dungeon Dungeon;
    public HashSet<Door> doors;
    public HashSet<Corridor> corridors;
    public Vector2Int center;
    public HashSet<FloorTile> floor {get{return this.Floor;}}


    public Room(HashSet<FloorTile> floor, RoomType roomType, Dungeon dungeon)
    {
        this.Floor = floor;
        this.RoomType = roomType;
        this.Dungeon = dungeon;
        this.corridors = new HashSet<Corridor>();
        this.doors = new HashSet<Door>();
    }

    public Room(RoomType roomType, Dungeon dungeon): 
        this(new HashSet<FloorTile>(), roomType, dungeon) {}

    public Room(Dungeon dungeon):
        this(new HashSet<FloorTile>(), RoomType.Normal, dungeon) {}

    public void addTile(Vector2Int position)
    {
        this.Floor.Add(new FloorTile(position));
    }

    public Vector2Int GetCenterTile()
    {
        return (Vector2Int) Vector3Int.RoundToInt(this.roomBounds.center);
    }
    // public HashSet<Vector2Int> GetBounds()
    // {
    //     List<Vector2Int> corners = new List<Vector2Int>();
    //     HashSet<Vector2Int> roomBounds = new HashSet<Vector2Int>();
    //     corners.Add(new Vector2Int(this.roomBounds.xMax, this.roomBounds.yMax));
    //     corners.Add(new Vector2Int(this.roomBounds.xMax, this.roomBounds.yMin));
    //     corners.Add(new Vector2Int(this.roomBounds.xMin, this.roomBounds.yMin));
    //     corners.Add(new Vector2Int(this.roomBounds.xMin, this.roomBounds.yMax));
    //     for (int i = 0;i<corners.Count;i++)
    //     {
    //         Vector2Int currentPosition = corners[i];
    //         //roomBounds.Add(currentPosition);
    //         Vector2Int nextPosition = corners.Count == i+1? corners[0]: corners[i+1];
    //         bool isFromX = currentPosition.x - nextPosition.x != 0;

    //         if (isFromX)
    //         {
    //             for(int walkingInt = currentPosition.x; walkingInt<nextPosition.x; walkingInt++)
    //             {
    //                 // Vector2Int temp = new Vector2Int(walkingInt, currentPosition.y);
    //                 // Debug.Log((new Vector2Int(walkingInt, currentPosition.y)).ToString());
    //                 roomBounds.Add(new Vector2Int(walkingInt, currentPosition.y));
    //             }
    //         }
    //         else
    //         {
    //             for(int walkingInt = currentPosition.y; walkingInt<nextPosition.y; walkingInt++)
    //             {
    //                 //Debug.Log((new Vector2Int(currentPosition.x, walkingInt)).ToString());
    //                 roomBounds.Add(new Vector2Int(currentPosition.x, walkingInt));
    //             }
    //         }

    //     }

    //     return roomBounds;
    // }


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

/**
Author: Riley Chometa
This method will be used to track corridor specific items such as FloorTiles and doorways
*/
public class Corridor
{
    private HashSet<FloorTile> Floor;
    public HashSet<FloorTile> floor {get { return this.Floor; }}
    private HashSet<Room> ConnectingRooms;
    private HashSet<Door> Doors;
    private GameObject doorPrefab;
    public HashSet<Door> doors {get {return this.Doors;}}


    public Corridor(GameObject doorPrefab)
    {
        Floor = new HashSet<FloorTile>();
        ConnectingRooms = new HashSet<Room>();
        this.Doors = new HashSet<Door>();
    }

    public void AddTile(Vector2Int position)
    {
        FloorTile corridorTile = new FloorTile(position);
        this.Floor.Add(corridorTile);
        Door currentDoor = new Door(this.doorPrefab, corridorTile);
        this.Doors.Add(currentDoor);
    }

    public void SpawnDoors()
    {
        foreach (Room room in this.ConnectingRooms)
        {
            //FindClosestDoors(room, Door1);
        } 
    }
    // private void FindClosestTile(Room room, FloorTile doorTile)
    // {
    //     FloorTile closest = null;
    //     foreach (FloorTile tile in this.Floor)
    //     {
    //         closest = closest == null ? tile : closest;

    //         foreach (Vector2Int roomTile in room.GetBounds())
    //         {
    //             if (CompareVector2Int(tile.position, roomTile) < CompareVector2Int(closest.position, roomTile))
    //             {
    //                 closest = tile;
    //             }
    //         }
    //     }
    //     doorTile = closest;
    // }

    private int CompareVector2Int(Vector2Int left, Vector2Int right)
    {
        return (left.y + left.x) - (right.y + right.x);
    }
}

/**
Author: Riley Chometa
The Class will be used to track tile specific items
*/
public class FloorTile
{
    private Vector2Int Position;
    private bool HasObject;
    private GameObject TileObject;

    public Vector2Int position 
    {
        get { return this.Position; }
    }

    public FloorTile(Vector2Int position)
    {
        this.Position = position;
        this.HasObject = false;
    }

}

public enum RoomType {
    Normal, Spawn, End
}