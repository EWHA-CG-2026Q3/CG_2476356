
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class S13_SquarePyramidMesh : MonoBehaviour
{
    void OnEnable()
    {
        GenerateMesh();
    }

    void GenerateMesh()
    {
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(0f, 0f, 0f),
            new Vector3(1f, 0f, 0f),
            new Vector3(0f, 0f, 1f),
            new Vector3(1f, 0f, 1f),
            new Vector3(0.5f, 1f, 0.5f)
        };

        int[] triangles = new int[]
        {
            0, 1, 2,
            1, 3, 2,
            0, 4, 1,
            2, 4, 0,
            3, 4, 2,
            1, 4, 3
        };

        Mesh mesh = new Mesh();
        mesh.name = "SquarePyramid";
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        GetComponent<MeshFilter>().sharedMesh = mesh;
    }
}
