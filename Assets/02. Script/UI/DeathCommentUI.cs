using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DeathCommentUI : MonoBehaviour
{
    public TextMeshProUGUI commentText;    // 표시할 텍스트
    public RectTransform backgroundImage;  // 배경 이미지 RectTransform

    public float paddingX = 50f;  // 텍스트 좌우 여백 (픽셀 단위)
    public DeathCommentData commentData;

    private void Start()
    {
        backgroundImage.gameObject.SetActive(false);
    }

    public void ShowComment()
    {
        backgroundImage.gameObject.SetActive(true);

        int rand = Random.Range(0, commentData.comments.Count);
        string randomText = commentData.comments[rand].text;

        commentText.text = randomText;

        GoogleTTSManager.Instance.Speak(randomText);

        commentText.ForceMeshUpdate();
        var textSize = commentText.textBounds.size;
        backgroundImage.sizeDelta = new Vector2(textSize.x + paddingX, backgroundImage.sizeDelta.y);
    }


    private void Awake()
    {
        LoadComments();
    }
    private void LoadComments()
    {
        // Resources 폴더 기준 경로, 확장자 없이 파일명만
        TextAsset jsonFile = Resources.Load<TextAsset>("death_comments");

        if (jsonFile != null)
            commentData = JsonUtility.FromJson<DeathCommentData>(jsonFile.text);
        else
            Debug.LogError("death_comments.json 파일을 찾을 수 없습니다.");
    }
}
