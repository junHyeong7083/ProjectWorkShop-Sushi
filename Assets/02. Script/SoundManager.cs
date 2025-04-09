using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public float masterVolume;
    public float bgmVolume;
    public float sfxVolume;

    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SoundManager>();
            }

            return instance;
        }
    }

    private AudioSource bgmPlayer;
    private AudioSource sfxPlayer;


    [SerializeField]
    private AudioClip MainBgmAudioClip;

    [SerializeField]
    private AudioClip[] sfxAudioClips;

    Dictionary<string, AudioClip> audioClipsDic = new Dictionary<string, AudioClip>();

    bool IsPause = false;

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            //  Debug.Log("???!!!");
            DontDestroyOnLoad(this.gameObject);
        }
        bgmPlayer = GetComponentsInChildren<AudioSource>()[0];
        sfxPlayer = GetComponentsInChildren<AudioSource>()[1];

        foreach (AudioClip audioclip in sfxAudioClips)
        {
            audioClipsDic.Add(audioclip.name, audioclip);
        }

    }

    // SFX
    public void PlaySFXSound(string name, float volume = 1f)
    {
        if (audioClipsDic.ContainsKey(name) == false)
        {
            return;
        }

        if (!IsPause)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = audioClipsDic[name];
            audioSource.Play();

            // 재생 중인 AudioSource 저장
            playingAudios[name] = audioSource;
        }
    }
    public void StopAudioClip(string name)
    {
        if (playingAudios.ContainsKey(name))
        {
            AudioSource audioSource = playingAudios[name];
            audioSource.Stop();
            Destroy(audioSource);  // AudioSource 제거
            playingAudios.Remove(name);
        }
    }
    // BGM 
    public void SetBGMSound(int bgm_num, float volume = 1f)
    {
        bgmPlayer.loop = true;

        if (bgm_num == 1)
        {
            bgmPlayer.clip = MainBgmAudioClip;
        }
    }
    private Dictionary<string, AudioSource> playingAudios = new Dictionary<string, AudioSource>();

    // Sound Play
    public void PlaySound()
    {
        bgmPlayer.Play();
        //  Debug.Log("player");
        if (IsPause)
            IsPause = false;
    }

    // Sound Pause
    public void PauseSound()
    {
        bgmPlayer.Pause();

        if (!IsPause)
            IsPause = true;
    }

    public void SFXPauseSound()
    {
        sfxPlayer.Pause();

        if (!IsPause)
            IsPause = true;
    }
}