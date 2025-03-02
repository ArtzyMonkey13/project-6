using UnityEngine;

public class StabilizePlayer : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        // Make sure the Rigidbody has no initial angular velocity or velocity at the start
        rb.linearVelocity = Vector3.zero;            // Stop any linear velocity (movement)
        rb.angularVelocity = Vector3.zero;     // Stop any angular velocity (rotation)
    }
}
