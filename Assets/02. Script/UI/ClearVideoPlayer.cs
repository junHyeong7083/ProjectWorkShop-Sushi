using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;
public class ClearVideoPlayer : MonoBehaviour
{
    [SerializeField] RawImage clearPanelRawImage;

    [SerializeField] VideoClip[] clips;

    [SerializeField] float fadeDuration = 1f;

    VideoPlayer videoPlayer;

    private void Start()
    {
        InitRenderTexture();


        videoPlayer = clearPanelRawImage.GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += OnVideoEnd;

        UIManager.Instance.OnClear += PlayClearVideo;
    }

    void InitRenderTexture()
    {
        RenderTexture rt = videoPlayer.targetTexture;
        RenderTexture.active = rt;
        GL.Clear(true, true, Color.clear);
        RenderTexture.active = null;
    }


    void PlayClearVideo(int score)
    {
        clearPanelRawImage.gameObject.SetActive(true);
     
        videoPlayer.clip = (score > 15) ? clips[0] : clips[1];
        StartCoroutine(FadeInAndPlay());
    }

    IEnumerator FadeInAndPlay()
    {
        Color color = clearPanelRawImage.color;
        color.a = 0f;
        clearPanelRawImage.color = color;

        float time = 0f;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            color.a = Mathf.Clamp01(time / fadeDuration);
            clearPanelRawImage.color = color;
            yield return null;
        }

        videoPlayer.Play();
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        // 엔딩크리딧 씬으로 넘어감
        SceneLoadManager.instance.LoadScene(2);
    }
}
