using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//3840 sides

public class GenerateWorld : MonoBehaviour
{
    public Material _earthMaterial;
    public Material _hazeMaterial;
    public int targetTileDepth = 1;
    private float tileThickness = 0.003f;
    private int _minEarthHeight, _maxEarthHeight;

    public Material GetMaterial(TileType tileType)
    {
        switch (tileType)
        {
            case TileType.EARTH:
                return _earthMaterial;
            case TileType.HAZE:
                return _hazeMaterial;
            default:
                return null;
        }
    }

    void Start()
    {
        Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;
        int[] triangles = mesh.triangles;
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;
        Vector2[] uvs = mesh.uv;
        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 vertex1 = vertices[triangles[i]];
            Vector3 vertex2 = vertices[triangles[i + 1]];
            Vector3 vertex3 = vertices[triangles[i + 2]];
            Vector3[] triVertices = new Vector3[] { vertex1, vertex2, vertex3 };

            TileStack s = new TileStack(triVertices);

            foreach (TileType t in Enum.GetValues(typeof(TileType)))
            {
                GameObject newTile = new GameObject();
                newTile.tag = "TileObject";
                newTile.name = t.ToString() + i;
                newTile.AddComponent<MeshFilter>();
                newTile.AddComponent<MeshRenderer>();
                newTile.transform.SetParent(gameObject.transform, false);
                newTile.isStatic = true;
                int targetHeight = 1;
                if (t == TileType.EARTH)
                {
                    GenerateTileMesh(0, targetTileDepth, triVertices, newTile, t);
                    newTile.AddComponent<MeshCollider>();
                    s.AddTile(newTile, this, t, targetHeight);
                    newTile.GetComponent<MeshCollider>().convex = true;
                }
                else if (t == TileType.HAZE)
                {
                    GenerateTileMesh(targetTileDepth, 40, triVertices, newTile, t);
                    newTile.GetComponent<MeshRenderer>().material = _hazeMaterial;
                    targetHeight = 40;
                }
                // else
                // {
                // GenerateTileMesh(targetTileDepth, targetTileDepth, triVertices, newTile, t);
                // newTile.SetActive(false);
                // }

                // s.AddTile(newTile, this, t, targetHeight);
            }
            TerrainController.AddStack(s);
        }
        TerrainController.AssignNeighbors();
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    public void GenerateTileMesh(int tileOrigin, int tileHeight, Vector3[] vertices, GameObject tileObject, TileType type)
    {
        Mesh tileMesh = tileObject.GetComponent<MeshFilter>().mesh;
        tileMesh.Clear();
        float lowerTileHeight = 1 + tileThickness * tileOrigin;
        float upperTileHeight = 1 + tileThickness * tileHeight;
        Vector3 lowerVert1 = vertices[0] * lowerTileHeight;
        Vector3 lowerVert2 = vertices[1] * lowerTileHeight;
        Vector3 lowerVert3 = vertices[2] * lowerTileHeight;
        Vector3 upperVert1 = vertices[0] * upperTileHeight;
        Vector3 upperVert2 = vertices[1] * upperTileHeight;
        Vector3 upperVert3 = vertices[2] * upperTileHeight;

        if (type == TileType.HAZE)
        {
            tileMesh.vertices = new Vector3[] { upperVert1, upperVert2, upperVert3 };
            tileMesh.triangles = new int[] { 0, 1, 2 };
        }
        else
        {
            tileMesh.vertices = new Vector3[] { lowerVert1, upperVert1, upperVert2 , lowerVert1, upperVert2, lowerVert2 ,
                                                lowerVert2, upperVert2, upperVert3, lowerVert2, upperVert3, lowerVert3 ,
                                                lowerVert3, upperVert3, upperVert1, lowerVert3, upperVert1, lowerVert1 ,
                                                upperVert1, upperVert3, upperVert2, lowerVert1, lowerVert3, lowerVert2 };
            tileMesh.triangles = new int[] { 0, 2, 1, 3, 5, 4,
                                            6, 8, 7, 9, 11, 10,
                                            12, 14, 13, 15, 17, 16,
                                            18, 20, 19, 21, 22, 23 };
        }
        tileMesh.RecalculateNormals();
        SetUVs(tileMesh);
    }

    private void SetUVs(Mesh mesh)
    {
        Vector2[] uvs = new Vector2[mesh.vertices.Length];
        for (int i = 0; i < uvs.Length;)
        {
            if (mesh.vertices[i].x == mesh.vertices[i + 1].x && mesh.vertices[i].x == mesh.vertices[i + 2].x)
            {
                uvs[i] = new Vector2(mesh.vertices[i].y, mesh.vertices[i].z);
                uvs[i + 1] = new Vector2(mesh.vertices[i + 1].y, mesh.vertices[i + 1].z);
                uvs[i + 2] = new Vector2(mesh.vertices[i + 2].y, mesh.vertices[i + 2].z);
            }
            else
                if (mesh.vertices[i].y == mesh.vertices[i + 1].y && mesh.vertices[i].y == mesh.vertices[i + 2].y)
            {
                uvs[i] = new Vector2(mesh.vertices[i].x, mesh.vertices[i].z);
                uvs[i + 1] = new Vector2(mesh.vertices[i + 1].x, mesh.vertices[i + 1].z);
                uvs[i + 2] = new Vector2(mesh.vertices[i + 2].x, mesh.vertices[i + 2].z);
            }
            else
                    if (mesh.vertices[i].z == mesh.vertices[i + 1].z && mesh.vertices[i].z == mesh.vertices[i + 2].z)
            {
                uvs[i] = new Vector2(mesh.vertices[i].x, mesh.vertices[i].y);
                uvs[i + 1] = new Vector2(mesh.vertices[i + 1].x, mesh.vertices[i + 1].y);
                uvs[i + 2] = new Vector2(mesh.vertices[i + 2].x, mesh.vertices[i + 2].y);
            }
            else
            {
                uvs[i] = new Vector2(mesh.vertices[i].x, mesh.vertices[i].y);
                uvs[i + 1] = new Vector2(mesh.vertices[i + 1].x, mesh.vertices[i + 1].y);
                uvs[i + 2] = new Vector2(mesh.vertices[i + 2].x, mesh.vertices[i + 2].y);
            }

            i += 3;
        }
        mesh.uv = uvs;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
