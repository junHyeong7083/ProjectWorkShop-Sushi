using UnityEngine;

public class TTTTT : MonoBehaviour
{
    public float mouseSensitivity = 2.0f;
    private float rotY;

    void Start()
    {
        rotY = transform.localEulerAngles.y;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");

        rotY += mouseX * mouseSensitivity;
        transform.rotation = Quaternion.Euler(0f, rotY, 0f);
    }
}
