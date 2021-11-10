using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public bool walkable;
    public Vector2 WorldPosition;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    public Node parent;

    int heapIndex;

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

    public int HeapIndex{
        get{
            return heapIndex;
        }
        set{
            heapIndex = value;
        }
    }

    public int CompareTo(Node node){
        int compare = fCost.CompareTo(node.fCost);
        if(compare == 0){
            compare = hCost.CompareTo(node.hCost);
        }

        return -compare;
    }
}
