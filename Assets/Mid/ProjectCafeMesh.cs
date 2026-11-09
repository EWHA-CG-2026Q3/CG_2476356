using UnityEngine;

// S13에서 사용한 패턴과 동일하게 OnEnable에서 메시를 생성합니다.
// Inspector에서 Shape을 Table 또는 Cup으로 선택하세요.
[ExecuteAlways]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProjectCafeMesh : MonoBehaviour
{
    public enum CafeShape
    {
        TableTop,
        CoffeeCup
    }

    public CafeShape shape = CafeShape.TableTop;
    [Range(12, 64)] public int tableSegments = 40;
    public float tableRadius = 0.82f;
    public float tableThickness = 0.13f;

    void OnEnable()
    {
        GenerateMesh();
    }

    void GenerateMesh()
    {
        Mesh mesh;

        if (shape == CafeShape.TableTop)
        {
            // 반복문을 사용해 정점 위치를 계산하는 사용자 정의 원형 테이블.
            mesh = BookCafeMeshFactory.CreateCylinder(
                "Project_RoundTableTop", tableRadius, tableThickness, tableSegments);
        }
        else
        {
            // 안쪽·바깥쪽 링과 손잡이를 각각 반복문으로 만드는 사용자 정의 컵.
            mesh = BookCafeMeshFactory.CreateCup("Project_HollowCoffeeCup");
        }

        GetComponent<MeshFilter>().sharedMesh = mesh;
    }
}
