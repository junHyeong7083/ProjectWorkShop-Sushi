using UnityEngine;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ReplayPlayer : MonoBehaviour
{
    [Header("Player && Camera Settings")]
    public Transform Player;
    public Transform Camera;

    public ReplayRecorder recorder;
    private CinemachineBrain brain;
    ShaderCountUpdater shaderUpdater;
    [Header("Replay Time")]
    [SerializeField] private float replayTime = 0.5f;

    [Header("Death Effect Settings")]
    [SerializeField] private Volume deathVolume; 
    [SerializeField] private float grayEffectDuration = 1f;

    private void Start()
    {
        Time.timeScale = 1f;
        shaderUpdater = GetComponent<ShaderCountUpdater>();
        brain = Camera.GetComponent<CinemachineBrain>();
        if (deathVolume != null)
            deathVolume.weight = 0f; // 처음엔 꺼두기
    }

    public void PlayReplay(System.Action onComplete)
    {
        StartCoroutine(ActivateGrayEffect());
        StartCoroutine(ReplayCoroutine(onComplete));
    }

    private IEnumerator ReplayCoroutine(System.Action onComplete)
    {
        ReplayManager.Instance.StartReplay();
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
        ReplayManager.Instance.StopReplay();

        shaderUpdater.AddCount();
        // 콜백(씬 리로드용)
        onComplete?.Invoke();
    }


    private IEnumerator ActivateGrayEffect()
    {
        if (deathVolume == null)
            yield break;

        float timer = 0f;
        while (timer < grayEffectDuration)
        {
            deathVolume.weight = Mathf.Lerp(0f, 1f, timer / grayEffectDuration);
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        deathVolume.weight = 1f;
    }
}
