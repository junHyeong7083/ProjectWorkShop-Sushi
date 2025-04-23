using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;
public class MouseHover : MonoBehaviour
{
    public Text[] buttonTexts;

    public void SetHighlight(int index, bool isHighlight)
    {
        if (index < 0 || index >= buttonTexts.Length) return;

        buttonTexts[index].color = isHighlight ? Color.yellow : Color.white;
    }
    public int currentText(int idx) => idx;

    public void MouseHoverIn()
    {
        
    }

    public void MouseHoverOut()
    {

    }
}
