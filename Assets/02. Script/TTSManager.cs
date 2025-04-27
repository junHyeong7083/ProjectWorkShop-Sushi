#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
using System.Speech.Synthesis;
#endif
using UnityEngine;

public class TTSManager : MonoBehaviour
{
    public static TTSManager Instance;

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
    private SpeechSynthesizer synthesizer;
#endif

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            try
            {
                synthesizer = new SpeechSynthesizer();
                synthesizer.Rate = 1;
                synthesizer.Volume = 100;
            }
            catch (System.Exception e)
            {
                Debug.LogError("TTS 초기화 실패: " + e.Message);
            }
#endif
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Speak(string text)
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        if (synthesizer != null && !string.IsNullOrEmpty(text))
        {
            synthesizer.SpeakAsyncCancelAll();  // 이전에 말하고 있던 거 끊고
            synthesizer.SpeakAsync(text);        // 새로 말하기
        }
#endif
    }
}
