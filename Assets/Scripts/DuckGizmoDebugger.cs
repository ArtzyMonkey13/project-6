using UnityEngine;

[ExecuteAlways]
public class DuckGizmoDebugger : MonoBehaviour
{
    public float detectionRadius = 5f;
    public Color detectionColor = new Color(1f, 0.5f, 0f, 0.25f);
    public Color wanderTargetColor = Color.green;
    public Color fleeDirectionColor = Color.blue; // New color for flee direction

    private DuckAvoidsPlayerWithoutNavMesh duckAI;

    void OnDrawGizmos()
    {
        if (duckAI == null)
            duckAI = GetComponent<DuckAvoidsPlayerWithoutNavMesh>();

        if (duckAI == null) return;

        // Draw detection radius
        Gizmos.color = detectionColor;
        Gizmos.DrawWireSphere(transform.position, duckAI.detectionRadius);

        // Draw wander target
        Gizmos.color = wanderTargetColor;
        Gizmos.DrawLine(transform.position, duckAI.wanderTarget);
        Gizmos.DrawSphere(duckAI.wanderTarget, 0.3f);

        // If fleeing from player, draw the flee direction
        if (Vector3.Distance(transform.position, duckAI.player.position) < duckAI.detectionRadius)
        {
            Gizmos.color = fleeDirectionColor;
            Vector3 fleeDirection = (transform.position - duckAI.player.position).normalized;
            Gizmos.DrawRay(transform.position, fleeDirection * duckAI.detectionRadius); // Direction of flee
        }
    }
}
