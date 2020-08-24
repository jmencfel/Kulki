using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    Grid grid;
    public Transform seeker, target;
    public static Pathfinding instance;

    private void Awake()
    {
        instance = this;
        grid = GetComponent<Grid>();
    }
    private void Update()
    {

    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (distX > distY)
            return 2 * distY + (distX-distY);
        return 2 * distX + (distY-distX);
    }
    public void RetracePath(Node StartNode, Node EndNode)
    {
        Debug.Log("Retracing path");
        List<Node> path = new List<Node>();
        Node currentNode = EndNode;

        while(currentNode!=StartNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        Debug.Log("Path has " + path.Count + " steps");
         GameController.instance.path = path;
        grid.path = path;
    }
   
    public bool FindPath(Vector2 startPos, Vector2 targetPos)
    {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        Debug.Log("Start :" + startNode.gridX + "/" + startNode.gridY + " Finish: " + targetNode.gridX + "/" + targetNode.gridY);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while(openSet.Count>0)
        {
            Node currentNode = openSet[0];
            
            for (int i=1;i<openSet.Count;i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost)
                {
                    if (openSet[i].hCost < currentNode.hCost)
                        currentNode = openSet[i];
                }
            }
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);
            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return true;
            }
            Debug.Log("Current node"+ currentNode.gridX +"/"+ currentNode.gridY + " has " + grid.GetNeighbours(currentNode).Count + " neighbours");
            foreach(Node neighbour in grid.GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                    continue;

                int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbour);
                if(newMovementCostToNeighbor < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbor;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }

        }
        return false;
    }
}
