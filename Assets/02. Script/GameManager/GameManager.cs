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

    public Volume deathVolume; 
    public float fadeSpeed = 1f;

    private void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
            instance = this;

        Time.timeScale = 1f;
        TimerOffset = Timer;
        gameOverPanel.gameObject.SetActive(false);
        deathVolume.weight = 0f;
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

    public void GameOver()
    {
        StartCoroutine(ActivateGrayEffect());

        replayPlayer.PlayReplay(() => {  SceneReload(); });
    }

    private IEnumerator ActivateGrayEffect()
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * fadeSpeed;
            deathVolume.weight = t;
            Debug.Log(deathVolume.weight);
            yield return null;
        }

        deathVolume.weight = 1f;
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
