using UnityEngine;

// S08-S12에서 다룬 Matrix4x4 변환: 이동(T) × 회전(R).
// 조명 전체를 묶은 부모 오브젝트에 붙여 Play 모드에서 확인합니다.
public class ProjectLampMotion : MonoBehaviour
{
    public float swingAngle = 10f;
    public float swingSpeed = 0.65f;
    public float verticalMotion = 0.06f;

    Vector3 startPosition;
    Quaternion startRotation;

    void Start()
    {
        startPosition = transform.localPosition;
        startRotation = transform.localRotation;
    }

    void Update()
    {
        float t = Time.time * swingSpeed;
        float angleX = Mathf.Sin(t) * swingAngle;
        float angleZ = Mathf.Cos(t * 0.8f) * swingAngle * 0.45f;
        float heightOffset = Mathf.Sin(t * 1.6f) * verticalMotion;

        Matrix4x4 T = Matrix4x4.Translate(
            startPosition + new Vector3(0f, heightOffset, 0f));
        Matrix4x4 R = Matrix4x4.Rotate(
            startRotation * Quaternion.Euler(angleX, 0f, angleZ));

        // 직접 만든 변환 행렬. 이동과 회전을 결합합니다.
        Matrix4x4 M = T * R;

        Vector4 p = M.GetColumn(3);
        transform.localPosition = new Vector3(p.x, p.y, p.z);
        transform.localRotation = M.rotation;
    }
}
