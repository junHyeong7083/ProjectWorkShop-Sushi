#if UNITY_EDITOR
using UnityEngine;

[ExecuteAlways]
public class GizmoHelper : MonoBehaviour
{
    [SerializeField] private Transform[] wayPoints;

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
}
#endif
