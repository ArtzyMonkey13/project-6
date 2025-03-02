using System.Collections;
using UnityEngine;

public class LiftAndCarry : MonoBehaviour
{
    public Transform capsule;  // The capsule to be lifted
    public float liftSpeed = 2f;  // Speed at which the capsule is lifted
    public float carryDistance = 2f;  // Distance the capsule will be carried from the player
    public float regenerateDelay = 1f;  // Time before capsule regenerates after disappearing

    private Vector3 originalPosition;  // The original position where the capsule starts
    private bool isCarrying = false;  // Flag to check if the capsule is being carried

    void Start()
    {
        // Store the original position of the capsule
        if (capsule != null)
        {
            originalPosition = capsule.position;
        }
    }

    void Update()
    {
        // Check if the player is pressing the "E" key
        if (Input.GetKey(KeyCode.E) && !isCarrying)
        {
            StartCoroutine(LiftAndCarryCapsule());
        }

        // If the player releases the "E" key, stop carrying and make the capsule disappear
        if (Input.GetKeyUp(KeyCode.E) && isCarrying)
        {
            StopCoroutine(LiftAndCarryCapsule());
            isCarrying = false;
            if (capsule != null)
            {
                // Make the capsule disappear
                capsule.gameObject.SetActive(false);
                // Start regenerating the capsule at its original position after a delay
                StartCoroutine(RegenerateCapsule());
            }
        }
    }

    // Coroutine to lift and carry the capsule
    private IEnumerator LiftAndCarryCapsule()
    {
        isCarrying = true;

        // While the player holds the "E" key, move the capsule with the player
        while (Input.GetKey(KeyCode.E))
        {
            // Update the capsule position relative to the player
            Vector3 targetPosition = transform.position + transform.forward * carryDistance + Vector3.up * 1.5f;
            capsule.position = Vector3.Lerp(capsule.position, targetPosition, liftSpeed * Time.deltaTime);

            yield return null;
        }
    }

    // Coroutine to regenerate the capsule after a delay
    private IEnumerator RegenerateCapsule()
    {
        yield return new WaitForSeconds(regenerateDelay);

        // Regenerate the capsule at the original position
        if (capsule != null)
        {
            capsule.position = originalPosition;
            capsule.gameObject.SetActive(true);
        }

        // Allow for the next lifting
        isCarrying = false;
    }
}
