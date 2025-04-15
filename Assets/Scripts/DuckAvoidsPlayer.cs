using UnityEngine;
using UnityEngine.AI;

public class DuckAvoidsPlayer : MonoBehaviour
{
    public Transform player;
    public float detectionRadius = 5f;
    public float safeDistance = 8f;
    public float wanderRadius = 10f;
    public float wanderTimer = 5f;

    private NavMeshAgent agent;
    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
                Debug.Log("Player found by tag.");
            }
            else
            {
                Debug.LogWarning("Player reference not set and could not be found by tag!");
            }
        }

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component missing from NPC!");
        }
    }

    void Update()
    {
        if (player == null || agent == null) return;

        timer += Time.deltaTime;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        Debug.Log($"Distance to player: {distanceToPlayer:F2}");

        if (distanceToPlayer < detectionRadius)
        {
            Debug.Log("Player too close! Attempting to flee...");

            Vector3 directionAway = (transform.position - player.position).normalized;
            Vector3 fleeTarget = transform.position + directionAway * safeDistance;

            if (NavMesh.SamplePosition(fleeTarget, out NavMeshHit hit, 10f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
                Debug.Log($"Fleeing to position: {hit.position}");
                timer = 0;
            }
            else
            {
                Debug.LogWarning("Failed to find valid flee destination on NavMesh.");
            }
        }
        else if (timer >= wanderTimer && !agent.pathPending)
        {
            Vector3 wanderTarget = RandomNavSphere(transform.position, wanderRadius, NavMesh.AllAreas);
            agent.SetDestination(wanderTarget);
            Debug.Log($"Wandering to new position: {wanderTarget}");
            timer = 0;
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;
        randomDirection += origin;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit navHit, distance, layermask))
        {
            return navHit.position;
        }

        Debug.LogWarning("RandomNavSphere failed to find valid position. Returning origin.");
        return origin;
    }
}
