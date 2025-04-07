using System.Collections;
using UnityEngine;

public enum LoopType
{
    None,       // 반복 없음 (끝나면 멈춤)
    Loop,       // 순방향 루프 (0-1-2-0-1-2)
    PingPong    // 왕복 (0-1-2-1-0-1-2-1-0)
}



[ExecuteAlways]
public class MovingPattern : MonoBehaviour
{
    [Header("자식오브젝트로 움직일 FBX모델을 넣으면 됨")]
    [SerializeField] private Transform movingTarget;

    [Space(20)]
    [Header("이동할 포인트 (wayPoints[0]은 무조건 시작지점이여야함)")]
    [SerializeField] private Transform[] wayPoints;

    [Space(10)]
    [Header("반복 방법 설정\nNone : 반복없음\nLoop : 0 1 2 0 1 2 ..\nPingPong : 0 1 2 1 0 1 2 ..")]
    [SerializeField] private LoopType loopType = LoopType.None;

    [Space(10)]
    [Header("true면 트리거로 이동 시작")]
    [SerializeField] private bool isTriggerPattern = false;

    [Space(10)]
    [Header("이동 속도")]
    [SerializeField] private float moveSpeed = 2f;

    [Space(10)]
    [Header("충돌처리 콜라이더")]
    [SerializeField] private BoxCollider boxColliderObj;



    private int currentTargetIndex = 0;
    private bool isMoving = false;
    private bool isForward = true; // PingPong 방향 제어용
#if UNITY_EDITOR
    private Vector3 lastWaypointPosition;
#endif

    private void Start()
    {
        if (!isTriggerPattern)
        {
            StartMoving();
        }
    }

    public void StartMoving()
    {
        // trigger 실행될때부터 패턴시작
        if (!isMoving)
        {
            isMoving = true;
            StartCoroutine(MovingSystem());
        }
    }

    private IEnumerator MovingSystem()
    {
        while (true)
        {
            Transform target = wayPoints[currentTargetIndex];

            while (Vector3.Distance(movingTarget.position, target.position) > 0.05f)
            {
                movingTarget.position = Vector3.MoveTowards(movingTarget.position, target.position, moveSpeed * Time.deltaTime);
                yield return null;
            }

            movingTarget.position = target.position;

            if (loopType == LoopType.Loop)
            {
                currentTargetIndex = (currentTargetIndex + 1) % wayPoints.Length;
            }
            else if (loopType == LoopType.PingPong)
            {
                if (isForward)
                {
                    currentTargetIndex++;
                    if (currentTargetIndex >= wayPoints.Length - 1)
                    {
                        isForward = false;
                    }
                }
                else
                {
                    currentTargetIndex--;
                    if (currentTargetIndex <= 0)
                    {
                        isForward = true;
                    }
                }
            }
            else // None
            {
                if (currentTargetIndex < wayPoints.Length - 1)
                {
                    currentTargetIndex++;
                }
                else
                {
                    break;
                }
            }

            yield return null;
        }

        isMoving = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 움직이지 않고 && trigger패턴이 TRUE가 되고 Player가 밟았을때
        if (!isMoving&&isTriggerPattern && other.CompareTag("Player"))
        {
            Debug.Log("nice");
            StartMoving();
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (movingTarget != null && wayPoints != null && wayPoints.Length > 0)
        {
            if (lastWaypointPosition != wayPoints[0].position)
            {
                movingTarget.position = wayPoints[0].position;
                lastWaypointPosition = wayPoints[0].position;

                // 이동할때 박스콜라이더의 위치도 변경해줌
                boxColliderObj.center = new Vector3(wayPoints[0].transform.position.x, wayPoints[0].transform.position.y, wayPoints[0].transform.position.z);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (wayPoints == null || wayPoints.Length < 2)
            return;

        Gizmos.color = Color.yellow;
        for (int i = 0; i < wayPoints.Length - 1; i++)
        {
            Gizmos.DrawLine(wayPoints[i].position, wayPoints[i + 1].position);
            Gizmos.DrawSphere(wayPoints[i].position, 0.1f);
        }
        Gizmos.DrawSphere(wayPoints[wayPoints.Length - 1].position, 0.1f);
    }
#endif
}