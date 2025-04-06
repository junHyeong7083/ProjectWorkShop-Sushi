using UnityEngine;
using System.Collections;

public class SushiSquashOscillation : MonoBehaviour
{
    private Vector3 originalScale;

    // 기존 squashOscillate 관련 변수들 (착지 애니메이션)
    public float squashAmount = 0.7f;
    public float oscillationDuration = 0.5f;
    public int oscillationCount = 5;
    public float lateralStretch = 0.2f;
    private bool isSquashing = false;

    // 점프 애니메이션용 지속 진동 변수들
    public float jumpOscillationAmplitude = 0.05f; // 예: 5% 정도의 스케일 변화
    public float jumpOscillationFrequency = 2f;    // 진동 속도 (Hz)
    private Coroutine jumpCoroutine;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    // 착지 애니메이션: 점프 후 착지 시 squash 효과
    public void SquashOscillate()
    {
        if (!isSquashing)
        {
            StopAllCoroutines();
            StartCoroutine(SquashOscillationCoroutine());
        }
    }

    private IEnumerator SquashOscillationCoroutine()
    {
        isSquashing = true;
        for (int rep = 0; rep < oscillationCount; rep++)
        {
            // 각 반복마다 효과가 점진적으로 줄어듦
            float amplitudeFactor = 1.0f - ((float)rep / (float)oscillationCount);
            float elapsed = 0f;
            while (elapsed < oscillationDuration)
            {
                elapsed += Time.deltaTime;
                // 0 ~ π 사이를 sine 함수로 매핑하여 효과를 부드럽게 전개
                float t = Mathf.Sin((elapsed / oscillationDuration) * Mathf.PI);
                // X, Z 축은 lateralStretch만큼 늘어나고, Y 축은 squashAmount 만큼 눌림
                float scaleX = originalScale.x * (1 + lateralStretch * amplitudeFactor * t);
                float scaleY = originalScale.y * (1 - (1 - squashAmount) * amplitudeFactor * t);
                float scaleZ = originalScale.z * (1 + lateralStretch * amplitudeFactor * t);
                transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
                yield return null;
            }
        }
        transform.localScale = originalScale;
        isSquashing = false;
    }

    // 점프 애니메이션: 점프 중 지속적으로 살짝 움츠렸다가 펴지는 효과
    public void StartJumpOscillation()
    {
        if (jumpCoroutine != null)
            StopCoroutine(jumpCoroutine);
        jumpCoroutine = StartCoroutine(JumpOscillationCoroutine());
    }

    public void StopJumpOscillation()
    {
        if (jumpCoroutine != null)
        {
            StopCoroutine(jumpCoroutine);
            jumpCoroutine = null;
            transform.localScale = originalScale; // 효과 종료 후 원래 스케일 복원
        }
    }

    private IEnumerator JumpOscillationCoroutine()
    {
        while (true)
        {
            // Time.time을 기준으로 sine 함수를 적용해 지속적인 스케일 진동 구현
            float scaleFactor = 1 + Mathf.Sin(Time.time * jumpOscillationFrequency) * jumpOscillationAmplitude;
            transform.localScale = originalScale * scaleFactor;
            yield return null;
        }
    }
}
