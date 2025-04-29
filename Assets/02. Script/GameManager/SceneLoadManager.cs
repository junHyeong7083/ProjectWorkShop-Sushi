using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    public static SceneLoadManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void LoadScene(int index)
    {
        if (index < 0 || index >= SceneManager.sceneCountInBuildSettings)
            return;

        SceneManager.LoadScene(index);
    }
    public void ReloadScene()
    {
        SoundManager.Instance.PlaySFXSound("respawnSfx");

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
    }
        
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
