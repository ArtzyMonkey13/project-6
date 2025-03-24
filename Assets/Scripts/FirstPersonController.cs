using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform PlayerCamera; // Reference to the player's camera
    public float LookSensitivityX = 2.0f; // Mouse sensitivity for X (horizontal)
    public float LookSensitivityY = 2.0f; // Mouse sensitivity for Y (vertical)
    public float MinYLookAngle = -60f; // Minimum vertical angle (up/down)
    public float MaxYLookAngle = 60f; // Maximum vertical angle (up/down)

    private float verticalRotation = 0f; // Current vertical rotation of the camera

    public float WalkSpeed = 5.0f; // Player movement speed
    public float SprintMultiplier = 1.5f; // Sprint multiplier when Shift is held
    public float JumpForce = 10f; // Jump force
    public float Gravity = -9.81f; // Gravity force
    private Vector3 velocity = Vector3.zero; // Player velocity
    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>(); // Get the CharacterController component
    }

    void Update()
    {
        // Handle player movement
        HandleMovement();

        // Handle camera rotation (looking around)
        HandleCameraRotation();
    }

    private void HandleMovement()
    {
        // Get input for movement (WASD or arrow keys)
        float horizontalMovement = Input.GetAxis("Horizontal"); // A/D or Left/Right arrows
        float verticalMovement = Input.GetAxis("Vertical"); // W/S or Up/Down arrows

        // Move the player based on input (forward, backward, left, right)
        Vector3 moveDirection = transform.forward * verticalMovement + transform.right * horizontalMovement;
        moveDirection.Normalize(); // Ensure the movement vector has a magnitude of 1

        // Set speed, apply sprint if Shift is held
        float speed = WalkSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed *= SprintMultiplier;
        }

        // Ground check and jumping
        if (characterController.isGrounded)
        {
            if (velocity.y < 0)
            {
                velocity.y = -2f; // Prevent floating above the ground
            }

            // Jumping when the J key is pressed
            if (Input.GetKeyDown(KeyCode.J))
            {
                velocity.y = Mathf.Sqrt(JumpForce * -2f * Gravity); // Jump force calculation
            }
        }

        // Apply gravity
        velocity.y += Gravity * Time.deltaTime;

        // Move the character
        characterController.Move((moveDirection * speed + velocity) * Time.deltaTime);
    }

    private void HandleCameraRotation()
    {
        if (PlayerCamera != null)
        {
            // Get mouse movement
            float mouseX = Input.GetAxis("Mouse X") * LookSensitivityX; // Horizontal (yaw) rotation
            float mouseY = Input.GetAxis("Mouse Y") * LookSensitivityY; // Vertical (pitch) rotation

            // Apply vertical rotation (clamped)
            verticalRotation -= mouseY;
            verticalRotation = Mathf.Clamp(verticalRotation, MinYLookAngle, MaxYLookAngle);

            // Rotate the camera based on vertical rotation
            PlayerCamera.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

            // Rotate the player (body) based on horizontal mouse movement
            transform.Rotate(Vector3.up * mouseX); // Rotate the player around Y-axis (left/right)
        }
    }
}
