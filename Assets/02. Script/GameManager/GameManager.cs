using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public ReplayPlayer replayPlayer;
    public TMP_Text timerTMP;
    public float Timer = 3f;
    float TimerOffset;

    public GameObject gameOverPanel;
    public TMP_Text deathText;

    DeathCommentUI deathCommentUI;
    private void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
            instance = this;

        Time.timeScale = 1f;
        TimerOffset = Timer;
        gameOverPanel.gameObject.SetActive(false);

        deathCommentUI = GetComponent<DeathCommentUI>();
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
        replayPlayer.PlayReplay(() => {  SceneLoadManager.instance.ReloadScene(); });
        deathCommentUI.ShowComment();
    }

}
