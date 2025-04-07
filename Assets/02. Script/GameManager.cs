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
        Debug.Log("fallll");
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
        deathCount++; // 1 증가
        PlayerPrefs.SetInt("DeathCount", deathCount); // 저장
        PlayerPrefs.Save(); // 실제로 저장 (생략 가능하지만 확실히 하려면 호출)

        deathText.text = "Sushi : " + deathCount.ToString();
    }

c

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
