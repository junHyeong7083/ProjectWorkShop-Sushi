using UnityEngine;
using System.Collections;
public class SushiSquash : MonoBehaviour
{
    private Vector3 originalScale;
    private bool isSquashing = false;
    public float squashAmount = 0.7f;
    public float squashSpeed = 5f;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    public void Squash()
    {
        if (!isSquashing)
            StartCoroutine(SquashCoroutine());
    }

    private IEnumerator SquashCoroutine()
    {
        isSquashing = true;
            
        Vector3 squashedScale = new Vector3(
            originalScale.x * 1.2f, // 옆으로 퍼지게
            originalScale.y * squashAmount, // 위아래로 눌리게
            originalScale.z * 1.2f
        );

        // Squash
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * squashSpeed;
            transform.localScale = Vector3.Lerp(originalScale, squashedScale, t);
            yield return null;
        }

        // 다시 복원
        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * squashSpeed;
            transform.localScale = Vector3.Lerp(squashedScale, originalScale, t);
            yield return null;
        }

        isSquashing = false;
    }
}
