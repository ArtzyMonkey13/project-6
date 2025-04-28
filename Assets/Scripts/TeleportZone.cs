using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class TeleportZone : MonoBehaviour
{
    public Transform designatedTeleportPoint;
    public float cooldownTime = 1f;
    private bool isPlayerInRange = false;

    private Transform playerTransform;

    void Start()
    {
        // Make sure the collider is a trigger
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;

        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                playerTransform = playerObj.transform;
        }

        Debug.Log($"[{gameObject.name}] Linked to: {designatedTeleportPoint?.name}");
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.T))
        {
            if (TeleportationManager.Instance != null && TeleportationManager.Instance.CanTeleport(playerTransform))
            {
                TeleportPlayer();
            }
        }
    }

    private void TeleportPlayer()
    {
        if (designatedTeleportPoint == null || playerTransform == null)
        {
            Debug.LogWarning($"[{gameObject.name}] Missing references.");
            return;
        }

        Vector3 targetPosition = designatedTeleportPoint.position + Vector3.up * 0.5f;

        Debug.Log($"[{gameObject.name}] Teleporting {playerTransform.name} to {targetPosition}");

        CharacterController controller = playerTransform.GetComponent<CharacterController>();
        if (controller != null)
        {
            controller.enabled = false;
            playerTransform.position = targetPosition;
            controller.enabled = true;
        }
        else if (playerTransform.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.linearVelocity = Vector3.zero;
            rb.MovePosition(targetPosition);
        }
        else
        {
            playerTransform.position = targetPosition;
        }

        // Register teleport to prevent immediate re-entry
        TeleportationManager.Instance.RegisterTeleport(playerTransform, cooldownTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
}
