using UnityEngine;
public enum GameState
{
    Gameplay, 
    Cutscene,
    Paused,
    Replay,
    Clear
}

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject optionPanel;

    public GameObject deathPanel;
    public GameObject cutsceneUI;


    [HideInInspector]
    public CanvasGroup cutsceneCanvasGroup;


    private GameState currentState = GameState.Gameplay;


    public event System.Action<int> OnClear;
    public event System.Action OnReplay;

    void Awake()
    {
        if (Instance != null) 
            Destroy(this.gameObject);
        
        Instance = this;

        cutsceneCanvasGroup = cutsceneUI.GetComponent<CanvasGroup>();
    }


    void Update()
    {
        // esc option panel
        if (Input.GetKeyDown(KeyCode.Escape) && 
            currentState == GameState.Gameplay)
            TogglePause(true);
        else if(Input.GetKeyDown(KeyCode.Escape) &&
            currentState == GameState.Paused) 
            TogglePause(false);
    }

    void TogglePause(bool stategamePlay)
    {
        if (currentState != GameState.Gameplay && currentState != GameState.Paused)
            return;
        bool paused = stategamePlay;

        optionPanel.SetActive(paused);
        currentState = paused ? GameState.Paused : GameState.Gameplay;
        Time.timeScale = paused ? 0 : 1;
        Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = paused;

    }

    // 컷신 시작
    public void EnterCutscene()
    {
        currentState = GameState.Cutscene;
        cutsceneUI.SetActive(true);
    }
    
    // 컷신이 끝났을때 -> 카메라 이동으로 연결
    public void EndCutscene()
    {
        cutsceneCanvasGroup.gameObject.SetActive(false);
        currentState = GameState.Gameplay;
        Time.timeScale = 1f;
    }

    // 게임씬에서 죽었을땐 컷신이 나올필요없음
    public void SkipCutScene()
    {
        currentState = GameState.Gameplay;
        cutsceneUI.SetActive(false);
        Time.timeScale = 1f;
    }


    public void OnGameStateChanged(GameState state, int score = 0)
    {
        currentState = state;

        switch(state)
        {
            case GameState.Replay:
                OnReplay?.Invoke();
                break;

            case GameState.Clear:
                OnClear?.Invoke(score);
                break;


        }
    }

    public void ForceReset()
    {
        currentState = GameState.Gameplay;
        optionPanel.SetActive(false);
        deathPanel.SetActive(false);
        cutsceneUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
    }
}
