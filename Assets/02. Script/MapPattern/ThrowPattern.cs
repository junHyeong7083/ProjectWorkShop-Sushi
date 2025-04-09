using System.Collections;
using UnityEngine;

public class ThrowPattern : MonoBehaviour
{
    bool isThrow = false;

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

    private void Start()
    {
        originalRotation = transform.rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!isThrow)
            {
                Throw();

                // 플레이어를 밀어내기
                Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
                if (playerRb != null)
                {
                    Vector3 throwDirection = (collision.transform.position - transform.position).normalized;
                    playerRb.AddForce(throwDirection * throwPower, ForceMode.Impulse);
                }
            }
        }
    }

    void Throw()
    {
        isThrow = true;
        StartCoroutine(ThrowSystem());
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
