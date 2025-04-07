using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // CameraPos를 드래그해서 넣을 것

    void FixedUpdate()
    {
        transform.position = target.position;
        //transform.rotation = // x는
    }
}
