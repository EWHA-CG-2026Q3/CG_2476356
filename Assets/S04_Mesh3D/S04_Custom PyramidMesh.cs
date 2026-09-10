using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class S04_PyramidMesh : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(0f, 0f, 0f), //0 0번점
            new Vector3(1f, 0f, 0f), //1 1번점
            new Vector3(0f, 0f, 1f), //2 4번점
            new Vector3(1f, 0f, 1f), //3 5번점
            new Vector3(0.5f, 1f, 0.5f) //4 윗면 가운데점
        };

        int[] triangles = new int[]
        {
            // 아랫면 y=0면 반1
            0, 1, 2,
            1, 3, 2,
            // 옆면1
            0, 4, 1,
            // 옆면2
            2, 4, 0,
            // 옆면3
            3, 4, 2,
            // 옆면4
            1, 4, 3
        };

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
    }
}
