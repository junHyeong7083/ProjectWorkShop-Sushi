using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class EndingCredit : MonoBehaviour
{
    [SerializeField] RectTransform creditRoot; // 텍스트 전체 묶음
    [SerializeField] TextMeshProUGUI sushiCountText;
    [SerializeField] Camera RenderCam;
    [SerializeField] float moveSpeed = 100f;

    [SerializeField] float sushiTimeStopped;

    [SerializeField] float fadeStartY = 2500f;
    [SerializeField] float fadeEndY = 4700f;

    private AudioSource bgmSource;

    private void Start()
    {
        RenderCam. GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
        RenderCam.GetComponent<Camera>().backgroundColor = new Color(0, 0, 0, 0); // 알파 0
        int percent = Mathf.Clamp(Mathf.RoundToInt((float)DataManager.Instance.deathCount / 30f * 100f), 0, 100);
        sushiCountText.text = $"스시의 상함정도 : {percent}%";

        SoundManager.Instance.PlayBGM(); // BGM 재생

        // SoundManager의 첫 번째 자식에서 AudioSource 가져오기
        bgmSource = SoundManager.Instance.transform.GetChild(0).GetComponent<AudioSource>();

        StartCoroutine(ScrollAndFadeCoroutine());
    }

    IEnumerator ScrollAndFadeCoroutine()
    {
        bool sushiStopDone = false;
        while (true)
        {
            // 텍스트 이동
            creditRoot.anchoredPosition += Vector2.up * moveSpeed * Time.deltaTime;

            float currentY = creditRoot.anchoredPosition.y;

            if (!sushiStopDone && currentY >= 1000f)
            {
                sushiStopDone = true; // 중복 멈춤 방지
                yield return new WaitForSeconds(sushiTimeStopped);
            }

            // 볼륨 줄이기 (2500~4700)
            if (currentY >= fadeStartY && currentY <= fadeEndY)
            {
                float t = Mathf.InverseLerp(fadeStartY, fadeEndY, currentY);
                bgmSource.volume = Mathf.Lerp(1f, 0f, t);
            }

            // 완전히 도달 시 종료 처리
            if (currentY > fadeEndY)
            {
                bgmSource.volume = 0f;
                SceneLoadManager.instance.LoadScene(0);
                yield break;
            }

            yield return null;
        }
    }
}
