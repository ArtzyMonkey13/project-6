using UnityEngine;

public class WeatherController : MonoBehaviour
{
    public GameObject rainEffect;
    public GameObject bubbleEffect;
    public GameObject player;

    private bool isInRainyZone = false;
    private bool isInSunnyZone = false;

    public Transform pondPosition; // Assign the pond's transform position in the Inspector
    public float bubbleRadius = 5f; // Radius around the pond for bubbles

    void Start()
    {
        // Initially, we assume no weather effect
        rainEffect.SetActive(false);
        bubbleEffect.SetActive(false);
    }

    void Update()
    {
        // Check player’s position and update weather effects accordingly
        UpdateWeatherBasedOnPlayer();
    }

    void UpdateWeatherBasedOnPlayer()
    {
        Vector3 playerPosition = player.transform.position;

        // Check if the player is within the rainy zone
        if (isInRainyZone)
        {
            // Enable rain effect
            rainEffect.SetActive(true);
        }
        else
        {
            // Disable rain effect if player is outside the rainy zone
            rainEffect.SetActive(false);
        }

        // Check if the player is within the sunny zone
        if (isInSunnyZone)
        {
            // Ensure the rain is off
            rainEffect.SetActive(false);
        }

        // Check for pond and activate bubble effect
        if (Vector3.Distance(playerPosition, pondPosition.position) < bubbleRadius)
        {
            // Enable the bubble effect if the player is near the pond
            bubbleEffect.SetActive(true);
        }
        else
        {
            bubbleEffect.SetActive(false);
        }
    }

    // Trigger detection for entering rainy or sunny zones
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RainyZone"))
        {
            isInRainyZone = true;
        }

        if (other.CompareTag("SunnyZone"))
        {
            isInSunnyZone = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("RainyZone"))
        {
            isInRainyZone = false;
        }

        if (other.CompareTag("SunnyZone"))
        {
            isInSunnyZone = false;
        }
    }
}
