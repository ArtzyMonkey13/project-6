using UnityEngine;

public class AddScoreObject : MonoBehaviour
{
    public ScoreManager scoreManager;  // Reference to the ScoreManager

    // This function will be called when the player touches the object
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))  // Check if the player collided with this object
        {
            scoreManager.AddScore(10);  // Add 10 points
            Destroy(gameObject);  // Optionally destroy the object after scoring
        }
    }
}
