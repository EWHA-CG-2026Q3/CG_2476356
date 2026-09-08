using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class S03_CustomPolygonMesh_Hibe : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(-0.5f,  1f, 0f),
            new Vector3( 0.5f,  1f, 0f),
            new Vector3( 1f,    0f, 0f),
            new Vector3( 0.5f, -1f, 0f),
            new Vector3(-0.5f, -1f, 0f),
            new Vector3(-1f,    0f, 0f)
        };

        int[] triangles = new int[]
        {
            0, 1, 2,
            0, 2, 3,
            0, 3, 4,
            0, 4, 5
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
