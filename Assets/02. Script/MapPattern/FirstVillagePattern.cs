using UnityEngine;

public class FirstVillagePattern : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            // 태초마을 함수 실행
        }
    }

    
}
