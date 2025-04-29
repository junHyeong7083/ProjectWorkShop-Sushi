using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
#if UNITYEDITR
using UnityEditor.Build;
using UnityEditor.Experimental.GraphView;
#endif
public class CutScene : MonoBehaviour
{
    public static CutScene instance; 

    public Image[] cutsceneImageBG;
    public Image[] cutsceneImage;

    public CanvasGroup canvasGroup;

    public float fadeDuration = 1f;
    public float intervalBetweenImages = 2.0f; // 이미지끼리 등장 간격

    CutSceneCamera cutSceneCamera;

    [Header("들어갈 텍스트 내용 0 1 2 3 순서대로 기입")]
    public string[] text;
    [SerializeField] float typingDuration; // 텍스트 타이핑 시간

    [HideInInspector]
    public bool showCutScene = false;
    [HideInInspector]
    public int isOnceShow;
    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else
            instance = this;
    }


    private void Start()
    {
       // typingDuration = fadeDuration; // 텍스트타이핑시간은 이미지가 페이드 되는동안
        isOnceShow = PlayerPrefs.GetInt("isOnceShow");
        cutSceneCamera = GetComponent<CutSceneCamera>();

        if (isOnceShow == 0)
        {
            canvasGroup.gameObject.SetActive(true);
            StartCoroutine(PlayCutscene());
        }
        else
        {
            canvasGroup.gameObject.SetActive(false);
            showCutScene = false;

            cutSceneCamera.cutsceneCam.Priority = 0;
            cutSceneCamera.playerCam.Priority = 20;
        }

       // canvasGroup.gameObject.SetActive(true);
       // StartCoroutine(PlayCutscene());


    }

    IEnumerator PlayCutscene()
    {
        showCutScene = true;
        for (int e = 0; e < cutsceneImageBG.Length; e++)
        {
            var imgBG = cutsceneImageBG[e];
            var imgCutScene = cutsceneImage[e];
            imgBG.gameObject.SetActive(true);

            // Image 자식 중 TextMeshProUGUI 가져오기
            var textComponent = imgBG.GetComponentInChildren<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = ""; // 처음은 비워두고
            }

            yield return StartCoroutine(FadeImage(imgBG, imgCutScene, 0, 1)); // 이미지 페이드 인

            if (textComponent != null)
            {
                yield return StartCoroutine(TypeText(textComponent, text[e]));
                // 여기서 Text 이름을 내용으로 사용 (아니면 Text 설정해둔 내용을 가져올 수도 있음)
            }

            yield return new WaitForSeconds(intervalBetweenImages);
        }

        // 모든 컷신 등장 끝나면 전체 페이드 아웃
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(FadeCanvasGroup(canvasGroup, 1, 0));
    }

    IEnumerator FadeImage(Image imgBG,Image cutscene, float from, float to)
    {
        float time = 0;
        Color bgColor = imgBG.color;
        Color cutColor = cutscene.color;
        while (time < fadeDuration)
        {
            bgColor.a = Mathf.Lerp(from, to, time / fadeDuration);
            cutColor.a = Mathf.Lerp(from, to, time / fadeDuration);

            imgBG.color = bgColor;
            cutscene.color = cutColor;
            time += Time.deltaTime;
            yield return null;
        }
        bgColor.a = to;
        cutColor.a = to;

        imgBG.color = bgColor;
        cutscene.color = cutColor;
    }


    IEnumerator FadeCanvasGroup(CanvasGroup group, float from, float to)
    {
        float time = 0;
        while (time < fadeDuration)
        {
            group.alpha = Mathf.Lerp(from, to, time / fadeDuration);
            time += Time.deltaTime;
            yield return null;
        }
        group.alpha = to;

        showCutScene = false;

        canvasGroup.gameObject.SetActive(false);
        // 한번 컷신 보여주면 값1로 변경후 저장
        PlayerPrefs.SetInt("isOnceShow" , 1);
        PlayerPrefs.Save();

        // 캔버스까지 모두 끝나면 카메라 무빙으로 도착지점 보여주기!!!!!
        cutSceneCamera.StartCutSceneCameraMoving();

    }

    IEnumerator TypeText(TextMeshProUGUI textComponent, string fullText)
    {
        textComponent.text = "";
        float time = 0f;
        int totalLength = fullText.Length;
        int lastlen = 0;

        while (time < typingDuration)
        {
            int curlen = Mathf.FloorToInt(Mathf.Lerp(0, totalLength, time / typingDuration));

            if (curlen > lastlen) // 새 글자가 생겼을 때만
            {
                int rand = Random.Range(0, 12);
                string soundName = "keyboard_" + (rand + 1);
                SoundManager.Instance.PlaySFXSound(soundName, 0.1f);

                textComponent.text = fullText.Substring(0, curlen);
                lastlen = curlen;
            }


            time += Time.deltaTime;
            yield return null;
        }


        textComponent.text = fullText; // 마지막 보정
    }
}
