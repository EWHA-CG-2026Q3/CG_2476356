
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class S12_HexagonalPrismMesh : MonoBehaviour
{
    void Start()
    {
        Vector3[] vertices = new Vector3[]
        {
            // 앞면 (z = -0.5)
            new Vector3(-0.5f,  1f, -0.5f), // 0
            new Vector3( 0.5f,  1f, -0.5f), // 1
            new Vector3( 1f,    0f, -0.5f), // 2
            new Vector3( 0.5f, -1f, -0.5f), // 3
            new Vector3(-0.5f, -1f, -0.5f), // 4
            new Vector3(-1f,    0f, -0.5f), // 5

            // 뒷면 (z = 0.5)
            new Vector3(-0.5f,  1f, 0.5f), // 6
            new Vector3( 0.5f,  1f, 0.5f), // 7
            new Vector3( 1f,    0f, 0.5f), // 8
            new Vector3( 0.5f, -1f, 0.5f), // 9
            new Vector3(-0.5f, -1f, 0.5f), // 10
            new Vector3(-1f,    0f, 0.5f)  // 11
        };

        int[] triangles = new int[]
        {
            // 앞면
            0, 2, 1,
            0, 3, 2,
            0, 4, 3,
            0, 5, 4,

            // 뒷면
            6, 7, 8,
            6, 8, 9,
            6, 9, 10,
            6, 10, 11,

            // 옆면 6개 (각각 삼각형 2개)
            0, 1, 7,  0, 7, 6,
            1, 2, 8,  1, 8, 7,
            2, 3, 9,  2, 9, 8,
            3, 4, 10, 3, 10, 9,
            4, 5, 11, 4, 11, 10,
            5, 0, 6,  5, 6, 11
        };

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().sharedMaterial =
            new Material(Shader.Find("Universal Render Pipeline/Lit"));
    }
}
