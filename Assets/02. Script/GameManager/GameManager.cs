using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering; // 추가

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public ReplayPlayer replayPlayer;
    public TMP_Text timerTMP;
    public float Timer = 3f;
    float TimerOffset;

    public GameObject gameOverPanel;
    public TMP_Text deathText;

    private void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
            instance = this;

        Time.timeScale = 1f;
        TimerOffset = Timer;
        gameOverPanel.gameObject.SetActive(false);
    }

    public void TimerInit() => Timer = TimerOffset;

    public void TimerSub()
    {
        Timer -= Time.deltaTime;
        if (Timer < 0f)
        {
            Timer = 0f;
            GameOver();
        }

        timerTMP.text = Timer.ToString("F2");
    }


    public void SceneLoad(int sceneIdx)
    {
        // 예외
        if (sceneIdx < 0 || sceneIdx >= SceneManager.sceneCountInBuildSettings)
            return;

        SceneManager.LoadScene(sceneIdx);
    }

    public void GameOver()
    {
        replayPlayer.PlayReplay(() => {  SceneReload(); });
    }
   
    public void SceneReload() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    public void GameExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
