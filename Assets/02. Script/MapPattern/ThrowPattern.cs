using System.Collections;
using UnityEngine;

public class ThrowPattern : MonoBehaviour
{
    bool isThrow = false;

    #region SettingValue
    [Header("회전속도 값이 오를수록 빠르게 회전합니다")]
    public float rotateSpeed = 1440f; // 초당 1440도 (매우 빠르게)

    [Space(20)]
    [Header("회전할 타이밍 : 값이 오를수록 오랫동안 회전합니다.")]
    public float rotateDuration = 0.3f; // 0.3초 동안 순간 회전

    [Space(20)]
    [Header("회전후 멈춰있을 시간")]
    public float waitTime = 5f; // 5초 대기

    [Space(20)]
    [Header("다시 원상태로 복귀될 시간")]
    public float backTime = 2f; // 2초 걸쳐 복귀

    [Space(20)]
    [Header("플레이어를 실질적으로 밀어내는힘 높을수록 멀리 보냅니다.")]
    public float throwPower = 20f; // 플레이어를 밀어내는 힘

    Quaternion originalRotation;

    [Space(20)]
    [Header("모델의 콜라이더")]
    [SerializeField] BoxCollider fbxCollider;

    [Space(20)]
    [Header("던질 콜라이더")]
    [SerializeField] BoxCollider throwCollider;

    [Space(20)]
    [Header("태초마을 위치좌표")]
    [SerializeField] Transform PalletTownTransform;


    [Space(20)]
    [Header("던져질때 꼭짓점 위치")]
    [SerializeField] Transform Pointtransform;
    #endregion
    private void Start()
    {
        originalRotation = transform.rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts.Length > 0)
        {
            foreach (var contact in collision.contacts)
            {
                if (contact.thisCollider == throwCollider)
                {
                    if (collision.gameObject.CompareTag("Player"))
                    {
                        if (!isThrow)
                        {
                            Throw();

                            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
                            if (playerRb != null)
                            {
                                // 기존 AddForce는 지우고
                                StartCoroutine(MoveAlongCurve(playerRb));
                            }
                        }
                    }
                    break; // 한번 처리했으면 끝내기
                }
            }
        }
    }



    void Throw()
    {
        isThrow = true;
        StartCoroutine(ThrowSystem());
    }

    IEnumerator MoveAlongCurve(Rigidbody playerRb)
    {
        Vector3 startPos = playerRb.position;
        Vector3 controlPos = Pointtransform.position;
        Vector3 endPos = PalletTownTransform.position;

        float duration = 2.0f; // 이동 총 시간
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            // 2차 베지어 곡선 공식
            Vector3 bezierPos = Mathf.Pow(1 - t, 2) * startPos
                              + 2 * (1 - t) * t * controlPos
                              + Mathf.Pow(t, 2) * endPos;

            playerRb.MovePosition(bezierPos); // Rigidbody로 부드럽게 이동
            elapsed += Time.fixedDeltaTime;   // FixedUpdate 시간에 맞춰 진행
            yield return new WaitForFixedUpdate();
        }

        playerRb.MovePosition(endPos); // 최종 위치 보정
    }



    IEnumerator ThrowSystem()
    {
        // 빠르게 z축 회전
        float timer = 0f;
        while (timer < rotateDuration)
        {
            transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        // 5초 대기
        yield return new WaitForSeconds(waitTime);

        // 원래 회전으로 천천히 복귀
        Quaternion currentRotation = transform.rotation;
        timer = 0f;
        while (timer < backTime)
        {
            transform.rotation = Quaternion.Slerp(currentRotation, originalRotation, timer / backTime);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.rotation = originalRotation;
        isThrow = false;
    }
}
