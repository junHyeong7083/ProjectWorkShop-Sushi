using UnityEngine;
using System.Collections;
using Unity.Cinemachine;


public class ReplayPlayer : MonoBehaviour
{
    public Transform Player;
    public Transform Camera;
    public ReplayRecorder recorder;
    CinemachineBrain brain;
    [SerializeField] float replayTime;
    PlayerMovement2 movementScript;

    private void Start()
    {
        movementScript = Player.GetComponent<PlayerMovement2>();
        Time.timeScale = 1f;
        brain = Camera.GetComponent<CinemachineBrain>();
    }
    public void PlayReplay(System.Action onComplete)
    {
        StartCoroutine(ReplayCoroutine(onComplete));
    }

    private IEnumerator ReplayCoroutine(System.Action onComplete)
    {
        movementScript.isReplayMode = true;
        // 무지성으로 짬
        Time.timeScale = replayTime;

        brain.enabled = false;
        var frames = recorder.FrameQueue.ToArray();
        GetComponent<Rigidbody>().isKinematic = true;

        foreach (var frame in frames)
        {
            Player.position = frame.playerPosition;
            Player.rotation = frame.playerRotation;
            Camera.position = frame.cameraPosition;
            Camera.rotation = frame.cameraRotation;

            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        brain.enabled = true;
        movementScript.isReplayMode = false;

        onComplete?.Invoke();
    }
}
