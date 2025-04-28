using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    [HideInInspector]
    public int deathCount = 0; // 죽을 때마다 올라가는 카운트


    [HideInInspector]
    public float mouseSensibility;
    [HideInInspector]
    public float soundValue;


    [SerializeField] Slider[] sliders; // 0 마우스  | 1 - 사운드


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
    private void Start()
    {
        soundValue = PlayerPrefs.GetFloat("SoundVolume", 0);
        sliders[1].value = soundValue;

        // value가 변경될때 이벤트 등록되게
        sliders[0].onValueChanged.AddListener(MouseSliderChanged);
        sliders[1].onValueChanged.AddListener(SoundSliderChanged);

    }

    void SoundSliderChanged(float newValue)
    {
        soundValue = newValue;
        PlayerPrefs.SetFloat("SoundVolume", soundValue); // 저장
        PlayerPrefs.Save();
        SoundManager.Instance.UpdateBGMVolume();
    }


    ///  value 0 : mouse 1    value1 : mouse 3  
    void MouseSliderChanged(float newValue) => mouseSensibility = Mathf.Lerp(1,3, newValue);
}

