using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid_script : MonoBehaviour
{
    public bool displayGridGizmos;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    Node[,] grid;
    float nodeDiameter;
    int gridSizeX, gridSizeY;

    void Awake(){
         displayGridGizmos = true; 
        // nodeDiameter = 2*nodeRadius;
        // gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);
        // gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);
        // createGrid();
    }

    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        nodeDiameter = 2*nodeRadius;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);
        createGrid();
    }

    public void ResetGrid()
    {
        displayGridGizmos = false; 
        nodeDiameter = 2*nodeRadius;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);
        createGrid();
    }

    public int MaxSize{
        get{
            return gridSizeX * gridSizeY;
        }
    }
    void createGrid(){

        grid = new Node[gridSizeX,gridSizeY];
        Vector2 bottomLeft = new Vector2(transform.position.x - (1 * gridWorldSize.x/2),transform.position.y - 1 * gridWorldSize.y/2);
        
        for (int x = 0; x < gridSizeX; x++){
            for (int y = 0; y < gridSizeY; y++){
                Vector2 worldPoint = bottomLeft + Vector2.right * (x*nodeDiameter + nodeRadius) + Vector2.up* (y*nodeDiameter + nodeRadius);
                bool walkable = !(Physics2D.OverlapCircle(worldPoint, nodeRadius, unwalkableMask));
                grid[x,y] = new Node(walkable, worldPoint,x,y);
            }    
        }
    }
    public Node nodeFromWorldPosition(Vector2 WorldPosition){
        float percentX = (WorldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
        float percentY = (WorldPosition.y + gridWorldSize.y/2) / gridWorldSize.y;
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
        Gizmos.DrawWireCube(transform.position, new Vector2(gridWorldSize.x, gridWorldSize.y));
        if(grid != null && displayGridGizmos){
            foreach (Node n in grid){
                Gizmos.color = (n.walkable)?Color.white:Color.red;
                Gizmos.DrawCube(n.WorldPosition, Vector2.one * (nodeDiameter-.1f));
            }
        }
    
    }
}
