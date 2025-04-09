using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;           // 따라갈 플레이어
    public Vector3 offset = new Vector3(0f, 2f, -5f);
    public float smoothSpeed = 0.125f;

    public float mouseSensitivity = 2.0f;   // 마우스 감도
    public float minPitch = -30f;           // 아래로 얼마나 볼 수 있는지
    public float maxPitch = 60f;            // 위로 얼마나 볼 수 있는지

    private float pitch = 0f;               // 상하 회전값
    private float yaw = 0f;                 // 좌우 회전값 (추가)

    [Header("Collision Settings")]
    public LayerMask groundLayer;           // 충돌을 감지할 레이어
    public float sphereCastRadius = 0.2f;   // 충돌 감지 반지름
    public float collisionOffset = 0.3f;    // 충돌 시, 살짝 떨어질 거리

    private void Update()
    {
        // 입력은 Update에서 처리
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        yaw += mouseX * mouseSensitivity;    // 좌우 회전 (추가)
        pitch -= mouseY * mouseSensitivity;  // 상하 회전
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
    }

    private void FixedUpdate()
    {
        // 카메라 목표 위치 계산
        Vector3 desiredOffset = Quaternion.Euler(pitch, yaw, 0f) * offset;
        Vector3 pivotPos = target.position;
        Vector3 desiredPos = pivotPos + desiredOffset;

        // 충돌 처리
        Vector3 direction = (desiredPos - pivotPos).normalized;
        float distance = desiredOffset.magnitude;

        RaycastHit hit;
        if (Physics.SphereCast(pivotPos, sphereCastRadius, direction, out hit, distance, groundLayer))
        {
            desiredPos = hit.point - direction * collisionOffset;
        }

        // 부드럽게 이동
        Vector3 smoothedPosition = Vector3.Slerp(transform.position, desiredPos, 1 - Mathf.Exp(-smoothSpeed * Time.deltaTime));
        transform.position = smoothedPosition;

        // 타겟 바라보기 (살짝 위를 바라보게)
        transform.LookAt(pivotPos + Vector3.up * 1.5f);
    }
}
