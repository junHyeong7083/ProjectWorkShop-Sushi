using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public ReplayPlayer replayPlayer;
    public TMP_Text timerTMP;
    public float Timer = 3f;
    float TimerOffset;
    public TMP_Text deathText;

    bool isOverlapDie = false;

    DeathCommentUI deathCommentUI;
    bool isTimerPaused = false;
    private void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
            instance = this;

        Time.timeScale = 1f;
        TimerOffset = Timer;

        deathCommentUI = GetComponent<DeathCommentUI>();
    }


    public void TimerInit() 
    {
        Timer = TimerOffset;
        timerTMP.text = Timer.ToString("F2");
        isTimerPaused = false;
    }

    public void TimerSub()
    {
        if (isTimerPaused) return;

        Timer -= Time.deltaTime;
        if (Timer < 0f)
        {
            Timer = 0f;
            GameOver();
        }

        timerTMP.text = Timer.ToString("F2");
    }

    /// Throw Pattern -> Start Point : timer stop  ||| startPoint -> init() -> isTimerPaused : false
    public void TimerStop()
    {
        isTimerPaused = true;
    }

    private bool isCleared = false;
    public void GameClear()
    {
        if (isCleared) return;
        isCleared = true; 
        TimerStop();
        int clearScore = DataManager.Instance.deathCount;
        UIManager.Instance.OnGameStateChanged(GameState.Clear, clearScore);
    }
    public void GameOver()
    {
        if (isOverlapDie || isCleared) return; 
        isOverlapDie = true;

        UIManager.Instance.OnGameStateChanged(GameState.Replay);
        deathCommentUI.ShowComment();

        int rand = Random.Range(0, 4);
        SoundManager.Instance.PlaySFXSound($"fail{rand + 1}Sfx");
    }

}
