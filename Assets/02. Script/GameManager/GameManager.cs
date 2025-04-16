using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TMP_Text timerTMP;

    public float Timer = 3f;

    public GameObject gameOverPanel;
    public TMP_Text deathText;

    private void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
            instance = this;

        Time.timeScale = 1f;
        gameOverPanel.gameObject.SetActive(false);
    }

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


    private void GameOver()
    {
        gameOverPanel.gameObject.SetActive(true);
        Time.timeScale = 0f;

        int deathCount = PlayerPrefs.GetInt("DeathCount", 0);
        deathCount++; // 1 ����
        PlayerPrefs.SetInt("DeathCount", deathCount); // ����
        PlayerPrefs.Save(); 

        deathText.text = "Sushi : " + deathCount.ToString();
    }


    public void ReStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
