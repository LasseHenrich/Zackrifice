using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{

    public int gridX; // x-Position in Node-Array
    public int gridY; // y-Position

    public bool isWall; // is node being obstructed
    public Vector3 position; // world position of node

    public Node parent; // previous node

    public int gCost; // cost of moving from starting point
    public int hCost; // distance to goal

    public int FCost { get { return gCost + hCost; } } // gCost + hCost

    public Node(bool isWall, Vector3 position, int gridX, int gridY) // Constructor
    {
        this.isWall = isWall;
        this.position = position;
        this.gridX = gridX;
        this.gridY = gridY;
    }

}