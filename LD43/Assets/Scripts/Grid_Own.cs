using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid_Own : MonoBehaviour
{

    public static Grid_Own instance;

    public Transform startPosition;
    public LayerMask wallMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;

    Node[,] grid;
    public List<Node> finalPath;


    float nodeDiameter;
    int gridSizeX, gridSizeY;


    private void Start()
    {
        instance = this;

        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                //Debug.Log(worldPoint);
                bool wall = false;

                if (Physics2D.OverlapCircle(worldPoint, nodeRadius - 0.1f, wallMask)) // -0.1f: Bisschen kleiner, damit bei angrenzenden Blöcken nicht noch die Outline mitgenommen wird
                {
                    //Debug.Log(worldPoint);
                    wall = true;
                }

                grid[x, y] = new Node(wall, worldPoint, x, y);

                /*if(wall)
                    Debug.Log(grid[x, y].position);*/
            }
        }
    }

    public Node NodeFromWorldPosition(Vector2 a_worldPosition)
    {
        float xPoint = ((a_worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x);
        float yPoint = ((a_worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y);

        xPoint = Mathf.Clamp01(xPoint);
        yPoint = Mathf.Clamp01(yPoint);

        int x = Mathf.RoundToInt((gridSizeX - 1) * xPoint);
        int y = Mathf.RoundToInt((gridSizeY - 1) * yPoint);

        //Debug.Log(x + " " + y);
        return grid[x, y];
    }

    public List<Node> GetNeighboringNodes(Node a_node)
    {
        List<Node> NeighboringNodes = new List<Node>();
        int xCheck;
        int yCheck;

        //Debug.Log(a_node.position);

        // Right Side
        xCheck = a_node.gridX + 1;
        yCheck = a_node.gridY;
        if (xCheck >= 0 && xCheck < gridSizeX) // wenn noch in der grid
        {
            if (yCheck >= 0 && yCheck < gridSizeY) // wenn noch in der grid
            {
                //Debug.Log(xCheck + " " + yCheck);
                NeighboringNodes.Add(grid[xCheck, yCheck]);
            }
        }

        // Left Side
        xCheck = a_node.gridX - 1;
        yCheck = a_node.gridY;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighboringNodes.Add(grid[xCheck, yCheck]);
            }
        }

        // Top Side
        xCheck = a_node.gridX;
        yCheck = a_node.gridY + 1;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighboringNodes.Add(grid[xCheck, yCheck]);
            }
        }

        // Bottom Side
        xCheck = a_node.gridX;
        yCheck = a_node.gridY - 1;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighboringNodes.Add(grid[xCheck, yCheck]);
            }
        }

        /*
        // Up-Right Side
        xCheck = a_node.gridX + 1;
        yCheck = a_node.gridY + 1;
        if (xCheck >= 0 && xCheck < gridSizeX) // wenn noch in der grid
        {
            if (yCheck >= 0 && yCheck < gridSizeY) // wenn noch in der grid
            {
                //Debug.Log(xCheck + " " + yCheck);
                NeighboringNodes.Add(grid[xCheck, yCheck]);
            }
        }

        // Up-Left Side
        xCheck = a_node.gridX - 1;
        yCheck = a_node.gridY + 1;
        if (xCheck >= 0 && xCheck < gridSizeX) // wenn noch in der grid
        {
            if (yCheck >= 0 && yCheck < gridSizeY) // wenn noch in der grid
            {
                //Debug.Log(xCheck + " " + yCheck);
                NeighboringNodes.Add(grid[xCheck, yCheck]);
            }
        }

        // Bottom-Right Side
        xCheck = a_node.gridX + 1;
        yCheck = a_node.gridY - 1;
        if (xCheck >= 0 && xCheck < gridSizeX) // wenn noch in der grid
        {
            if (yCheck >= 0 && yCheck < gridSizeY) // wenn noch in der grid
            {
                //Debug.Log(xCheck + " " + yCheck);
                NeighboringNodes.Add(grid[xCheck, yCheck]);
            }
        }

        // Bottom-Left Side
        xCheck = a_node.gridX - 1;
        yCheck = a_node.gridY - 1;
        if (xCheck >= 0 && xCheck < gridSizeX) // wenn noch in der grid
        {
            if (yCheck >= 0 && yCheck < gridSizeY) // wenn noch in der grid
            {
                //Debug.Log(xCheck + " " + yCheck);
                NeighboringNodes.Add(grid[xCheck, yCheck]);
            }
        }
        */
        // ausgeklammert, da Entity so an Ecken hängen bleiben kann

        return NeighboringNodes;
    }

    public Vector3 GetRandomNode()
    {
        List<Node> theNodes = new List<Node>();
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                if (!grid[x, y].isWall)
                {
                    theNodes.Add(grid[x, y]);
                }
            }
        }
        Node node = theNodes[Mathf.RoundToInt(Random.Range(0, theNodes.Count - 1))];
        return node.position;
    }

    public Vector2 WorldToTile(Vector2 worldPos)
    {
        float x = System.Convert.ToInt32(worldPos.x);
        float y = System.Convert.ToInt32(worldPos.y);

        if (worldPos.x < x) // aufgerundet
            x -= nodeRadius;
        else // abgerundet
            x += nodeRadius;

        if (worldPos.x < y) // aufgerundet
            y -= nodeRadius;
        else // abgerundet
            y += nodeRadius;

        return new Vector2(x, y);
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 0));

        if(grid != null)
        {
            foreach(Node node in grid)
            {
                if (node.isWall)
                {
                    Gizmos.color = Color.yellow;
                }
                else
                {
                    Gizmos.color = Color.white;
                }

                if(finalPath != null)
                {
                    if (finalPath.Contains(node))
                    {
                        Gizmos.color = Color.red;
                    }
                }

                Gizmos.DrawCube(node.position, Vector3.one * (nodeDiameter - distance));
            }
        }
    }*/

}