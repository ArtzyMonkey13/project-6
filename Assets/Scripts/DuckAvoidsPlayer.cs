using UnityEngine;
using UnityEngine.AI;
public class DuckAvoidsPlayer : MonoBehaviour
{
    public Transform player;
    public float detectionRadius = 5f;
    public float safeDistance = 8f;
    public float wanderRadius = 10f;
    public float wanderTimer = 5f;

    private UnityEngine.AI.NavMeshAgent agent;
    private float timer;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        timer = wanderTimer;
    }

    void Update()
    {
        timer += Time.deltaTime;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // If too close to player, move away
        if (distanceToPlayer < detectionRadius)
        {
            Vector3 directionAway = (transform.position - player.position).normalized;
            Vector3 fleeTarget = transform.position + directionAway * safeDistance;

            UnityEngine.AI.NavMeshHit hit;
            if (UnityEngine.AI.NavMesh.SamplePosition(fleeTarget, out hit, 2f, UnityEngine.AI.NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
                timer = 0; // reset wander timer after fleeing
            }
        }
        // Otherwise, wander randomly
        else if (timer >= wanderTimer && !agent.pathPending)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        UnityEngine.AI.NavMeshHit navHit;
        UnityEngine.AI.NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }
}