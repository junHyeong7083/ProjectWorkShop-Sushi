using UnityEngine;
using System;

public class ReplayManager : MonoBehaviour
{
    public static ReplayManager Instance { get; private set; }

    public bool IsReplaying { get; private set; }

    public event Action<bool> OnReplayStateChanged; // 선택사항

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void StartReplay()
    {
        IsReplaying = true;
        Debug.Log(IsReplaying);
        OnReplayStateChanged?.Invoke(true);
    }

    public void StopReplay()
    {
        IsReplaying = false;
        OnReplayStateChanged?.Invoke(false);
    }
}
