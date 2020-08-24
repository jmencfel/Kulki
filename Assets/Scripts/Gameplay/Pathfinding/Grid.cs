using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    [SerializeField]
    Node[,] grid;
    public float nodeRadius;
    float nodeDiameter;
    public int GridSizeX = 9;
    public int GridSizeY = 9;
    public Transform rando;
    
    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        CreateGrid();
    }
    void CreateGrid()
    {
        int PosX = 0;
        int PosY = 0;
        grid = new Node[GridSizeX, GridSizeY];
        for(int i=0; i<GridSizeX; i++)
        {
            PosY = 0;
            for (int j = 0; j < GridSizeY; j++)
            {
                Vector3 position = new Vector3(PosX, PosY, 0);
                RaycastHit2D hit = Physics2D.Raycast(position, Vector2.left, 0.1f);               
                bool walkable = (hit.collider == null);
                grid[i, j] = new Node(walkable, position, i, j);
                PosY--;
               
            }
            PosX++;
        }

    }
    public void Update()
    {
       
    }
    public void UpdateWalkableInfo()
    {
        for (int i = 0; i < GridSizeX; i++)
        {
            for (int j = 0; j < GridSizeY; j++)
            {
                Vector3 position = new Vector3(grid[i, j].worldPosition.x, grid[i, j].worldPosition.y, 0);
                RaycastHit2D hit = Physics2D.Raycast(position, Vector2.left, 0.1f);
                bool walkable = (hit.collider == null);
                grid[i, j].walkable = walkable;

            }
        }
    }
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

       
        if (node.gridX-1 >= 0 && node.gridX - 1 < GridSizeX )
             neighbours.Add(grid[node.gridX-1, node.gridY]);
        if (node.gridX + 1 >= 0 && node.gridX + 1 < GridSizeX)
            neighbours.Add(grid[node.gridX + 1, node.gridY]);
        if (node.gridY - 1 >= 0 && node.gridY - 1 < GridSizeY)
            neighbours.Add(grid[node.gridX, node.gridY-1]);
        if (node.gridY + 1 >= 0 && node.gridY + 1 < GridSizeY)
            neighbours.Add(grid[node.gridX, node.gridY+1]);



        return neighbours;
    }
    public Node NodeFromWorldPoint(Vector2 position)
    {   
        return grid[(int)position.x, Mathf.Abs((int)position.y)];
    }

    public List<Node> path = new List<Node>();


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));
        if(grid!=null)
        {
            foreach(Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                if(path!=null)
                {
                    if(path.Contains(n))
                    {
                        Gizmos.color = Color.cyan;
                    }
                }
                Gizmos.DrawCube(n.worldPosition,Vector3.one*0.2f);
            }

            
        }
    }
}
