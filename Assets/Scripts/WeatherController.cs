using UnityEngine;

public class WeatherController : MonoBehaviour
{
    public GameObject rainEffect;
    public GameObject snowEffect;
    public GameObject fogEffect;
    public GameObject sunEffect;  // If you want a sun effect, can be optional

    private enum WeatherState
    {
        Sunny,
        Rainy,
        Snowy,
        Foggy
    }

    private WeatherState currentWeather;

    // Weather change parameters
    public float weatherChangeInterval = 10f;  // Time in seconds to change weather
    private float weatherChangeTimer;

    void Start()
    {
        // Initialize the weather system and start with a default weather
        currentWeather = WeatherState.Sunny;
        UpdateWeatherEffects();

        // Start the timer for weather change
        weatherChangeTimer = weatherChangeInterval;
    }

    void Update()
    {
        // Countdown to the next weather change
        weatherChangeTimer -= Time.deltaTime;

        if (weatherChangeTimer <= 0)
        {
            // Reset timer
            weatherChangeTimer = weatherChangeInterval;

            // Cycle to the next weather condition
            CycleWeather();
        }
    }

    void CycleWeather()
    {
        // Change weather randomly or cyclically
        // For random weather change, use this line instead of the one below:
        // currentWeather = (WeatherState)Random.Range(0, 4);

        // For a cyclic weather pattern:
        currentWeather = (WeatherState)(((int)currentWeather + 1) % 4);

        // Update weather effects based on the current weather
        UpdateWeatherEffects();
    }

    void UpdateWeatherEffects()
    {
        // Disable all weather effects first
        rainEffect.SetActive(false);
        snowEffect.SetActive(false);
        fogEffect.SetActive(false);
        sunEffect.SetActive(false); // Optional: for sun effect

        // Enable the correct weather effect
        switch (currentWeather)
        {
            case WeatherState.Sunny:
                sunEffect.SetActive(true);  // Optional sun effect
                break;
            case WeatherState.Rainy:
                rainEffect.SetActive(true);
                break;
            case WeatherState.Snowy:
                snowEffect.SetActive(true);
                break;
            case WeatherState.Foggy:
                fogEffect.SetActive(true);
                break;
        }
    }
}

