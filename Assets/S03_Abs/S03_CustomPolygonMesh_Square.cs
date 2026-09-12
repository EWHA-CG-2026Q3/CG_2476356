using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class NewMonoBehaviourScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // TODO 1: 원하는 다각형의 정점 좌표를 채우세요 (최소 4개)
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(0f, 1f, 0f),    //0번째 점. xy평면에서 봤을 때 좌상
            new Vector3(1f, 1f, 0f),    //1번째 점. 우상
            new Vector3(1f, 0f, 0f),    //2번째 점, 좌하단
            new Vector3(0f, 0f, 0f)     //3번째 점. 원점, 우하단
        };

        //TODO 2: 정점 3개씩 묶어 삼각형들을 구성하세요
        int[] triangles = new int[]
        {
            0, 1, 2,    //오른쪽위 삼각형, 시계순
            0, 2, 3     //왼쪽 아래 삼각형, 시계순
        };

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;

        GetComponent<MeshRenderer>().sharedMaterial = 
        new Material(Shader.Find("Universal Render Pipeline/Lit"));
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
