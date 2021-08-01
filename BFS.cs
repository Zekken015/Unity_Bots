using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFS : MonoBehaviour
{
    public GridGenerator GridData;

    Queue<Vector3> queue;
    HashSet<Vector3> visited;

    Dictionary<Vector3, Vector3> cellParents;

    private void Start()
    {
        queue = new Queue<Vector3>();
        visited = new HashSet<Vector3>();
        cellParents = new Dictionary<Vector3, Vector3>();
    }

    //private void Update()
    //{
    //    Search();
    //}

    public void Search()
    {
        ClearData();
        //Debug.Log("Test0");
        queue.Enqueue(GridData.StartPosition);
        visited.Add(GridData.StartPosition);

        while (queue.Count > 0)
        {
            //Debug.Log("Test1");
            var currentCell = queue.Dequeue();
            //Debug.Log("Current:" + currentCell + " End:" + GridData.EndPosition);
            if (currentCell == GridData.EndPosition)
            {
                Debug.Log("Destination Reached!");
                GridData.VisualizePath(cellParents);
                return;
            }

            //Debug.Log("Test2");
            var neighbours = GridData.GetNeighbours(currentCell);
            
            foreach (var neighbour in neighbours)
            {
                //Debug.Log("Test3");
                if (!visited.Contains(neighbour))
                {
                    //Debug.Log("Test4");
                    queue.Enqueue(neighbour);
                    visited.Add(neighbour);
                    cellParents[neighbour] = currentCell;
                }
            }
            //GridData.ClearPath();
        }
    }

    private void ClearData()
    {
        queue.Clear();
        visited.Clear();
        cellParents.Clear();
    }
}
