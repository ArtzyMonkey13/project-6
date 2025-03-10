using UnityEngine;

public class FloatingCube : MonoBehaviour
{
    public float hoverHeight = 3f; // How high the cube floats
    public float hoverSpeed = 2f;  // Speed at which the cube hovers
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Ensure gravity is off
    }

    void FixedUpdate()
    {
        Vector3 targetPosition = new Vector3(transform.position.x, hoverHeight, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, hoverSpeed * Time.deltaTime);
    }
}
