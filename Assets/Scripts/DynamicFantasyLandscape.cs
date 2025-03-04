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
    public GameObject[] treePrefabs;
    public GameObject[] boulderPrefabs; // Array for boulders
    public GameObject riverPrefab;

    // Scale and rotation ranges
    public float minTreeScale = 0.8f;
    public float maxTreeScale = 1.5f;
    public float minBoulderScale = 0.5f;
    public float maxBoulderScale = 2.0f;
    public float maxRotation = 360f;

    // Overlap check radius
    public float objectRadius = 3f; // The radius within which we check for overlaps (e.g., 3 units)

    void Awake()
    {
        if (terrain == null)
            terrain = GetComponent<Terrain>();

        // Generate the terrain on awake
        GenerateTerrain();
    }

    void Start()
    {
        // Start the tree and boulder placement after a slight delay to ensure terrain is set
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

        PlaceTrees();
        PlaceBoulders(); // Place boulders after the terrain is generated
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
                    // Get world position based on terrain height
                    Vector3 position = new Vector3(x, terrain.SampleHeight(new Vector3(x, 0, z)), z);
                    if (position.y > 5f && position.y < heightMultiplier - 5f) // Only place trees within height limits
                    {
                        // Check if the position is clear of other objects
                        if (!IsPositionOccupied(position))
                        {
                            // Pick a random tree prefab
                            GameObject treePrefab = treePrefabs[Random.Range(0, treePrefabs.Length)];

                            // Randomize scale while preserving proportions
                            float treeScale = Random.Range(minTreeScale, maxTreeScale);
                            Vector3 originalScale = treePrefab.transform.localScale;
                            Vector3 randomScale = new Vector3(originalScale.x * treeScale, originalScale.y * treeScale, originalScale.z * treeScale);

                            Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0, maxRotation), 0); // Rotate around the Y-axis

                            GameObject tree = Instantiate(treePrefab, position, randomRotation);
                            tree.transform.localScale = randomScale; // Apply random scale
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
                    // Get world position based on terrain height
                    Vector3 position = new Vector3(x, terrain.SampleHeight(new Vector3(x, 0, z)), z);
                    if (position.y > 2f && position.y < heightMultiplier - 2f) // Only place boulders within reasonable height limits
                    {
                        // Check if the position is clear of other objects
                        if (!IsPositionOccupied(position))
                        {
                            // Pick a random boulder prefab
                            GameObject boulderPrefab = boulderPrefabs[Random.Range(0, boulderPrefabs.Length)];

                            // Randomize scale while preserving proportions
                            float boulderScale = Random.Range(minBoulderScale, maxBoulderScale);
                            Vector3 originalScale = boulderPrefab.transform.localScale;
                            Vector3 randomScale = new Vector3(originalScale.x * boulderScale, originalScale.y * boulderScale, originalScale.z * boulderScale);

                            Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0, maxRotation), 0); // Rotate around the Y-axis

                            GameObject boulder = Instantiate(boulderPrefab, position, randomRotation);
                            boulder.transform.localScale = randomScale; // Apply random scale
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
        Vector3 riverStart = new Vector3(0, terrain.SampleHeight(Vector3.zero), 0);
        Vector3 riverEnd = new Vector3(width, terrain.SampleHeight(new Vector3(width, 0, 0)), length);

        // Instantiate a river prefab along the path (You can create or download a river asset)
        if (riverPrefab != null)
        {
            Instantiate(riverPrefab, riverStart, Quaternion.identity);
        }
    }
}
