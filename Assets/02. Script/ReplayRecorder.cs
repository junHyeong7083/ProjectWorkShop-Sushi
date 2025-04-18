using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public struct ReplayFrame
{
    public Vector3 playerPosition;
    public Quaternion playerRotation;
    public Vector3 cameraPosition;
    public Quaternion cameraRotation;
}
public class ReplayRecorder : MonoBehaviour
{
    public Transform Player;
    public Transform Camera;

    public float duration = 1f;
    public float interval = 0.02f;


    private Queue<ReplayFrame> frameQueue = new Queue<ReplayFrame>();
    private float timer;
    public Queue<ReplayFrame> FrameQueue => new Queue<ReplayFrame>(frameQueue); // 복사본 반환
    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer >= interval)
        {
            timer = 0f;

            frameQueue.Enqueue(new ReplayFrame
            {
                playerPosition = Player.position,
                playerRotation = Player.rotation,
                cameraPosition = Camera.position,
                cameraRotation = Camera.rotation
            });

            while (frameQueue.Count > duration / interval)
                frameQueue.Dequeue();
        }
    }
}

