using Priority_Queue;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Dijkstra : MonoBehaviour
{
    public GridGenerator GridData;

    SimplePriorityQueue<Vector3, int> priorityQueue;
    HashSet<Vector3> visited;

    Dictionary<Vector3, Vector3> cellParents;

    private void Start()
    {
        priorityQueue = new SimplePriorityQueue<Vector3, int>();
        visited = new HashSet<Vector3>();
        cellParents = new Dictionary<Vector3, Vector3>();
    }

    public void Search()
    {
        var distances = GridData.WalkableCells.ToDictionary(x => x, x => int.MaxValue); // added "using System.Linq;"
        distances[GridData.StartPosition] = 0;
        ClearData();

        priorityQueue.Enqueue(GridData.StartPosition, 0);
        visited.Add(GridData.StartPosition);

        while (priorityQueue.Count > 0)
        {
            var currentCell = priorityQueue.Dequeue();
            Debug.Log("Current:" + currentCell + " End:" + GridData.EndPosition);
            if (currentCell == GridData.EndPosition)
            {
                Debug.Log("Destination Reached!");
                GridData.VisualizePath(cellParents);
                return;
            }

            var neighbours = GridData.GetNeighbours(currentCell);

            foreach (var neighbour in neighbours)
            {
                Debug.Log("Test 1");
                if (!visited.Contains(neighbour))
                {
                    Debug.Log("Test 2");
                    var distance = distances[currentCell] + 1;
                    Debug.Log("Test 3");
                    if (distance < distances[neighbour])
                    {
                        Debug.Log("Test 4");
                        distances[neighbour] = distance;

                        priorityQueue.Enqueue(neighbour, distance);
                        visited.Add(neighbour);
                        cellParents[neighbour] = currentCell;
                    }
                }
            }
        }
    }

    private void ClearData()
    {
        priorityQueue.Clear();
        visited.Clear();
        cellParents.Clear();
    }
}
