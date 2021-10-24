using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid_script : MonoBehaviour
{
    bool displayGridGizmos;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    Node[,] grid;
    float nodeDiameter;
    int gridSizeX, gridSizeY;
    Vector3 bottomLeft;

    void Awake(){
        displayGridGizmos = false;
        nodeDiameter = 2*nodeRadius;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);
        createGrid();
        
    }
    void createGrid(){
        grid = new Node[gridSizeX,gridSizeY];
        Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2;
        
        for (int x = 0; x < gridSizeX; x++){
            for (int y = 0; y < gridSizeY; y++){
                Vector3 worldPoint = bottomLeft + Vector3.right * (x*nodeDiameter + nodeRadius) + Vector3.forward* (y*nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                grid[x,y] = new Node(walkable, worldPoint,x,y);
            }    
        }
    }
    public Node nodeFromWorldPosition(Vector3 WorldPosition){
        float percentX = (WorldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
        float percentY = (WorldPosition.z + gridWorldSize.y/2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt(percentX * (gridSizeX- 1));
        int y = Mathf.RoundToInt(percentY * (gridSizeY- 1));

        return grid[x,y];
    }

    public List<Node> getNeighbours(Node node){
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++){
            for (int y = -1; y <= 1; y++){
                if (x == 0 && y == 0){
                    continue;
                }
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY){
                    neighbours.Add(grid[checkX, checkY]);
                }
            }   
        }
        return neighbours;
    }

    void OnDrawGizmos(){
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        if(grid != null && displayGridGizmos){
            foreach (Node n in grid){
                Gizmos.color = (n.walkable)?Color.white:Color.red;
                Gizmos.DrawCube(n.WorldPosition, Vector3.one * (nodeDiameter-.1f));
            }
        }
    
    }
}
