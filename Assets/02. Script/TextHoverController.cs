using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TextHoverController : MonoBehaviour
{
    public static TextHoverController instance;

    public Text[] btnTexts; // 0, 1, 2 텍스트

    [Header("기본 폰트사이즈가 130")]
    [SerializeField] int highlightFont = 130;
    int initFontSize = 0;

    [Header("Start - 0 \nOption - 1 \nExit - 2")]
    [SerializeField] Color[] highlightColor;
    private void Awake()
    {
        if (instance == null)
            instance = this;

        initFontSize = btnTexts[0].fontSize;

        
        Cursor.lockState = CursorLockMode.None;
    }
    private void Start()
    {
        // title씬에 있는 유일한 dondestroy cs => 여기서 title 올떄마다 쉐이더 값 0초기화
        DataManager.Instance.deathCount = 0;
    }

    public void SetHighlight(int index, bool isHighlight)
    {
        if (index < 0 || index >= btnTexts.Length) return;

        // colorHighlight 테스트용으로 yellow, white
        btnTexts[index].color = isHighlight ? highlightColor[index] : Color.white;

        // fontHighlight 
        btnTexts[index].fontSize = isHighlight ? highlightFont : initFontSize;


        // soundmanager.instace.playsfx(" ");
    }


    public void buttonClickSound()
    {
        SoundManager.Instance.PlaySFXSound("clickSfx");
    }
}
