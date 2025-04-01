using UnityEngine;

public class DucklingFollower : MonoBehaviour
{
    public Transform player;  // Reference to the player's transform
    public float followDistance = 2.0f;  // Desired distance behind the player
    public float followSpeed = 2.0f;  // Speed at which the duckling follows the player

    private void Update()
    {
        // Ensure the player is assigned
        if (player == null)
        {
            Debug.LogWarning("Player is not assigned to the DucklingFollower script.");
            return;
        }

        // Calculate the direction to follow the player from behind
        Vector3 targetPosition = player.position - player.forward * followDistance;

        // Move the duckling towards the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }
}
