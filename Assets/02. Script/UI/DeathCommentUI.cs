using UnityEngine;
using TMPro;

public class DeathCommentUI : MonoBehaviour
{
    public TextMeshProUGUI commentText;    // 표시할 텍스트
    public RectTransform backgroundImage;  // 배경 이미지 RectTransform

    public float paddingX = 50f;  // 텍스트 좌우 여백 (픽셀 단위)
    DeathCommentData commentData;

    void Awake()
    {
        commentData = JsonLoader.Load<DeathCommentData>("death_comments");
        if (commentData == null || commentData.comments == null || commentData.comments.Count == 0)
            Debug.LogError("[DeathCommentUI] 코멘트 데이터 x");
    }

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


}
