using UnityEngine;
using System.Collections;

public class ArtworkInfoTeleport : MonoBehaviour
{
    public Transform designatedTeleportPoint;
    public Transform playerTransform;

    private bool isPlayerInRange = false;
    private bool isTeleporting = false;

    void Update()
    {
        if (isPlayerInRange && !isTeleporting && Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log($"T key pressed while inside trigger: {gameObject.name}. Triggering teleport.");
            TriggerTeleport();
            StartCoroutine(TeleportCooldown());
        }
    }

    public void TriggerTeleport()
    {
        TeleportPlayer();
    }

    private void TeleportPlayer()
    {
        if (designatedTeleportPoint == null)
        {
            Debug.LogError("Designated teleport point is not assigned.");
            return;
        }

        if (playerTransform == null)
        {
            Debug.LogError("Player transform is not assigned.");
            return;
        }

        Vector3 targetPosition = designatedTeleportPoint.position + Vector3.up * 0.5f;

        Debug.Log($"Teleporting '{playerTransform.name}' from {playerTransform.position} to {targetPosition} via {gameObject.name}");

        // Try CharacterController teleport
        CharacterController controller = playerTransform.GetComponent<CharacterController>();
        if (controller != null)
        {
            controller.enabled = false;
            playerTransform.position = targetPosition;
            controller.enabled = true;

            Debug.Log("Teleported using CharacterController.");
            return;
        }

        // Try Rigidbody teleport
        Rigidbody rb = playerTransform.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.MovePosition(targetPosition);

            Debug.Log("Teleported using Rigidbody.");
            return;
        }

        // Fallback: direct position set
        playerTransform.position = targetPosition;
        Debug.Log("Teleported using direct transform.position.");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"OnTriggerEnter triggered by: {other.name} on {gameObject.name}");

        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Player is in range of the teleport point.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"OnTriggerExit triggered by: {other.name} on {gameObject.name}");

        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log("Player left the teleport range.");
        }
    }

    private IEnumerator TeleportCooldown()
    {
        isTeleporting = true;

        Collider thisCollider = GetComponent<Collider>();
        if (thisCollider != null)
        {
            thisCollider.enabled = false;
            Debug.Log($"Disabling trigger collider on {gameObject.name} for cooldown.");
        }

        yield return new WaitForSeconds(1.0f);

        if (thisCollider != null)
        {
            thisCollider.enabled = true;
            Debug.Log($"Re-enabled trigger collider on {gameObject.name}.");
        }

        isTeleporting = false;
    }
}
