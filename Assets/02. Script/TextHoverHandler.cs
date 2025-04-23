using UnityEngine;
using UnityEngine.EventSystems;

public class TextHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private int index;
    public void OnPointerEnter(PointerEventData eventData)
    {
        TextHoverController.instance.SetHighlight(index, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TextHoverController.instance.SetHighlight(index, false);
    }
}
