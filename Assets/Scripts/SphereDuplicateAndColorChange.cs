using UnityEngine;

public class SphereDuplicateAndColorChange : MonoBehaviour
{
    public GameObject spherePrefab;  // Reference to the sphere prefab
    private Renderer sphereRenderer;

    public float spawnOffsetRange = 2f;  // The maximum distance the new sphere will be offset by

    void Start()
    {
        // Get the Renderer component to change the color of the sphere
        sphereRenderer = GetComponent<Renderer>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the sphere collides with the player object
        if (collision.gameObject.CompareTag("Player"))
        {
            // Get a random offset for the new sphere's position
            Vector3 randomOffset = new Vector3(
                Random.Range(-spawnOffsetRange, spawnOffsetRange),
                0f,  // No offset on the Y-axis to keep them level
                Random.Range(-spawnOffsetRange, spawnOffsetRange)
            );

            // Duplicate the sphere with a random offset from the original position
            Instantiate(spherePrefab, transform.position + randomOffset, transform.rotation);

            // Change the color of the current sphere to a random color
            ChangeColor();
        }
    }

    void ChangeColor()
    {
        // Generate a random color
        Color randomColor = new Color(Random.value, Random.value, Random.value);
        
        // Apply the random color to the sphere's material
        sphereRenderer.material.color = randomColor;
    }
}
