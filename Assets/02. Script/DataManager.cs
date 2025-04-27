using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    [HideInInspector]
    public int deathCount = 0; // 죽을 때마다 올라가는 카운트

    private void Awake()
    {
        // 싱글톤 패턴 + 씬 넘어가도 파괴 금지
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);


            if (SceneManager.GetActiveScene().name == "TitleScene")
                deathCount = 0;

            Debug.Log("deathcount : " + deathCount);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
