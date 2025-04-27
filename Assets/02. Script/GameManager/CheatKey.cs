using UnityEngine;

public class CheatKey : MonoBehaviour
{


    void Update()
    {
        /// f1 누르면 점점 상하게  f2 누르면 다시 원래대로
        if (Input.GetKeyDown(KeyCode.F1))
        {

        }
        if (Input.GetKeyDown(KeyCode.F2))
        {

        }


        if(Input.GetKeyDown(KeyCode.F4)) // 테스트용
            PlayerPrefs.DeleteAll();

        /// f3 누르면 컷신 기록 초기화
        if(Input.GetKeyDown(KeyCode.F3))
           CutScene.instance. isOnceShow = PlayerPrefs.GetInt("isOnceShow",0);
    }

}
