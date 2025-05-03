using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
public class RotatePattern : MonoBehaviour
{
    [SerializeField] GameObject CupObject;
    BoxCollider cupCollider;

    [Header("회전할때 걸리는 시간")]
    [SerializeField] float rotateTime;

    [Header("회전하고 다시 회전하며 돌아올 시간")]
    [SerializeField] float initrotateTime; 
    Vector3 originRot = new Vector3(0, 0, 0);
    Vector3 targetRot = new Vector3(0, 0, -180);

    [Header("몇초뒤에 초기화 들어갈건지")]
    [SerializeField] float InitTime;
    // initTime 이 지나면 다시 원상복구
    private void Start()
    {
        cupCollider = CupObject.GetComponent<BoxCollider>();   
    }




    void RotateStart() => StartCoroutine(RotateCoroutine());


    IEnumerator RotateCoroutine()
    {
        float elapsed = 0f;
        //cupCollider.enabled = false;
        while (elapsed < rotateTime)
        {
            float t = elapsed / rotateTime;
            Vector3 rot = Vector3.Lerp(originRot, targetRot, t);
            CupObject.transform.localEulerAngles = rot;
            elapsed += Time.deltaTime;
            yield return null;
        }
        CupObject.transform.localEulerAngles = targetRot;
        // 2단계: 일정 시간 대기
        yield return new WaitForSeconds(InitTime);
        cupCollider.enabled = true;
        elapsed = 0f;
        while (elapsed < rotateTime)
        {
            float t = elapsed / rotateTime;
            Vector3 rot = Vector3.Lerp(targetRot, originRot, t);
            CupObject.transform.localEulerAngles = rot;
            elapsed += Time.deltaTime;
            yield return null;
        }
        CupObject.transform.localEulerAngles = originRot;
    }




    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("SIBAL");
            RotateStart();
        }
            
    }

}
