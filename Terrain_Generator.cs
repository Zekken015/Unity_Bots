using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Terrain_Generator : MonoBehaviour
{
    public int Width = 20;
    public int Depth = 20;
    public GameObject VertexPrefab;
    public bool VisualizeVertices;
    public Gradient gradient;
    public NavMeshSurface surface;
    Vector3[] vertices;
    int[] triangles;
    Color[] colors;
    Vector2[] uvs;
    MeshFilter meshFilter;
    Mesh mesh;
    float minHeight;
    float maxHeight;


    // Start is called before the first frame update
    void Start()
    {
        InitializeMesh();
        CreateMesh();
        UpdateMesh();

        if(VisualizeVertices)
            DrawVertices();
        GetComponent<MeshCollider>().sharedMesh = mesh;
        surface.BuildNavMesh();

    }


    private void DrawVertices()
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            Instantiate(VertexPrefab, vertices[i], Quaternion.identity, transform);
        }
    }

    private void InitializeMesh()
    {
        meshFilter = GetComponent<MeshFilter>();

        mesh = new Mesh();
        mesh.name = "Procedural Terrain";

        meshFilter.mesh = mesh;
    }

    private void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
    }

    private void CreateMesh()
    {
        CreateVertices();
        CreateTriangles();
        CreateColours();
        CreateUVs();
    }

    private void CreateUVs()
    {
        uvs = new Vector2[vertices.Length];
        var vertexIndex = 0;
        for (int z = 0; z <= Depth; z++)
        {
            for (int x = 0; x <= Width; x++)
            {
                uvs[vertexIndex] = new Vector2((float)x / Width, (float)z / Depth);
                vertexIndex ++;
            }
        }
    }

    private void CreateColours()
    {
        colors = new Color[vertices.Length];
        var currentVertexIndex = 0;
        for (int z = 0; z <= Depth; z++)
        {
            for (int x = 0; x <= Width; x++)
            {
                var height = Mathf.InverseLerp(minHeight, maxHeight, vertices[currentVertexIndex].y);
                colors[currentVertexIndex] = gradient.Evaluate(height);
                currentVertexIndex ++;
            }
        }
    }

    private void CreateTriangles()
    {
        triangles = new int[Width * Depth * 6];
        int currentVertexPoint = 0;
        int currentTrianglePoint = 0;

        for (int z = 0; z < Depth; z++)
        {

            for (int x = 0; x < Width; x++)
            {
                triangles[currentTrianglePoint + 0] = currentVertexPoint + 0;
                triangles[currentTrianglePoint + 1] = currentVertexPoint + Width + 1;
                triangles[currentTrianglePoint + 2] = currentVertexPoint + 1;
                triangles[currentTrianglePoint + 3] = currentVertexPoint + 1;
                triangles[currentTrianglePoint + 4] = currentVertexPoint + Width + 1;
                triangles[currentTrianglePoint + 5] = currentVertexPoint + Width + 2;

                currentVertexPoint ++;
                currentTrianglePoint += 6;
            }
            currentVertexPoint ++;
        }
    }

    private void CreateVertices()
    {
        vertices = new Vector3[(Width + 1) * (Depth + 1)];
        int vertexIndex = 0;
        for(int z = 0; z <= Depth; z++)
        {
            for(int x = 0; x <= Width; x++)
            {
                float y = Mathf.PerlinNoise(x * 0.1f, z * 0.1f) * 3f;
                vertices[vertexIndex] = new Vector3(x, y, z);

                if (y > maxHeight)
                {
                    maxHeight = y;
                }

                if (y < minHeight)
                {
                    minHeight = y;
                }

                vertexIndex++;
            }
        }
    }
}
