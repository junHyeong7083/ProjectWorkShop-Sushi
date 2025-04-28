using UnityEngine;
using Unity.Cinemachine;
using System.Collections;
public class CutSceneCamera : MonoBehaviour
{
    public CinemachineCamera cutsceneCam;
    public CinemachineCamera playerCam;

    public Transform startPoint;
    public Transform goalPoint;
    [SerializeField] float moveSpeed;
    [SerializeField] float waitTime;

    public void StartCutSceneCameraMoving()
    {
        cutsceneCam.Priority = 20;
        playerCam.Priority = 10;

        StartCoroutine(PlayCutscene());
    }


    private IEnumerator PlayCutscene()
    {
        // 출발 → 골 지점 이동
        yield return StartCoroutine(MoveCamera(startPoint.position, goalPoint.position,startPoint.rotation, goalPoint.rotation));

        // 골 지점에서 대기
        yield return new WaitForSeconds(waitTime);

     //   // 골 → 출발 지점 복귀
     //   yield return StartCoroutine(MoveCamera(goalPoint.position, startPoint.position, goalPoint.rotation, startPoint.rotation));

        // 컷신 끝 → 플레이어 조작 시작
        cutsceneCam.Priority = 0;
        playerCam.Priority = 20;
    }
    private IEnumerator MoveCamera(Vector3 fromPos, Vector3 toPos, Quaternion fromRot, Quaternion toRot)
    {
        float elapsed = 0f;
        float distance = Vector3.Distance(startPoint.position, goalPoint.position);
        float duration = distance / moveSpeed;
        Quaternion correctedToRot = ShortestRotation(fromRot, toRot);

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            cutsceneCam.transform.position = Vector3.Lerp(startPoint.position, goalPoint.position, t);
            cutsceneCam.transform.rotation = Quaternion.Slerp(startPoint.rotation, correctedToRot, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        cutsceneCam.transform.position = goalPoint.position;
        cutsceneCam.transform.rotation = goalPoint.rotation;
    }

    private Quaternion ShortestRotation(Quaternion from, Quaternion to)
    {
        if (Quaternion.Dot(from, to) < 0f)
        {
            to = new Quaternion(-to.x, -to.y, -to.z, -to.w);
        }
        return to;
    }
}
