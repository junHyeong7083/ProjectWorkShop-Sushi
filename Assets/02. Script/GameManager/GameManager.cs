using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public ReplayPlayer replayPlayer;
    public TMP_Text timerTMP;
    public float Timer = 3f;
    float TimerOffset;

    public GameObject gameOverPanel;
    public TMP_Text deathText;

    bool isOverlapDie = false;

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

        Cursor.lockState = CursorLockMode.Locked;
        deathCommentUI = GetComponent<DeathCommentUI>();
    }

    public void TimerInit() => Timer = TimerOffset;

    public void TimerSub()
    {
        Timer -= Time.deltaTime;
        if (Timer < 0f)
        {
            Timer = 0f;
            Debug.Log("TimerOut");
            GameOver();
        }

        timerTMP.text = Timer.ToString("F2");
    }

    public void GameOver()
    {
        if(!isOverlapDie)
        {
            isOverlapDie = true;
            replayPlayer.PlayReplay(() => { SceneLoadManager.instance.ReloadScene(); });
            deathCommentUI.ShowComment();

            int rand = Random.Range(0, 4);

            switch (rand)
            {
                case 0:
                    SoundManager.Instance.PlaySFXSound("fail1Sfx");
                    break;

                case 1:
                    SoundManager.Instance.PlaySFXSound("fail2Sfx");
                    break;

                case 2:
                    SoundManager.Instance.PlaySFXSound("fail3Sfx");
                    break;

                case 3:
                    SoundManager.Instance.PlaySFXSound("fail4Sfx");
                    break;
            }
        }
      
    }

}
