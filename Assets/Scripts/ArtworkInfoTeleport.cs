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
            Debug.Log("T key pressed. Triggering teleport.");
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

        playerTransform.position = designatedTeleportPoint.position;
        Debug.Log("Player teleported to: " + designatedTeleportPoint.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Player is in range of the teleport point.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log("Player left the teleport range.");
        }
    }

    private IEnumerator TeleportCooldown()
    {
        isTeleporting = true;
        yield return new WaitForSeconds(0.5f);
        isTeleporting = false;
    }
}
