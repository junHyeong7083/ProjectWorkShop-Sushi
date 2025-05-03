using UnityEngine;
using System;

public class ReplayManager : MonoBehaviour
{
    public static ReplayManager Instance { get; private set; }
    public bool IsReplaying { get; private set; }
    public event Action<bool> OnReplayStateChanged;

    [SerializeField] private ReplayPlayer replayPlayer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        UIManager.Instance.OnReplay += () =>
        {
            PlayReplay(() => SceneLoadManager.instance.ReloadScene());
        };
    }

    public void PlayReplay(Action onComplete)
    {
        IsReplaying = true;
        OnReplayStateChanged?.Invoke(true);
        replayPlayer.PlayReplay(() =>
        {
            IsReplaying = false;
            OnReplayStateChanged?.Invoke(false);
            onComplete?.Invoke();
        });
    }
}
