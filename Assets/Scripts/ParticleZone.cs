using UnityEngine;

public class ParticleZone : MonoBehaviour
{
    public ParticleSystem particleEffect; // Reference to the particle system
    public string playerTag = "Player";   // Tag of the player, change if needed

    private void Start()
    {
        if (particleEffect == null)
        {
            Debug.LogError("Particle effect is not assigned!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the trigger zone
        if (other.CompareTag(playerTag))
        {
            ActivateParticleEffect();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the player leaves the trigger zone
        if (other.CompareTag(playerTag))
        {
            DeactivateParticleEffect();
        }
    }

    // Activates the particle system
    private void ActivateParticleEffect()
    {
        if (particleEffect != null && !particleEffect.isPlaying)
        {
            particleEffect.Play();
        }
    }

    // Deactivates the particle system
    private void DeactivateParticleEffect()
    {
        if (particleEffect != null && particleEffect.isPlaying)
        {
            particleEffect.Stop();
        }
    }
}
