using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;           // 따라갈 플레이어
    public Vector3 offset = new Vector3(0f, 2f, -5f);
    public float smoothSpeed = 0.125f;

    public float mouseSensitivity = 2.0f;   // 마우스 감도
    public float minPitch = -30f;           // 아래로 얼마나 볼 수 있는지
    public float maxPitch = 60f;            // 위로 얼마나 볼 수 있는지

    private float pitch = 0f;               // 카메라 상하 회전값

    [Header("Collision Settings")]
    public LayerMask groundLayer;           // 'Grounded' 등 충돌을 감지할 레이어
    public float sphereCastRadius = 0.2f;   // 살짝 넓게 검출하기 위한 반지름
    public float collisionOffset = 0.3f;    // 충돌 시, 살짝 밀려날 거리

    private void LateUpdate()
    {
        if (target == null) return;

        // 1) 마우스 입력 받아서 pitch 조정
        float mouseY = Input.GetAxis("Mouse Y");
        pitch -= mouseY * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // 2) 카메라가 위치해야 할 "이상적인 위치" 계산
        //    - pitch(상하),  target.eulerAngles.y(좌우)는 '플레이어의 y회전'을 따라감
        //    - 만약 마우스 X로 카메라도 회전하고 싶다면, 별도 yaw 변수를 만들어 쓰세요!
        Vector3 desiredOffset = Quaternion.Euler(pitch, target.eulerAngles.y, 0f) * offset;
        Vector3 pivotPos = target.position;                // 기준점(플레이어 위치)
        Vector3 desiredPos = pivotPos + desiredOffset;       // 최종적으로 원하는 카메라 위치

        // 3) 충돌(바닥/벽 등) 체크: pivotPos(플레이어) → desiredPos(카메라) 사이에 장애물 있는지 SphereCast
        Vector3 direction = (desiredPos - pivotPos).normalized;
        float distance = desiredOffset.magnitude;

        RaycastHit hit;
        // sphereCastRadius만큼 두께를 주어 얇은 벽도 걸리도록
        if (Physics.SphereCast(pivotPos, sphereCastRadius, direction, out hit, distance, groundLayer))
        {
            // 충돌하면 충돌지점(hit.point)에서 약간( collisionOffset ) 뒤로 뺀 위치에 카메라 배치
            desiredPos = hit.point - direction * collisionOffset;
        }

        // 4) 카메라 이동: 부드럽게 Lerp
       Vector3 smoothedPosition = Vector3.Slerp(transform.position, desiredPos, 1 - Mathf.Exp(-smoothSpeed * Time.deltaTime));
        transform.position = smoothedPosition;


        // 5) 플레이어를 바라보기
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}
