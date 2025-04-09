using System.Collections;
using System.Drawing;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FallingPattern : MonoBehaviour
{
    [Space(20)]
    [Header("플레이어가 올라간 후 몇초뒤에 떨어질것인가")]
    [SerializeField] float delayTime;

    [Space(20)]
    [Header("몇초뒤에 리셋시킬지")]
    [SerializeField] float resetTime;

    Rigidbody rb;

    private Vector3 originPosition;
    private Quaternion originRotation;

    bool isFalling = false;

    MeshRenderer meshRenderer;
    Material material;
    UnityEngine.Color color;
    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        material = meshRenderer.material;
        color = material.color;

        rb = GetComponent<Rigidbody>(); 
        originPosition = this.transform.position;
        originRotation = this.transform.rotation;
            
        rb.isKinematic = true;
        rb.useGravity = false;

        rb.automaticCenterOfMass = false;
        rb.automaticInertiaTensor = false;
    }

    void FallingStart()
    {
        // 무게 중심 살짝 이동 (랜덤으로 해도 되고 고정으로 해도 됨)
        rb.centerOfMass = new Vector3(Random.Range(-0.5f, 0.5f), 0f, Random.Range(-0.5f, 0.5f));
        StartCoroutine(Falling());
    }

    IEnumerator Falling()
    {
        isFalling = true;
        // 1. delayTime 만큼 기다린다
        yield return new WaitForSeconds(delayTime);

        // 2. 떨어지게 한다
        rb.useGravity = true;
        rb.isKinematic = false;

        // 추가: 토크(회전력) 주기
        rb.AddTorque(new Vector3(
            Random.Range(-50f, 50f),
            Random.Range(-50f, 50f),
            Random.Range(-50f, 50f)));
        

        // 3. 떨어진 후 resetTime 만큼 기다린다
                yield return new WaitForSeconds(resetTime);

        // 4. 알파값 1초 동안 0으로 만들기
        float duration = 1f;
        float elapsed = 0f;
        var renderer = this.GetComponent<Renderer>();
        var originalColor = renderer.material.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            var color = originalColor;
            color.a = alpha;
            renderer.material.color = color;
            yield return null;
        }

        // 5. 위치와 회전 초기화
        this.transform.position = originPosition;
        this.transform.rotation = originRotation;

        // 6. 알파값 다시 1로 설정
        var resetColor = renderer.material.color;
        resetColor.a = 1f;
        renderer.material.color = resetColor;

        // 7. 중력 끄기
        rb.useGravity = false;
        rb.isKinematic = true;

        isFalling = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isFalling) // Player이랑 충돌하고 isFalling = false;
        {
            FallingStart();
            Debug.Log("fucking");
        }
    }


}
