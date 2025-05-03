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

        if (Camera == null)
        {
            Debug.LogError("[ReplayPlayer] Camera가 할당되지 않았습니다.");
            return;
        }

        brain = Camera.GetComponent<CinemachineBrain>();
        if (brain == null)
        {
            Debug.LogWarning("[ReplayPlayer] CinemachineBrain 컴포넌트를 찾을 수 없습니다.");
        }

        if (deathVolume != null)
            deathVolume.weight = 0f;
    }

    public void PlayReplay(System.Action onComplete)
    {
        if (Player == null || Camera == null || recorder == null)
        {
            Debug.LogError("[ReplayPlayer] 필수 참조 값이 비어있습니다.");
            onComplete?.Invoke(); // 그래도 흐름은 유지
            return;
        }

        StartCoroutine(ActivateGrayEffect());
        StartCoroutine(ReplayCoroutine(onComplete));
    }

    private IEnumerator ReplayCoroutine(System.Action onComplete)
    {
        Time.timeScale = replayTime;

        if (brain != null)
            brain.enabled = false;

        var frames = recorder.FrameQueue.ToArray();
        if (frames.Length == 0)
        {
            Debug.LogWarning("[ReplayPlayer] 저장된 리플레이 프레임이 없습니다.");
            onComplete?.Invoke();
            yield break;
        }

        var rb = Player.GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = true;

        foreach (var frame in frames)
        {
            Player.position = frame.playerPosition;
            Player.rotation = frame.playerRotation;
            Camera.position = frame.cameraPosition;
            Camera.rotation = frame.cameraRotation;

            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }

        if (brain != null)
            brain.enabled = true;

        shaderUpdater?.AddCount();
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
