using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


[Serializable]
public class Node
{
    public Vector2Int coordinates;
    public bool isWalkable;
    public bool isExplored;
    public bool isPath;
    public int cost;
    public Node connectedTo;

    public Node(Vector2Int coordinates, bool isWalkable = true)
    {
        this.coordinates = coordinates;
        this.isWalkable = isWalkable;
    }

}
