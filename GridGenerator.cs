using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public int Width = 20;
    public int Depth = 20;
    public int NumberOfObstacles = 10;
    public GameObject Player;
    public GameObject Player2;
    public GameObject Destination;
    public GameObject ObstaclePrefab;
    public GameObject ObstaclePrefab2;
    public GameObject Ground;
    public GameObject Ground2;
    Vector3 test2 = new Vector3(0, 0, 0);
    Vector3 test1 = new Vector3(0, 0.5f, 0);
    bool moveCube = false;

    [Header("Visualize Path")]
    public GameObject PathPrefab;
    public Transform PathCellsHolder;

    [HideInInspector]
    public Vector3 StartPosition;
    [HideInInspector]
    public Vector3 EndPosition;

    public HashSet<Vector3> Obstacles;
    public HashSet<Vector3> WalkableCells;

    private void Start()
    {
        Obstacles = new HashSet<Vector3>();
        WalkableCells = new HashSet<Vector3>();
        GenerateGrid();
    }

    private void Update()
    {
        
    }

    public void GenerateGrid()
    {
        ClearData();
        ClearPath();

        Ground.transform.position = new Vector3(Width / 2f, 0, Depth / 2f);
        //Ground.transform.localScale = new Vector3(Width / 10f, 1, Depth / 10f);
        Ground2.transform.position = new Vector3(0, 0, 0);

        PlaceObstacles();
        PlaceObstacles2();
        StartPosition = PlaceObject(Player);
        PlaceObject(Player2);
        EndPosition = PlaceObject(Destination);

        LocateWalkableCells();
        //Ground.transform.position = test2;
        //ObstaclePrefab.transform.position = test1;
    }

    private void LocateWalkableCells()
    {
        for (int z = 0; z < Depth; z++)
        {
            for (int x = 0; x < Width; x++)
            {
                var currentCell = new Vector3(x, 0, z);
                if (!IsCellOccupied(currentCell))
                {
                    WalkableCells.Add(currentCell);
                }
            }
        }
    }

    private Vector3 PlaceObject(GameObject gameObjectToPlace)
    {
        while (true)
        {
            var positionX = UnityEngine.Random.Range(1, Width);
            var positionZ = UnityEngine.Random.Range(1, Depth);

            var cellPosition = new Vector3(positionX, ObstaclePrefab.transform.position.y, positionZ);

            if (!IsCellOccupied(cellPosition))
            {
                //Obstacles.Add(cellPosition);
                var objectPosition = cellPosition;
                objectPosition.y = gameObjectToPlace.transform.position.y;

                Instantiate(gameObjectToPlace, cellPosition, Quaternion.identity, transform);
                return cellPosition;
            }
        }
    }

    private void PlaceObstacles()
    {
        var obstaclesToGenerate = NumberOfObstacles;
        while (obstaclesToGenerate > 0)
        {
            var positionX = UnityEngine.Random.Range(1, Width);
            var positionZ = UnityEngine.Random.Range(1, Depth);

            var cellPosition = new Vector3(positionX, ObstaclePrefab.transform.position.y, positionZ);

            if (!IsCellOccupied(cellPosition))
            {
                Obstacles.Add(cellPosition);
                var objectPosition = cellPosition;
                objectPosition.y = ObstaclePrefab.transform.position.y;

                Instantiate(ObstaclePrefab, cellPosition, Quaternion.identity, transform);
                obstaclesToGenerate--;
            }

        }
    }
    private void PlaceObstacles2()
    {
        var obstaclesToGenerate = NumberOfObstacles;
        while (obstaclesToGenerate > 0)
        {
            var positionX = UnityEngine.Random.Range(1, Width);
            var positionZ = UnityEngine.Random.Range(1, Depth);

            var cellPosition = new Vector3(positionX, ObstaclePrefab2.transform.position.y, positionZ);

            if (!IsCellOccupied(cellPosition))
            {
                Obstacles.Add(cellPosition);
                var objectPosition = cellPosition;
                objectPosition.y = ObstaclePrefab2.transform.position.y;

                Instantiate(ObstaclePrefab2, cellPosition, Quaternion.identity, transform);
                obstaclesToGenerate--;
            }

        }
    }

    public List<Vector3> GetNeighbours(Vector3 currentCell)
    {
        var neighbours = new List<Vector3>()
        {
            new Vector3(currentCell.x, 0.5f, currentCell.z + 1), //up
            new Vector3(currentCell.x, 0.5f, currentCell.z - 1), //down
            new Vector3(currentCell.x - 1, 0.5f, currentCell.z), //left
            new Vector3(currentCell.x + 1, 0.5f, currentCell.z), //right
        };

        var walkableNeighbours = new List<Vector3>();
        foreach (var neighbour in neighbours)
        {
            if (!IsCellOccupied(neighbour) && IsInLevelBounds(neighbour))
            {
                walkableNeighbours.Add(neighbour);
            }
        }
        return walkableNeighbours;
    }

    private bool IsInLevelBounds(Vector3 cell)
    {
        if (cell.x > 0 && cell.x < Width && cell.z > 0 && cell.z < Depth)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void VisualizePath(Dictionary<Vector3, Vector3> cellParents)
    {
        var path = new List<Vector3>();
        var current = cellParents[EndPosition];

        while (current != StartPosition)
        {
            path.Add(current);
            current = cellParents[current];
        }

        for (int i = 0; i < path.Count; i++)
        {
            var pathCellPosition = path[i];
            pathCellPosition.y = PathPrefab.transform.position.y;
            Instantiate(PathPrefab, pathCellPosition, Quaternion.identity, PathCellsHolder);
        }

    }

    private bool IsCellOccupied(Vector3 cellPosition)
    {
        if (Obstacles.Contains(cellPosition))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ClearData()
    {
        DeleteChildren(transform);
        Obstacles.Clear();
        WalkableCells.Clear();
    }
    public void ClearPath()
    {
        DeleteChildren(PathCellsHolder);
    }

    private void DeleteChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }

    public void MoveCube()
    {
        moveCube = true;
    }
}
