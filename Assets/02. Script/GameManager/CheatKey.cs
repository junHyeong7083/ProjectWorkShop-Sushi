using UnityEngine;
using UnityEngine.Timeline;

public class CheatKey : MonoBehaviour
{
    public GameObject Player;
    public GameObject GoalCheatKey;
    public GameObject cupPatternCheatKey;
    void Update()
    {
        // f4 도착지점
        if(Input.GetKeyDown(KeyCode.F4))
            Player.transform.position = GoalCheatKey.transform.position;

        // F3누르면 컵 패턴 으로
        if (Input.GetKeyDown(KeyCode.F3))
            Player.transform.position = cupPatternCheatKey.transform.position;

        /// 바로 타이틀로
        if (Input.GetKeyDown(KeyCode.F5))
            SceneLoadManager.instance.LoadScene(0);

        /// 엔딩씬으로
        if (Input.GetKeyDown(KeyCode.F6))
            SceneLoadManager.instance.LoadScene(2);
    }

}
