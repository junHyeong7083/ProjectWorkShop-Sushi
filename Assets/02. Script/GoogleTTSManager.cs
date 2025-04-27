using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class GoogleTTSManager : MonoBehaviour
{
    public static GoogleTTSManager Instance;
    [HideInInspector]
    public string apiKey = "AIzaSyBBBbNkedc61oN-2uf2cd7hmeLkplxJVXI";  


    private AudioSource audioSource;
    private string[] voiceNames = { "ko-KR-Wavenet-A", "ko-KR-Wavenet-B", "ko-KR-Wavenet-C", "ko-KR-Wavenet-D", "ko-KR-Standard-A", "ko-KR-Standard-B" };
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Speak(string text)
    {
        StartCoroutine(RequestTTS(text));
    }

    private IEnumerator RequestTTS(string text)
    {
        // url with api
        string url = "https://texttospeech.googleapis.com/v1/text:synthesize?key=" + apiKey;

        // 음성을 랜덤으로 선택
        string randomVoice = voiceNames[Random.Range(0, voiceNames.Length)];
        // 요청 JSON 만들기
        string jsonPayload = "{\"input\":{\"text\":\"" + text + "\"}," +
                              "\"voice\":{\"languageCode\":\"ko-KR\",\"name\":\"" + randomVoice + "\"}," +
                              "\"audioConfig\":{\"audioEncoding\":\"MP3\"}}";

        byte[] postData = Encoding.UTF8.GetBytes(jsonPayload);

        using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(postData);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("TTS 요청 실패: " + www.error);
            }
            else
            {
                // 응답 파싱
                string responseText = www.downloadHandler.text;
                string audioContent = JsonUtility.FromJson<TTSResponse>(responseText).audioContent;

                // base64 디코딩해서 오디오 데이터로 변환
                byte[] audioBytes = System.Convert.FromBase64String(audioContent);
                StartCoroutine(PlayAudioClip(audioBytes));
            }
        }
    }

    private IEnumerator PlayAudioClip(byte[] audioData)
    {
        string tempFilePath = Application.persistentDataPath + "/tts_temp_audio.mp3";
        System.IO.File.WriteAllBytes(tempFilePath, audioData); // 1. 파일로 저장

        using (var audioLoader = UnityWebRequestMultimedia.GetAudioClip("file://" + tempFilePath, AudioType.MPEG))
        {
            yield return audioLoader.SendWebRequest();

            if (audioLoader.result == UnityWebRequest.Result.Success)
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(audioLoader);
                audioSource.clip = clip;
                audioSource.Play();
            }
            else
            {
                Debug.LogError("오디오 재생 실패: " + audioLoader.error);
            }
        }
    }


    // 응답용 임시 클래스
    [System.Serializable]
    private class TTSResponse
    {
        public string audioContent;
    }
}
