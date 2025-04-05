using UnityEngine;

public class DucklingFollower : MonoBehaviour
{
    public Transform player;  // Reference to the player's transform
    public float followSpeed = 2.0f;  // Speed at which the duckling moves around the player
    public float roamRadius = 3.0f;  // The maximum distance the duckling can roam from the player
    public float roamingIntensity = 0.5f;  // How much the duckling roams around the player
    public float roamDuration = 2.0f;  // Time between changes in roaming direction
    public float minSafeDistance = 1.0f;  // The minimum distance from the player to avoid collision
    public float maxSafeDistance = 2.0f;  // The maximum distance from the player before they stop roaming too far
    public float groundCheckDistance = 1.0f;  // Distance to check for ground below the duckling

    private Rigidbody rb;  // Reference to the rigidbody to control physics
    private Vector3 currentRoamDirection;  // The direction the duckling is currently roaming
    private float roamTimer;  // Timer to control when to change roam direction

    private void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();

        // Ensure the Rigidbody is not kinematic and is affected by gravity
        if (rb != null)
        {
            rb.freezeRotation = true;  // Prevent tipping over
            rb.useGravity = true;      // Ensure gravity is enabled so ducklings fall to the ground
        }

        // Initialize roaming direction
        currentRoamDirection = Random.insideUnitSphere.normalized;

        // Ensure the duckling doesn't collide with the player using layers
        SetUpLayerCollisions();
    }

    private void Update()
    {
        // Ensure the player is assigned
        if (player == null)
        {
            Debug.LogWarning("Player is not assigned to the DucklingFollower script.");
            return;
        }

        // Add randomness to the movement by introducing a roaming effect
        roamTimer -= Time.deltaTime;
        if (roamTimer <= 0)
        {
            // Change roaming direction periodically
            currentRoamDirection = Random.insideUnitSphere.normalized;
            roamTimer = roamDuration;  // Reset the timer
        }

        // Calculate the target position around the player within the roam radius
        Vector3 targetPosition = player.position + currentRoamDirection * roamRadius;

        // Ensure the duckling doesn't get too close to the player (avoid bumping)
        Vector3 directionToPlayer = targetPosition - player.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        // If too close to the player, we move the duckling further away within the safe range
        if (distanceToPlayer < minSafeDistance)
        {
            // Move the duckling to a safer position around the player
            targetPosition = player.position + directionToPlayer.normalized * minSafeDistance;
        }
        // Otherwise, if the duckling is too far away, we limit the distance it can roam
        else if (distanceToPlayer > maxSafeDistance)
        {
            targetPosition = player.position + directionToPlayer.normalized * maxSafeDistance;
        }

        // Smoothly move the duckling towards the target position with roaming behavior
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        // Optionally, we can smooth the roaming direction by interpolating its movement
        currentRoamDirection = Vector3.Lerp(currentRoamDirection, Random.insideUnitSphere.normalized, roamingIntensity * Time.deltaTime);

        // Raycast to keep the duckling on the ground
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance))
        {
            // Ensure the duckling is grounded and prevent it from floating
            float groundHeight = hit.point.y;
            if (transform.position.y > groundHeight)
            {
                // Adjust the vertical position to the ground level
                transform.position = new Vector3(transform.position.x, groundHeight, transform.position.z);
            }
        }
    }

    // Set up layers to prevent collisions between ducklings and the player
    private void SetUpLayerCollisions()
    {
        // Create a new layer for the ducklings
        int ducklingLayer = LayerMask.NameToLayer("Duckling");
        gameObject.layer = ducklingLayer;

        // Set the player's layer to a separate layer (e.g., "Player")
        int playerLayer = LayerMask.NameToLayer("Player");
        player.gameObject.layer = playerLayer;

        // Set up collision rules so that ducklings don't collide with the player
        Physics.IgnoreLayerCollision(ducklingLayer, playerLayer, true);
    }
}
