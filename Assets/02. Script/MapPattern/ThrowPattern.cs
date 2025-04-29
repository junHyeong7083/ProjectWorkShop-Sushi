using System.Collections;
using UnityEngine;

public class ThrowPattern : MonoBehaviour
{
    [Header("회전속도 값이 오를수록 빠르게 회전합니다")]
    public float rotateSpeed = 1440f;

    [Header("회전할 타이밍")]
    public float rotateDuration = 0.3f;

    [Header("회전 후 멈춰있을 시간")]
    public float waitTime = 5f;

    [Header("원상 복귀 시간")]
    public float backTime = 2f;

    [Header("던질 때 이동 시간")]
    public float throwDuration = 2f;

    [Header("모델 콜라이더")]
    [SerializeField] private BoxCollider fbxCollider;

    [Header("던질 콜라이더")]
    [SerializeField] private BoxCollider throwCollider;

    [Header("태초마을 위치")]
    [SerializeField] private Transform palletTownTransform;

    [Header("던질 곡선 제어 포인트")]
    [SerializeField] private Transform pointTransform;

    private Quaternion originalRotation;
    private bool isThrow = false;

    private void Start()
    {
        originalRotation = transform.rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!IsValidCollision(collision)) return;
        if (isThrow) return;

        Throw();

        Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();

        if (playerRb != null)
        {
            StartCoroutine(MoveAlongCurve(playerRb));
            GameManager.instance.TimerStop();
        }
    }

    // Collision어떤거 충돌했는지 체크
    private bool IsValidCollision(Collision collision)
    {
        if (collision.contacts.Length == 0) 
            return false;

        foreach (var contact in collision.contacts)
        {
            if (contact.thisCollider == throwCollider &&
                collision.gameObject.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

    private void Throw()
    {
        isThrow = true;
        StartCoroutine(ThrowSystem());
    }

    // 플레이어가 날라가는 동작구현
    private IEnumerator MoveAlongCurve(Rigidbody playerRb)
    {
        Vector3 startPos = playerRb.position;
        Vector3 controlPos = pointTransform.position;
        Vector3 endPos = palletTownTransform.position;

        float elapsed = 0f;

        while (elapsed < throwDuration)
        {
            float t = elapsed / throwDuration;

            Vector3 bezierPos = Mathf.Pow(1 - t, 2) * startPos
                              + 2 * (1 - t) * t * controlPos
                              + Mathf.Pow(t, 2) * endPos;

            playerRb.MovePosition(bezierPos);
            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        playerRb.MovePosition(endPos);
    }

    // 던지는 시스템
    private IEnumerator ThrowSystem()
    {
        yield return RotateQuickly();
        yield return new WaitForSeconds(waitTime);
        yield return RecoverRotation();
        isThrow = false;
    }

    /// 오브젝트 던지기
    private IEnumerator RotateQuickly()
    {
        float timer = 0f;
        while (timer < rotateDuration)
        {
            transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }
    }

    /// 원상태 복구
    private IEnumerator RecoverRotation()
    {
        Quaternion currentRotation = transform.rotation;
        float timer = 0f;
        while (timer < backTime)
        {
            transform.rotation = Quaternion.Slerp(currentRotation, originalRotation, timer / backTime);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.rotation = originalRotation;
    }
}
