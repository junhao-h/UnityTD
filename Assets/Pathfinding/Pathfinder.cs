using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    [SerializeField] Vector2Int startCoordinates;
    public Vector2Int StartCoordinates { get { return startCoordinates; } }
    [SerializeField] Vector2Int endCoordinates;
    public Vector2Int EndCoordinates { get { return endCoordinates; } }

    Node startNode;
    Node endNode;
    Node currentSearchNode;

    Vector2Int[] directions = { Vector2Int.left, Vector2Int.up, Vector2Int.right, Vector2Int.down };
    GridManager gridManager;
    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();
    Dictionary<Vector2Int, Node> explored = new Dictionary<Vector2Int, Node>();
    Queue<Node> frontier = new Queue<Node>();

    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        {
            if (gridManager != null)
            {
                grid = gridManager.Grid;
                startNode = grid[startCoordinates];
                endNode = grid[endCoordinates];
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        GetNewPath();
    }

    public List<Node> GetNewPath(Vector2Int coordinates)
    {
        gridManager.ResetNodes();
        BreadthFirstSearch(coordinates);
        return BuildPath();
    }
    public List<Node> GetNewPath()
    {
        return GetNewPath(startCoordinates);
    }

    void ExploreNeightbors()
    {
        List<Node> neighbors = new List<Node>();

        foreach (Vector2Int direction in directions)
        {
            Vector2Int neightborCoords = currentSearchNode.coordinates + direction;

            if (grid.ContainsKey(neightborCoords))
            {
                neighbors.Add(grid[neightborCoords]);
            }
        }

        foreach (Node neighbor in neighbors)
        {
            if (!explored.ContainsKey(neighbor.coordinates) && neighbor.isWalkable)
            {
                neighbor.connectedTo = currentSearchNode;
                explored.Add(neighbor.coordinates, neighbor);
                frontier.Enqueue(neighbor);
            }
        }
    }

    void BreadthFirstSearch(Vector2Int coordinates)
    {
        // Ensure that start and endpoint will always be walkable
        startNode.isWalkable = true;
        endNode.isWalkable = true;
        // Wiped saved info
        frontier.Clear();
        explored.Clear();

        frontier.Enqueue(grid[coordinates]);
        explored.Add(coordinates, grid[coordinates]);
        // Exit conidition
        bool isRunning = true;

        while (frontier.Count > 0 && isRunning)
        {
            currentSearchNode = frontier.Dequeue();
            currentSearchNode.isExplored = true;
            ExploreNeightbors();
            // Node found
            if (currentSearchNode.coordinates == endCoordinates)
            {
                isRunning = false;
            }
        }
    }

    List<Node> BuildPath()
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        path.Add(endNode);
        currentNode.isPath = true;

        while (currentNode.connectedTo != null)
        {
            currentNode = currentNode.connectedTo;
            path.Add(currentNode);
            currentNode.isPath = true;
        }
        path.Reverse();
        return path;
    }

    public bool WillBlockPath(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            bool previousState = grid[coordinates].isWalkable;
            grid[coordinates].isWalkable = false;
            List<Node> newpath = GetNewPath();
            grid[coordinates].isWalkable = previousState;

            if (newpath.Count <= 1)
            {
                GetNewPath();
                return true;
            }
        }
        return false;
    }

    public void NotifyReceivers()
    {
        BroadcastMessage("RecalculatePath", false, SendMessageOptions.DontRequireReceiver);
    }
}
