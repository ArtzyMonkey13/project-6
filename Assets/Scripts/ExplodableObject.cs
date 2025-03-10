using UnityEngine;

public class ExplodableObject : MonoBehaviour
{
    public GameObject explosionEffect; // A prefab for the explosion effect
    public float explosionDelay = 0.5f; // Delay before the explosion happens
    public int hitPoints = 1; // How many hits it takes to destroy this object

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that hit this one is a grape bullet
        if (other.CompareTag("GrapeBullet"))
        {
            // Reduce hit points on collision
            hitPoints--;

            if (hitPoints <= 0)
            {
                Explode(); // Call the explode function when hit points reach 0
            }

            // Optionally, destroy the grape bullet after it collides
            Destroy(other.gameObject);
        }
    }

    private void Explode()
    {
        // Show explosion effect (if assigned)
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
        }

        // Destroy the object after explosion
        Destroy(gameObject);
    }
}
