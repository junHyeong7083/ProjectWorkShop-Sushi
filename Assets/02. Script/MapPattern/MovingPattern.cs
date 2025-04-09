using System.Collections;
using UnityEngine;

public enum LoopType
{
    None,
    Loop,
    PingPong
}

[ExecuteAlways]
public class MovingPattern : MonoBehaviour
{
    [Header("자식오브젝트로 움직일 FBX모델을 넣으면 됨")]
    [SerializeField] private Transform movingTarget;

    [Space(20)]
    [Header("이동할 포인트 (startPos은 무조건 0번인덱스)")]
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
        if (!isMoving && isTriggerPattern && other.CompareTag("Player"))
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

                if (boxColliderObj != null)
                {
                    boxColliderObj.center = wayPoints[0].position;
                }
            }
        }
    }
#endif
}
