using UnityEditor;
using UnityEngine;

public class MeshGridGenerator : MonoBehaviour
{
    public MeshFilter meshFilter; // Reference to the MeshFilter containing the mesh
    public Vector3 cellSize = new Vector3(0.2f, 0.2f, 0.2f);

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        var mesh = meshFilter.sharedMesh;
        var bounds = mesh.bounds;

        var gridSize = new Vector3Int(
            Mathf.CeilToInt(bounds.size.x / cellSize.x),
            Mathf.CeilToInt(bounds.size.y / cellSize.y),
            Mathf.CeilToInt(bounds.size.z / cellSize.z)
        );

        var gridStart = bounds.min + cellSize * 0.5f;
        var collection = new GameObject("GridCollection")
        {
            transform =
            {
                position = bounds.center
            }
        };

        for (var x = 0; x < gridSize.x; x++)
        {
            for (var y = 0; y < gridSize.y; y++)
            {
                for (var z = 0; z < gridSize.z; z++)
                {
                    var cellCenter = gridStart + new Vector3(x * cellSize.x, y * cellSize.y, z * cellSize.z);

                    // ReSharper disable once InvertIf
                    if (IsPointInsideMesh(cellCenter, mesh))
                    {
                        var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        sphere.transform.position = cellCenter;
                        sphere.transform.localScale = cellSize * 0.9f; // Slightly smaller than cellSize
                        sphere.transform.parent = collection.transform;

                        Destroy(sphere.GetComponent<Collider>());
                    }
                }
            }
        }

        PrefabUtility.SaveAsPrefabAsset(collection, "Assets/Prefabs/collection2.prefab");
        Destroy(collection);
    }

    private static bool IsPointInsideMesh(Vector3 point, Mesh mesh)
    {
        //point = trans.transform.InverseTransformPoint(point);
        var triangles = mesh.triangles;
        var ray = new Ray(point, Vector3.down);
        var intersectCount = 0;
        for (var i = 0; i < triangles.Length; i += 3)
        {
            var v1 = mesh.vertices[triangles[i]];
            var v2 = mesh.vertices[triangles[i + 1]];
            var v3 = mesh.vertices[triangles[i + 2]];

            if (RayTriangleIntersection(ray, v1, v2, v3))
            {
                intersectCount++;
            }
        }

        return intersectCount % 2 == 1;
    }

    private static bool RayTriangleIntersection(Ray ray, Vector3 v1, Vector3 v2, Vector3 v3)
    {
        var e1 = v2 - v1;
        var e2 = v3 - v1;
        var p = Vector3.Cross(ray.direction, e2);
        var det = Vector3.Dot(e1, p);
        if (det > -0.00001f && det < 0.00001f) return false;
        var invDet = 1 / det;
        var t = ray.origin - v1;
        var u = Vector3.Dot(t, p) * invDet;
        if (u < 0 || u > 1) return false;
        var q = Vector3.Cross(t, e1);
        var v = Vector3.Dot(ray.direction, q) * invDet;
        if (v < 0 || u + v > 1) return false;
        var tValue = Vector3.Dot(e2, q) * invDet;
        return !(tValue < 0) && !(tValue > 1000);
    }
}