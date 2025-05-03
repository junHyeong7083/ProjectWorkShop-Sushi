using UnityEngine;

public class CursorController : MonoBehaviour
{
    public enum CursorMode
    {
        LockedHidden,
        UnlockedVisible
    }

    [SerializeField] CursorMode mode = CursorMode.LockedHidden;
    private void Start()
    {
        ApplyCursorMode(mode);
    }

    public static void ApplyCursorMode(CursorMode mode)
    {
        switch (mode)
        {
            case CursorMode.LockedHidden:
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;

            case CursorMode.UnlockedVisible:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true; break;

        }
    }
}
