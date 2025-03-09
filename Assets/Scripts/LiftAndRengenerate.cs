using System.Collections;
using UnityEngine;

public class LiftAndRegenerate : MonoBehaviour
{
    public Transform capsule;  // The capsule to be lifted
    public float liftSpeed = 2f;  // Speed at which the capsule is lifted
    public float liftHeight = 5f;  // Height at which the capsule will be lifted
    public float regenerateDelay = 1f;  // Time before capsule regenerates after disappearing

    private Vector3 originalPosition;  // The original position where the capsule starts
    private bool isLifting = false;  // Flag to check if the capsule is being lifted

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
        if (Input.GetKey(KeyCode.E) && !isLifting)
        {
            StartCoroutine(LiftCapsule());
        }

        // If the player releases the "E" key, stop lifting and make the capsule disappear
        if (Input.GetKeyUp(KeyCode.E) && isLifting)
        {
            StopCoroutine(LiftCapsule());
            isLifting = false;
            if (capsule != null)
            {
                // Make the capsule disappear
                capsule.gameObject.SetActive(false);
                // Start regenerating the capsule at its original position after a delay
                StartCoroutine(RegenerateCapsule());
            }
        }
    }

    // Coroutine to lift the capsule
    private IEnumerator LiftCapsule()
    {
        isLifting = true;

        // Lifting the capsule upwards
        Vector3 targetPosition = new Vector3(capsule.position.x, capsule.position.y + liftHeight, capsule.position.z);

        while (capsule.position.y < targetPosition.y)
        {
            capsule.position = Vector3.MoveTowards(capsule.position, targetPosition, liftSpeed * Time.deltaTime);
            yield return null;
        }

        isLifting = false;  // Capsule is fully lifted, set isLifting to false
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
        isLifting = false;
    }
}
