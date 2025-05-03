using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
public class TextHoverButton : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    [Header("텍스트 및 효과 설정")]
    [SerializeField] private Text buttonText;
    [SerializeField] private int highlightFontSize;


    int defaultFontSize;

    private void Awake()
    {
        if(buttonText == null) buttonText = GetComponentInChildren<Text>();
        defaultFontSize = buttonText.fontSize;
    }


    public void OnPointerEnter(PointerEventData eventData) => buttonText.fontSize = highlightFontSize;
    public void OnPointerExit(PointerEventData eventData) => buttonText.fontSize = defaultFontSize;

    public void OnClick() => SoundManager.Instance.PlaySFXSound("clickSfx");
}
