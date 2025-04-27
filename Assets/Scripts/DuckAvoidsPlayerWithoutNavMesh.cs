using UnityEngine;

public class DuckAvoidsPlayerWithoutNavMesh : MonoBehaviour
{
    public Transform player;
    public float detectionRadius = 5f;
    public float moveSpeed = 3f;
    public float wallAvoidDistance = 1.5f;
    public LayerMask wallLayer;

    public float wanderRadius = 5f;
    public float wanderInterval = 3f;

    private Vector3 moveDirection;
    private Vector3 wanderTarget;
    private float wanderTimer;

    void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                Debug.LogWarning("Player not assigned and could not be found by tag.");
            }
        }

        PickNewWanderTarget();
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRadius)
        {
            FleeFromPlayer();
        }
        else
        {
            Wander();
        }

        if (moveDirection != Vector3.zero)
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            transform.forward = moveDirection;
        }
    }

    void FleeFromPlayer()
    {
        Vector3 fleeDirection = (transform.position - player.position).normalized;

        // Avoid walls
        fleeDirection += GetWallAvoidanceVector();

        moveDirection = fleeDirection.normalized;
    }

    void Wander()
    {
        wanderTimer += Time.deltaTime;

        if (wanderTimer >= wanderInterval || Vector3.Distance(transform.position, wanderTarget) < 1f)
        {
            PickNewWanderTarget();
            wanderTimer = 0f;
        }

        Vector3 direction = (wanderTarget - transform.position).normalized;

        // Avoid walls
        direction += GetWallAvoidanceVector();

        moveDirection = direction.normalized;
    }

    void PickNewWanderTarget()
    {
        Vector2 randomCircle = Random.insideUnitCircle * wanderRadius;
        Vector3 randomPos = new Vector3(randomCircle.x, 0, randomCircle.y);
        wanderTarget = transform.position + randomPos;
    }

    Vector3 GetWallAvoidanceVector()
    {
        Vector3 avoidVector = Vector3.zero;
        Vector3[] directions = {
            transform.forward,
            -transform.forward,
            transform.right,
            -transform.right
        };

        foreach (Vector3 dir in directions)
        {
            if (Physics.Raycast(transform.position, dir, wallAvoidDistance, wallLayer))
            {
                avoidVector -= dir;
            }
        }

        return avoidVector;
    }
}
