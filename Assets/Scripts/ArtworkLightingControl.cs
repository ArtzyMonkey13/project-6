using UnityEngine;

public class ArtworkLightingControl : MonoBehaviour
{
    // Reference to the light that shines on the artwork
    public Light artworkLight;

    // Flag to track whether the player is in range
    private bool isPlayerInRange = false;

    // Update is called once per frame
    void Update()
    {
        // If the player is in range, the light will be on, otherwise, it's off
        if (isPlayerInRange && artworkLight != null)
        {
            if (!artworkLight.enabled)  // Only enable if it's not already on
            {
                artworkLight.enabled = true;
                Debug.Log("Lighting activated for the artwork.");
            }
        }
        else
        {
            if (artworkLight != null && artworkLight.enabled)  // Only disable if it's on
            {
                artworkLight.enabled = false;
                Debug.Log("Lighting deactivated for the artwork.");
            }
        }
    }

    // Detect when the player enters the trigger zone
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Player entered the lighting trigger zone.");
        }
    }

    // Detect when the player exits the trigger zone
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log("Player exited the lighting trigger zone.");
        }
    }
}