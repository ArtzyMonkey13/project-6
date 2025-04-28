using UnityEngine;
using System.Collections;

public class ArtworkInfoTeleport1 : MonoBehaviour
{
    public Transform designatedTeleportPoint;
    public Transform playerTransform;

    private bool isPlayerInRange = false;
    private bool isTeleporting = false;
    private bool localTeleportCooldown = false;

    void Start()
    {
        // Auto-assign playerTransform if not manually set
        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                playerTransform = playerObj.transform;
        }

        Debug.Log($"[{gameObject.name}] Initialized with designatedTeleportPoint: {designatedTeleportPoint?.name}, playerTransform: {playerTransform?.name}");
    }

    void Update()
    {
        if (isPlayerInRange && !isTeleporting && !localTeleportCooldown && Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log($"[{gameObject.name}] T key pressed while player is in range. Triggering teleport.");
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
            Debug.LogError($"[{gameObject.name}] Designated teleport point is not assigned.");
            return;
        }

        if (playerTransform == null)
        {
            Debug.LogError($"[{gameObject.name}] Player transform is not assigned.");
            return;
        }

        Vector3 targetPosition = designatedTeleportPoint.position + Vector3.up * 0.5f;

        Debug.Log($"[{gameObject.name}] Teleporting '{playerTransform.name}' from {playerTransform.position} to {targetPosition}");

        CharacterController controller = playerTransform.GetComponent<CharacterController>();
        if (controller != null)
        {
            controller.enabled = false;
            playerTransform.position = targetPosition;
            controller.enabled = true;
            Debug.Log($"[{gameObject.name}] Teleported using CharacterController.");
            return;
        }

        Rigidbody rb = playerTransform.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.MovePosition(targetPosition);
            Debug.Log($"[{gameObject.name}] Teleported using Rigidbody.");
            return;
        }

        playerTransform.position = targetPosition;
        Debug.Log($"[{gameObject.name}] Teleported using direct transform.position.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log($"[{gameObject.name}] Player entered teleport range.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log($"[{gameObject.name}] Player left teleport range.");
        }
    }

    private IEnumerator TeleportCooldown()
    {
        isTeleporting = true;
        localTeleportCooldown = true;

        Collider thisCollider = GetComponent<Collider>();
        if (thisCollider != null)
        {
            thisCollider.enabled = false;
            Debug.Log($"[{gameObject.name}] Disabling trigger collider for cooldown.");
        }

        yield return new WaitForSeconds(1.0f); // You can make this a public variable if needed

        if (thisCollider != null)
        {
            thisCollider.enabled = true;
            Debug.Log($"[{gameObject.name}] Re-enabled trigger collider.");
        }

        isTeleporting = false;
        localTeleportCooldown = false;
    }
}
