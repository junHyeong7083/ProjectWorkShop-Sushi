using UnityEngine;

public class CheatKey : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F2)) // 테스트용
            PlayerPrefs.DeleteAll();

        /// f3 누르면 컷신 기록 초기화
        if(Input.GetKeyDown(KeyCode.F3))
           CutScene.instance. isOnceShow = PlayerPrefs.GetInt("isOnceShow",0);
    }

}
