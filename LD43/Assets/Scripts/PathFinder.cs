using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFinder : MonoBehaviour
{

    public static PathFinder instance;

    Grid_Own grid;

    private void Awake()
    {
        instance = this;

        grid = GetComponent<Grid_Own>();
    }

    public List<Node> FindPath(Vector3 a_startPos, Vector3 a_targetPos)
    {
        Node startNode = grid.NodeFromWorldPosition(WorldToNodeWorldPos(a_startPos));
        Node targetNode = grid.NodeFromWorldPosition(WorldToNodeWorldPos(a_targetPos));
        //Debug.Log("a_target: " + a_targetPos + " , my : " + targetNode.position);

        List<Node> openList = new List<Node>();
        HashSet<Node> closeList = new HashSet<Node>();

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].FCost < currentNode.FCost || openList[i].FCost == currentNode.FCost && openList[i].hCost < currentNode.hCost)
                {
                    currentNode = openList[i];
                }
            }
            openList.Remove(currentNode);
            closeList.Add(currentNode);

            if (currentNode == targetNode)
            {
                return GetFinalPath(startNode, targetNode);
            }

            foreach (Node neighborNode in grid.GetNeighboringNodes(currentNode))
            {

                if ((neighborNode.isWall && neighborNode != targetNode) || closeList.Contains(neighborNode)) // Falls (Wand die nicht Target ist) oder Node schon gecheckt wurde
                {
                    continue;
                }

                int moveCost = currentNode.gCost + GetManhattenDistance(currentNode, neighborNode);

                if (moveCost < neighborNode.gCost || !openList.Contains(neighborNode))
                {
                    neighborNode.gCost = moveCost;
                    neighborNode.hCost = GetManhattenDistance(neighborNode, targetNode);
                    neighborNode.parent = currentNode;

                    if (!openList.Contains(neighborNode))
                    {
                        openList.Add(neighborNode);
                    }
                }

            }

        }
        Debug.Log("d");
        return null;
    }

    public Vector2 WorldToNodeWorldPos(Vector2 pos)
    {
        float iX = System.Convert.ToInt32(pos.x);
        float iY = System.Convert.ToInt32(pos.y);

        if (iX < pos.x) // abgerundet
            iX += 0.5f; // Hardcoded
        else if(iX > pos.x)
            iX -= 0.5f;

        if (iY < pos.y) // abgerundet
            iY += 0.5f; // Hardcoded
        else if (iY > pos.y)
            iY -= 0.5f;
        return new Vector2(iX, iY);
    }

    List<Node> GetFinalPath(Node a_startingNode, Node a_endNode)
    {
        List<Node> finalPath = new List<Node>();
        Node currentNode = a_endNode;

        while (currentNode != a_startingNode)
        {
            finalPath.Add(currentNode);
            currentNode = currentNode.parent;
        }

        finalPath.Reverse();

        grid.finalPath = finalPath;

        return finalPath;
    }

    int GetManhattenDistance(Node a_nodeA, Node a_nodeB)
    {
        int ix = Mathf.Abs(a_nodeA.gridX - a_nodeB.gridX);
        int iy = Mathf.Abs(a_nodeA.gridY - a_nodeB.gridY);

        return ix + iy;
    }
}
