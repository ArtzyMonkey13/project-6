using UnityEngine;

public class DuckAvoidsPlayerWithoutNavMesh : MonoBehaviour
{
    public Transform player;
    public float detectionRadius = 5f;
    public float moveSpeed = 3f;
    public float wanderRadius = 5f;
    public float wanderInterval = 3f;

    public Vector3 wanderTarget;
    private float wanderTimer;
    private Vector3 moveDirection;

    void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
            else
                Debug.LogWarning("Player not assigned and could not be found by tag.");
        }

        // Pick a new wander target at the start
        PickNewWanderTarget();
    }

    void Update()
    {
        if (player == null) return;

        // Distance from the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Flee if the player is close enough
        if (distanceToPlayer < detectionRadius)
        {
            FleeFromPlayer();
        }
        else
        {
            Wander();
        }

        // Move the NPC in the desired direction
        if (moveDirection.sqrMagnitude > 0.01f) // Only move if there's a direction
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            // Smoothly rotate to face the move direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    void FleeFromPlayer()
    {
        // Calculate direction away from the player
        Vector3 fleeDirection = transform.position - player.position;
        moveDirection = fleeDirection.normalized;
    }

    void Wander()
    {
        wanderTimer += Time.deltaTime;

        // If enough time has passed or we're too close to the target, pick a new wander target
        if (wanderTimer >= wanderInterval || Vector3.Distance(transform.position, wanderTarget) < 1f)
        {
            PickNewWanderTarget();
            wanderTimer = 0f;
        }

        // Move toward the new wander target
        Vector3 direction = wanderTarget - transform.position;
        moveDirection = direction.normalized;
    }

    void PickNewWanderTarget()
    {
        // Pick a random position within the wander radius
        Vector2 randomOffset = Random.insideUnitCircle * wanderRadius;
        wanderTarget = transform.position + new Vector3(randomOffset.x, 0, randomOffset.y);
    }
}

