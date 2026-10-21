using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class S13_StarCrystalMesh : MonoBehaviour
{
    [Header("Shape")]
    public float topLength = 0.8f;
    public float sideLength = 0.8f;
    public float bottomLength = 1.0f;

    public float innerX = 0.22f;
    public float innerY = 0.22f;

    [Header("Depth")]
    public float depth = 0.18f;

    void OnEnable()
    {
        GenerateMesh();
    }

    void OnValidate()
    {
        GenerateMesh();
    }

    void GenerateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.name = "StarCrystal";

        Vector3[] vertices = new Vector3[]
        {
            // 바깥 윤곽 8점 (z=0 평면)
            new Vector3(0f,  topLength, 0f),      // 0 위
            new Vector3(innerX,  innerY, 0f),     // 1 우상 안쪽
            new Vector3(sideLength, 0f, 0f),      // 2 오른쪽
            new Vector3(innerX, -innerY, 0f),     // 3 우하 안쪽
            new Vector3(0f, -bottomLength, 0f),   // 4 아래
            new Vector3(-innerX, -innerY, 0f),    // 5 좌하 안쪽
            new Vector3(-sideLength, 0f, 0f),     // 6 왼쪽
            new Vector3(-innerX, innerY, 0f),     // 7 좌상 안쪽

            // 앞/뒤 중심점
            new Vector3(0f, 0f,  depth),          // 8 앞 중심
            new Vector3(0f, 0f, -depth)           // 9 뒤 중심
        };

        int[] triangles = new int[]
        {
            // 앞면 (중심 8번)
            8, 0, 1,
            8, 1, 2,
            8, 2, 3,
            8, 3, 4,
            8, 4, 5,
            8, 5, 6,
            8, 6, 7,
            8, 7, 0,

            // 뒷면 (중심 9번)
            9, 1, 0,
            9, 2, 1,
            9, 3, 2,
            9, 4, 3,
            9, 5, 4,
            9, 6, 5,
            9, 7, 6,
            9, 0, 7
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        GetComponent<MeshFilter>().sharedMesh = mesh;
    }
}