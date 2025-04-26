using UnityEngine;

public class DeathCommentLoader : MonoBehaviour
{
    public DeathCommentData commentData;


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
        {
            Debug.LogError("death_comments.json 파일을 찾을 수 없습니다.");
        }
    }

    // 예시로 하나 출력
    public void PrintRandomComment()
    {
        if (commentData != null && commentData.comments.Count > 0)
        {
            int rand = Random.Range(0, commentData.comments.Count);
            Debug.Log(commentData.comments[rand].text);
        }
    }
}