using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;              // Reference to the player’s position
    public float chaseSpeed = 5f;         // Normal chasing speed
    public float slowDownSpeed = 2f;      // Speed when the enemy slows down
    public float stopDistance = 2f;       // Distance at which the enemy stops chasing
    public float slowDownDistance = 5f;   // Distance at which the enemy begins to slow down
    public float rotationSpeed = 5f;      // Rotation speed for smooth turning
    public float slowDownDuration = 3f;   // Time the enemy stays slowed down after being hit

    private float currentSpeed;           // The current speed of the enemy
    private bool isSlowed = false;        // Flag to check if the enemy is slowed down
    private float slowDownTimer = 0f;     // Timer to control the slow-down effect duration

    private void Start()
    {
        currentSpeed = chaseSpeed;        // Initialize current speed as normal chase speed
    }

    private void Update()
    {
        // If the enemy is slowed down, update the slow-down timer
        if (isSlowed)
        {
            slowDownTimer -= Time.deltaTime;
            if (slowDownTimer <= 0f)
            {
                isSlowed = false;
                currentSpeed = chaseSpeed; // Reset to normal speed after slow-down effect ends
            }
        }

        // Calculate distance between enemy and player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // If the player is within stop distance, the enemy should stop chasing
        if (distanceToPlayer < stopDistance)
        {
            return; // Stop moving if too close
        }

        // If the player is within slow down distance, the enemy should slow down
        if (distanceToPlayer < slowDownDistance)
        {
            MoveTowardsPlayer(slowDownSpeed); // Slow down the movement
        }
        else
        {
            MoveTowardsPlayer(currentSpeed); // Move normally
        }
    }

    private void MoveTowardsPlayer(float speed)
    {
        // Calculate the direction from the enemy to the player
        Vector3 direction = (player.position - transform.position).normalized;

        // Rotate the enemy smoothly towards the player
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Move the enemy towards the player at the selected speed
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    // Method called when the enemy is shot
    public void OnShot()
    {
        // This method is called when the enemy is shot by the player
        if (!isSlowed)
        {
            isSlowed = true;
            slowDownTimer = slowDownDuration; // Reset the timer
            currentSpeed = slowDownSpeed;     // Slow down the enemy's speed
            Debug.Log("Enemy slowed down after being hit!");
        }
    }
}
