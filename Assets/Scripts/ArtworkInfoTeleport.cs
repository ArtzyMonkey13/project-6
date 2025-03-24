using UnityEngine;

public class ArtworkInfoTeleport : MonoBehaviour
{
    // Designated teleportation point
    public Transform designatedTeleportPoint;

    // Reference to the player's transform (drag and drop the player object in the Inspector)
    public Transform playerTransform;

    // Flag to determine if the player is in range to teleport
    private bool isPlayerInRange = false;

    // Update method to check for teleport trigger (when T key is pressed)
    void Update()
    {
        // Check if player is in range and presses the "T" key
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("T key pressed. Triggering teleport.");
            TriggerTeleport();
        }
    }

    // Trigger the teleportation (can be called multiple times)
    public void TriggerTeleport()
    {
        TeleportPlayer(); // Call teleportation method directly
    }

    // Teleport the player to the designated teleport point
    private void TeleportPlayer()
    {
        // Check if teleport point is assigned in the Inspector
        if (designatedTeleportPoint == null)
        {
            Debug.LogError("Designated teleport point is not assigned.");
            return; // Prevent teleportation if the point is not assigned
        }

        // Check if playerTransform is assigned in the Inspector
        if (playerTransform == null)
        {
            Debug.LogError("Player transform is not assigned.");
            return;
        }

        // Teleport the player to the designated point
        playerTransform.position = designatedTeleportPoint.position;
        Debug.Log("Player teleported to: " + designatedTeleportPoint.name);
    }

    // Detect when the player enters the teleport range
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Player is in range of the teleport point.");
        }
    }

    // Detect when the player leaves the teleport range
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log("Player left the teleport range.");
        }
    }
}
