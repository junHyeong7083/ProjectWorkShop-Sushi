using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    float mouseSensitivity;
    private float rotY;

    void Start()
    {
        mouseSensitivity = DataManager.Instance.mouseSensibility;
        rotY = transform.localEulerAngles.y;
    }


    void Update()
    {
        if (ReplayManager.Instance.IsReplaying || CutScene.instance.showCutScene) return;
        float mouseX = Input.GetAxis("Mouse X");

        rotY += mouseX * mouseSensitivity;
        transform.rotation = Quaternion.Euler(0f, rotY, 0f);
    }
}
