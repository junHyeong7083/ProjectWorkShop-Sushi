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

    [SerializeField] RawImage clearPanelRawImage;
    [SerializeField] float fadeDuration;
    [SerializeField] VideoClip[] clips; // 0 실패 1 성공
    VideoPlayer videoPlayer;


    private void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
            instance = this;

        Time.timeScale = 1f;
        TimerOffset = Timer;

        Cursor.lockState = CursorLockMode.Locked;
        deathCommentUI = GetComponent<DeathCommentUI>();
    }

    private void Start()
    {
        clearPanelRawImage.gameObject.SetActive(false);
        videoPlayer = clearPanelRawImage.GetComponent<VideoPlayer>();

        videoPlayer.loopPointReached += OnVideoEnd;
    }

    bool isTimerPaused = false;

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
            Debug.Log("TimerOut");
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
        // 0 - 실패 || 1 - 성공
        videoPlayer.clip = (clearScore > 15) ? clips[0] : clips[1];

        // 패널 활성화 및 페이드인 + 영상 재생
        clearPanelRawImage.gameObject.SetActive(true);
        StartCoroutine(FadeInAndPlayVideo());
    }
    IEnumerator FadeInAndPlayVideo()
    {
        // 초기 세팅
        Color color = clearPanelRawImage.color;
        color.a = 0f;
        clearPanelRawImage.color = color;

        float time = 0f;

        // 페이드 인 루프

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / fadeDuration);
            color.a = t;
            clearPanelRawImage.color = color;
            yield return null;
        }

        // 재생 시작
        videoPlayer.Play();
        
    }
    private void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene("Title");
    }
    public void GameOver()
    {
        if (isOverlapDie || isCleared) return; 
        isOverlapDie = true;

        replayPlayer.PlayReplay(() => { SceneLoadManager.instance.ReloadScene(); });
        deathCommentUI.ShowComment();

        int rand = Random.Range(0, 4);
        SoundManager.Instance.PlaySFXSound($"fail{rand + 1}Sfx");
    }

}
