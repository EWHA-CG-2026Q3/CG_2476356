using System.Collections.Generic;
using UnityEngine;

// ① 기하 모델링: 반복문으로 정점 위치를 계산하는 사용자 정의 메시.
// 교과서 기본 정육면체/피라미드 대신 원형 테이블, 속이 빈 손잡이 컵 등을 만든다.
public static class BookCafeMeshFactory
{
    public static Mesh CreateCylinder(string meshName, float radius, float height, int segments)
    {
        segments = Mathf.Max(8, segments);
        var vertices = new List<Vector3>();
        var triangles = new List<int>();
        int bottom = AddRing(vertices, radius, -height * 0.5f, segments);
        int top = AddRing(vertices, radius, height * 0.5f, segments);
        ConnectRings(triangles, bottom, top, segments);

        int bottomCenter = vertices.Count;
        vertices.Add(new Vector3(0, -height * 0.5f, 0));
        int topCenter = vertices.Count;
        vertices.Add(new Vector3(0, height * 0.5f, 0));
        for (int i = 0; i < segments; ++i)
        {
            int next = (i + 1) % segments;
            AddTriangle(triangles, bottomCenter, bottom + i, bottom + next); // 아래 면
            AddTriangle(triangles, topCenter, top + next, top + i);         // 윗 면
        }
        return FinalizeMesh(meshName, vertices, triangles);
    }

    public static Mesh CreateCup(string meshName, int segments = 32)
    {
        segments = Mathf.Max(12, segments);
        var vertices = new List<Vector3>();
        var triangles = new List<int>();

        // 외부 바닥 -> 외부 입구 -> 내부 입구 -> 내부 바닥.
        // 두 개의 다른 반지름을 사용해 위쪽이 넓고 속이 빈 컵을 만든다.
        int outerBottom = AddRing(vertices, 0.137f, 0.000f, segments);
        int outerTop = AddRing(vertices, 0.190f, 0.335f, segments);
        int innerTop = AddRing(vertices, 0.157f, 0.335f, segments);
        int innerBottom = AddRing(vertices, 0.110f, 0.055f, segments);

        ConnectRings(triangles, outerBottom, outerTop, segments); // 바깥 벽 (법선 바깥)
        ConnectRings(triangles, outerTop, innerTop, segments);    // 컵 테두리 (법선 위)
        ConnectRings(triangles, innerTop, innerBottom, segments); // 안쪽 벽 (법선 안)

        int innerCenter = vertices.Count;
        vertices.Add(new Vector3(0, 0.055f, 0));
        int outerCenter = vertices.Count;
        vertices.Add(new Vector3(0, 0, 0));
        for (int i = 0; i < segments; i++)
        {
            int next = (i + 1) % segments;
            AddTriangle(triangles, innerCenter, innerBottom + next, innerBottom + i);
            AddTriangle(triangles, outerCenter, outerBottom + i, outerBottom + next);
        }

        // 컵 손잡이: XY 평면의 타원 호를 따라 단면 원을 반복해서 잇는다.
        int arcSegments = 20;
        int tubeSides = 8;
        int handleStart = vertices.Count;
        for (int a = 0; a <= arcSegments; ++a)
        {
            float t = (float)a / arcSegments;
            float angle = Mathf.Lerp(120f, -120f, t) * Mathf.Deg2Rad;
            Vector3 center = new Vector3(
                0.260f + 0.155f * Mathf.Cos(angle),
                0.175f + 0.155f * Mathf.Sin(angle),
                0f);
            Vector3 tangent = new Vector3(
                -0.155f * Mathf.Sin(angle),
                 0.155f * Mathf.Cos(angle),
                 0f).normalized;
            // 호는 각도가 감소하는 방향으로 이어지지만, 단면 방향만 일관되면 됨.
            Vector3 side = new Vector3(-tangent.y, tangent.x, 0f);
            for (int s = 0; s < tubeSides; ++s)
            {
                float phi = s * Mathf.PI * 2f / tubeSides;
                vertices.Add(center + 0.029f * (Mathf.Cos(phi) * side +
                                                Mathf.Sin(phi) * Vector3.forward));
            }
        }
        for (int a = 0; a < arcSegments; ++a)
        {
            int first = handleStart + a * tubeSides;
            int second = handleStart + (a + 1) * tubeSides;
            for (int s = 0; s < tubeSides; ++s)
            {
                int next = (s + 1) % tubeSides;
                AddTriangle(triangles, first + s, second + s, second + next);
                AddTriangle(triangles, first + s, second + next, first + next);
            }
        }
        return FinalizeMesh(meshName, vertices, triangles);
    }

    public static Mesh CreateLampShade(string meshName, int segments = 32)
    {
        segments = Mathf.Max(8, segments);
        var vertices = new List<Vector3>();
        var triangles = new List<int>();
        int top = AddRing(vertices, 0.16f, 0.14f, segments);
        int bottom = AddRing(vertices, 0.43f, -0.14f, segments);
        ConnectRings(triangles, bottom, top, segments);
        // 앞/뒷면 정점을 따로 만들어 면의 법선이 서로 상쇄되지 않게 한다.
        int innerTop = AddRing(vertices, 0.16f, 0.14f, segments);
        int innerBottom = AddRing(vertices, 0.43f, -0.14f, segments);
        ConnectRings(triangles, innerTop, innerBottom, segments);
        return FinalizeMesh(meshName, vertices, triangles);
    }

    public static Mesh CreateCurtain(string meshName, float width, float height,
                                    int horizontalSteps = 32, int verticalSteps = 16)
    {
        var vertices = new List<Vector3>();
        var triangles = new List<int>();
        for (int y = 0; y <= verticalSteps; ++y)
        {
            float v = (float)y / verticalSteps;
            for (int x = 0; x <= horizontalSteps; ++x)
            {
                float u = (float)x / horizontalSteps;
                float fold = Mathf.Sin(u * Mathf.PI * 8f) * (0.07f + 0.05f * v);
                float drape = 0.035f * Mathf.Sin(v * Mathf.PI) * Mathf.Sin(u * Mathf.PI * 2f);
                vertices.Add(new Vector3((u - 0.5f) * width + drape,
                                         -v * height, fold));
            }
        }
        int stride = horizontalSteps + 1;
        for (int y = 0; y < verticalSteps; ++y)
        for (int x = 0; x < horizontalSteps; ++x)
        {
            int a = y * stride + x;
            int b = a + 1;
            int c = a + stride;
            int d = c + 1;
            AddTriangle(triangles, a, c, d);
            AddTriangle(triangles, a, d, b);
        }
        // 앞/뒷면이 모두 보이도록 별도의 뒷면 정점 생성
        int frontCount = vertices.Count;
        for (int i = 0; i < frontCount; ++i) vertices.Add(vertices[i]);
        int frontTriangleCount = triangles.Count;
        for (int i = 0; i < frontTriangleCount; i += 3)
        {
            AddTriangle(triangles,
                frontCount + triangles[i],
                frontCount + triangles[i + 2],
                frontCount + triangles[i + 1]);
        }
        return FinalizeMesh(meshName, vertices, triangles);
    }

    private static int AddRing(List<Vector3> vertices, float r, float y, int segments)
    {
        int start = vertices.Count;
        for (int i = 0; i < segments; ++i)
        {
            // 여기서 반복문을 사용하여 각 정점의 위치를 직접 계산한다 (필수 조건).
            float angle = i * Mathf.PI * 2f / segments;
            vertices.Add(new Vector3(r * Mathf.Cos(angle), y, r * Mathf.Sin(angle)));
        }
        return start;
    }

    private static void ConnectRings(List<int> triangles, int lower,
                                     int upper, int segments)
    {
        for (int i = 0; i < segments; ++i)
        {
            int next = (i + 1) % segments;
            AddTriangle(triangles, lower + i, upper + i, upper + next);
            AddTriangle(triangles, lower + i, upper + next, lower + next);
        }
    }

    private static void AddTriangle(List<int> triangles, int a, int b, int c)
    {
        triangles.Add(a);
        triangles.Add(b);
        triangles.Add(c);
    }

    private static Mesh FinalizeMesh(string meshName,
                                     List<Vector3> vertices, List<int> triangles)
    {
        Mesh mesh = new Mesh { name = meshName };
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        return mesh;
    }
}
