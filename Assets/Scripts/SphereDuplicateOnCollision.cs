using UnityEngine;

public class SphereDuplicateOnCollision : MonoBehaviour
{
    // The distance the duplicate will pop up from the original sphere
    public float popDistance = 2f;
    // The prefab of the sphere to instantiate
    public GameObject spherePrefab;
    
    // When a collision happens with the player
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collider is tagged as "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // Duplicate the sphere
            Vector3 duplicatePosition = transform.position + new Vector3(popDistance, 0, 0); // Position the duplicate
            Instantiate(spherePrefab, duplicatePosition, Quaternion.identity);
            
            // Optional: You can add a sound or effect here when the sphere duplicates
            // For example: AudioSource.PlayClipAtPoint(duplicateSound, transform.position);
        }
    }
}
