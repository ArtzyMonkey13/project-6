using UnityEngine;

public class EnemyBalance : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        // Get the Rigidbody component attached to the enemy
        rb = GetComponent<Rigidbody>();

        // Apply constraints to prevent tipping over
        if (rb != null)
        {
            rb.freezeRotation = true; // Disable all rotation by default
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; // Freeze rotation on X and Z axes
        }

        // Correct initial rotation to ensure the enemy is upright at the start of the game
        CorrectInitialRotation();
    }

    void Update()
    {
        // Keep the enemy upright by applying small torque on X and Z axes
        if (rb != null)
        {
            rb.AddTorque(Vector3.Cross(transform.up, Vector3.up) * 10f, ForceMode.Force);
        }
    }

    void CorrectInitialRotation()
    {
        // Ensure the enemy starts with an upright rotation (no tilt)
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
    }
}
