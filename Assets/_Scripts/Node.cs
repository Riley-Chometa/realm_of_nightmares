using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool walkable;
    public Vector2 WorldPosition;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    public Node parent;

    public Node(bool isWalkable, Vector2 coordinates, int _gridX, int _gridY){
        walkable = isWalkable;
        WorldPosition = coordinates;
        gridX = _gridX;
        gridY = _gridY;
    }

    public int fCost{
        get{
        return gCost + hCost;
        }
    }
}
