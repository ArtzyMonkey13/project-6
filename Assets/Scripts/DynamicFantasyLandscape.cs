using UnityEngine;
using System.Collections;

public class DynamicFantasyLandscape : MonoBehaviour
{
    public Terrain terrain;
    public float heightMultiplier = 10f;
    public float width = 512;
    public float length = 512;
    public float scale = 20f;
    public float treeDensity = 0.05f; // The higher, the more trees
    public float boulderDensity = 0.02f; // The higher, the more boulders
    public float flowerDensity = 0.03f; // Density for flowers
    public float mushroomDensity = 0.03f; // Density for mushrooms

    public GameObject[] treePrefabs;
    public GameObject[] boulderPrefabs; // Array for boulders
    public GameObject[] flowerPrefabs; // Array for flowers
    public GameObject[] mushroomPrefabs; // Array for mushrooms
    public GameObject riverPrefab;

    // Scale and rotation ranges
    public float minTreeScale = 0.8f;
    public float maxTreeScale = 1.5f;
    public float minBoulderScale = 0.5f;
    public float maxBoulderScale = 2.0f;
    public float minFlowerScale = 0.5f;
    public float maxFlowerScale = 1.0f;
    public float minMushroomScale = 0.3f;
    public float maxMushroomScale = 0.8f;
    public float maxRotation = 360f;

    // Overlap check radius
    public float objectRadius = 1.5f; // The radius within which we check for overlaps (e.g., 3 units)

    void Awake()
    {
        if (terrain == null)
            terrain = GetComponent<Terrain>();

        // Generate the terrain on awake
        GenerateTerrain();
    }

    void Start()
    {
        // Start the object placement after a slight delay to ensure terrain is set
        StartCoroutine(DelayedObjectPlacement());
        AddRiver();
    }

    void GenerateTerrain()
    {
        // Get the terrain data and set dimensions
        TerrainData terrainData = terrain.terrainData;
        terrainData.heightmapResolution = (int)width + 1;
        terrainData.SetHeights(0, 0, GenerateHeightMap());

        terrainData.size = new Vector3(width, heightMultiplier, length);
    }

    float[,] GenerateHeightMap()
    {
        // Perlin noise generation for the heightmap
        float[,] heights = new float[(int)width, (int)length];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < length; z++)
            {
                float xCoord = (float)x / width * scale;
                float zCoord = (float)z / length * scale;

                float height = Mathf.PerlinNoise(xCoord, zCoord);
                heights[x, z] = height;
            }
        }
        return heights;
    }

    IEnumerator DelayedObjectPlacement()
    {
        // Wait until the next frame to ensure the terrain is set up first
        yield return null;

        // Place all objects after the terrain is generated
        PlaceTrees();
        PlaceBoulders();
        PlaceFlowers();
        PlaceMushrooms();
    }

    void PlaceTrees()
    {
        // Randomly place trees on the terrain
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < length; z++)
            {
                if (Random.value < treeDensity)
                {
                    Vector3 position = new Vector3(x, terrain.SampleHeight(new Vector3(x, 0, z)), z);
                    if (position.y > 2f && position.y < heightMultiplier - 2f) // Only place trees within height limits
                    {
                        if (!IsPositionOccupied(position))
                        {
                            GameObject treePrefab = treePrefabs[Random.Range(0, treePrefabs.Length)];
                            float treeScale = Random.Range(minTreeScale, maxTreeScale);
                            Vector3 randomScale = new Vector3(treeScale, treeScale, treeScale);

                            Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0, maxRotation), 0);
                            GameObject tree = Instantiate(treePrefab, position, randomRotation);
                            tree.transform.localScale = randomScale;
                        }
                    }
                }
            }
        }
    }

    void PlaceBoulders()
    {
        // Randomly place boulders on the terrain
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < length; z++)
            {
                if (Random.value < boulderDensity)
                {
                    Vector3 position = new Vector3(x, terrain.SampleHeight(new Vector3(x, 0, z)), z);
                    if (position.y > 2f && position.y < heightMultiplier - 2f) // Only place boulders within reasonable height limits
                    {
                        if (!IsPositionOccupied(position))
                        {
                            GameObject boulderPrefab = boulderPrefabs[Random.Range(0, boulderPrefabs.Length)];
                            float boulderScale = Random.Range(minBoulderScale, maxBoulderScale);
                            Vector3 randomScale = new Vector3(boulderScale, boulderScale, boulderScale);

                            Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0, maxRotation), 0);
                            GameObject boulder = Instantiate(boulderPrefab, position, randomRotation);
                            boulder.transform.localScale = randomScale;
                        }
                    }
                }
            }
        }
    }

    void PlaceFlowers()
    {
        // Randomly place flowers on the terrain
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < length; z++)
            {
                if (Random.value < flowerDensity)
                {
                    Vector3 position = new Vector3(x, terrain.SampleHeight(new Vector3(x, 0, z)), z);
                    if (position.y > 2f && position.y < heightMultiplier - 2f) // Only place flowers within reasonable height limits
                    {
                        if (!IsPositionOccupied(position))
                        {
                            GameObject flowerPrefab = flowerPrefabs[Random.Range(0, flowerPrefabs.Length)];
                            float flowerScale = Random.Range(minFlowerScale, maxFlowerScale);
                            Vector3 randomScale = new Vector3(flowerScale, flowerScale, flowerScale);

                            Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0, maxRotation), 0);
                            GameObject flower = Instantiate(flowerPrefab, position, randomRotation);
                            flower.transform.localScale = randomScale;
                        }
                    }
                }
            }
        }
    }

    void PlaceMushrooms()
    {
        // Randomly place mushrooms on the terrain
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < length; z++)
            {
                if (Random.value < mushroomDensity)
                {
                    Vector3 position = new Vector3(x, terrain.SampleHeight(new Vector3(x, 0, z)), z);
                    if (position.y > 2f && position.y < heightMultiplier - 2f) // Only place mushrooms within reasonable height limits
                    {
                        if (!IsPositionOccupied(position))
                        {
                            GameObject mushroomPrefab = mushroomPrefabs[Random.Range(0, mushroomPrefabs.Length)];
                            float mushroomScale = Random.Range(minMushroomScale, maxMushroomScale);
                            Vector3 randomScale = new Vector3(mushroomScale, mushroomScale, mushroomScale);

                            Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0, maxRotation), 0);
                            GameObject mushroom = Instantiate(mushroomPrefab, position, randomRotation);
                            mushroom.transform.localScale = randomScale;
                        }
                    }
                }
            }
        }
    }

    bool IsPositionOccupied(Vector3 position)
    {
        // Check if any object is within the defined radius of the position
        Collider[] colliders = Physics.OverlapSphere(position, objectRadius);
        foreach (var collider in colliders)
        {
            // Check if the collider is not the terrain itself (or any other object type you want to ignore)
            if (collider.gameObject.CompareTag("Terrain") == false)
            {
                return true; // There's an object in the way, so we shouldn't place an object here
            }
        }
        return false; // No objects are occupying the space
    }

    void AddRiver()
{
    // Example of adding a river (a simple placeholder object)
    // You can improve this by sculpting the terrain and placing actual river meshes

    // Define a buffer area near the edges where you won't place the pond
    float buffer = 10f; // This will keep the pond away from the extreme edges
    float pondWidth = 30f; // Width of the pond
    float pondLength = 30f; // Length of the pond

    // Randomly choose a position for the pond, but make sure it doesn't go off the edges
    float xPos = Random.Range(buffer, width - buffer - pondWidth);
    float zPos = Random.Range(buffer, length - buffer - pondLength);

    // Sample height at the chosen position for the pond's starting point
    float yPos = terrain.SampleHeight(new Vector3(xPos, 0, zPos));

    // Adjust for terrain height if necessary (this keeps the pond level with the terrain)
    Vector3 pondPosition = new Vector3(xPos, yPos, zPos);

    // Instantiate the river/pond at the determined position
    if (riverPrefab != null)
    {
        Instantiate(riverPrefab, pondPosition, Quaternion.identity);
    }
}

}
