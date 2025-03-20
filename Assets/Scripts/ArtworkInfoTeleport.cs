using UnityEngine;

public class ArtworkInfoTeleport : MonoBehaviour
{
    // Empty GameObjects for teleportation points
    public Transform teleportPoint1;
    public Transform teleportPoint2;

    // Flag to determine if teleportation should occur
    private bool isTeleporting = false;

    // Update method to check for teleport trigger
    void Update()
    {
        if (isTeleporting)
        {
            TeleportPlayer();
            isTeleporting = false; // Reset flag after teleportation
        }
    }

    // Call this method to trigger teleportation
    public void TriggerTeleport()
    {
        isTeleporting = true;
    }

    // Teleport the player to a new location
    private void TeleportPlayer()
    {
        // Randomly decide which point to teleport to (1 or 2)
        Transform targetPoint = Random.Range(0, 2) == 0 ? teleportPoint1 : teleportPoint2;

        // Teleport the player to the selected point
        if (targetPoint != null)
        {
            transform.position = targetPoint.position;
            Debug.Log("Player teleported to: " + targetPoint.name);
        }
        else
        {
            Debug.LogError("Teleport destination not set.");
        }
    }
}
