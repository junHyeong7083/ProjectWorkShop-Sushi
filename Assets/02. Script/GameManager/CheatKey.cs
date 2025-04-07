using UnityEngine;

public class CheatKey : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F2)) // 테스트용
            PlayerPrefs.DeleteAll();
    }
}
