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
        commentText.text = commentData.comments[rand].text;

        // 텍스트의 실제 크기를 가져옴
        commentText.ForceMeshUpdate(); // 강제로 업데이트
        var textSize = commentText.textBounds.size;

        // 텍스트 길이에 따라 배경의 가로 사이즈 조절
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
