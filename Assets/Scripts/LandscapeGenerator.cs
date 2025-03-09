using UnityEngine;
using System.Collections.Generic;

public class LandscapeGenerator : MonoBehaviour
{
    public GameObject planePrefab;        // The plane object to act as the ground
    public GameObject treePrefab;         // A prefab for trees (You can create a simple tree using cubes or a more detailed model)
    public GameObject rockPrefab;         // A prefab for rocks (could be a simple cube or a custom 3D model)
    public GameObject duckPondPrefab;     // A prefab for the duck pond (could be a sphere or custom model)

    public int treeCount = 5;             // Number of trees
    public int rockCount = 3;             // Number of rocks
    public float terrainSize = 20f;       // Size of the terrain (scale)
    public float minObjectSpacing = 2f;   // Minimum distance between objects
    public float centralRegionSize = 10f; // The size of the central region where objects will be placed
    public int maxPlacementAttempts = 100; // Maximum number of attempts to place an object before giving up

    private List<Vector3> treePositions = new List<Vector3>(); // Keeps track of all tree positions
    private List<Vector3> rockPositions = new List<Vector3>(); // Keeps track of all rock positions

    private void Start()
    {
        GenerateLandscape();
    }

    private void GenerateLandscape()
    {
        // Create the plane as the ground
        GameObject plane = Instantiate(planePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        plane.transform.localScale = new Vector3(terrainSize, 1, terrainSize); // Scale it to create a large flat area

        // Create the duck pond (use a sphere for simplicity, adjust scale and position)
        GameObject pond = Instantiate(duckPondPrefab, new Vector3(5, 0.1f, 5), Quaternion.identity);
        pond.transform.localScale = new Vector3(3f, 0.1f, 3f); // Flatten the pond to make it appear like a small body of water

        // Generate rocks within the central region
        for (int i = 0; i < rockCount; i++)
        {
            Vector3 rockPosition = GetRandomPositionInCentralRegion();
            int attempts = 0;

            // Try to find a valid position for the rock
            while ((IsPositionOccupied(rockPosition) || IsTreeNearRock(rockPosition)) && attempts < maxPlacementAttempts)
            {
                rockPosition = GetRandomPositionInCentralRegion();
                attempts++;
            }

            // Only place the rock if a valid position was found
            if (attempts < maxPlacementAttempts)
            {
                // Vary the size of the rocks
                float rockSize = Random.Range(1f, 3f); // Random scale for rocks
                GameObject rock = Instantiate(rockPrefab, rockPosition, Quaternion.identity);
                rock.transform.localScale = new Vector3(rockSize, rockSize, rockSize);
                rockPositions.Add(rockPosition); // Add rock position to the list
            }
        }

        // Generate trees within the central region
        for (int i = 0; i < treeCount; i++)
        {
            Vector3 treePosition = GetRandomPositionInCentralRegion();
            int attempts = 0;

            // Try to find a valid position for the tree
            while ((IsPositionOccupied(treePosition) || IsRockNearTree(treePosition)) && attempts < maxPlacementAttempts)
            {
                treePosition = GetRandomPositionInCentralRegion();
                attempts++;
            }

            // Only place the tree if a valid position was found
            if (attempts < maxPlacementAttempts)
            {
                // Vary the size of the trees
                float treeSize = Random.Range(0.5f, 2f); // Random scale for trees
                GameObject tree = Instantiate(treePrefab, treePosition, Quaternion.identity);
                tree.transform.localScale = new Vector3(treeSize, treeSize, treeSize);
                treePositions.Add(treePosition); // Add tree position to the list
            }
        }
    }

    // Get a random position within the defined central region
    private Vector3 GetRandomPositionInCentralRegion()
    {
        float xPos = Random.Range(-centralRegionSize / 2, centralRegionSize / 2);
        float zPos = Random.Range(-centralRegionSize / 2, centralRegionSize / 2);
        return new Vector3(xPos, 0, zPos);
    }

    // Check if the position is already occupied by another object (tree or rock)
    private bool IsPositionOccupied(Vector3 newPosition)
    {
        foreach (Vector3 pos in treePositions)
        {
            if (Vector3.Distance(pos, newPosition) < minObjectSpacing) // If too close to another tree
            {
                return true; // Occupied
            }
        }
        foreach (Vector3 pos in rockPositions)
        {
            if (Vector3.Distance(pos, newPosition) < minObjectSpacing) // If too close to a rock
            {
                return true; // Occupied
            }
        }
        return false; // Free
    }

    // Ensure no tree is too close to a rock (based on rock's size and minimum spacing)
    private bool IsRockNearTree(Vector3 treePosition)
    {
        foreach (Vector3 rockPosition in rockPositions)
        {
            float rockRadius = 1.5f; // Estimated radius of rocks (adjust this based on your rock size)
            if (Vector3.Distance(treePosition, rockPosition) < (rockRadius + minObjectSpacing))
            {
                return true; // A tree is too close to a rock
            }
        }
        return false; // No rocks near the tree
    }

    // Ensure no rock is too close to a tree (based on tree's size and minimum spacing)
    private bool IsTreeNearRock(Vector3 rockPosition)
    {
        foreach (Vector3 treePosition in treePositions)
        {
            float treeRadius = 1f; // Estimated radius of trees (adjust this based on your tree size)
            if (Vector3.Distance(rockPosition, treePosition) < (treeRadius + minObjectSpacing))
            {
                return true; // A rock is too close to a tree
            }
        }
        return false; // No trees near the rock
    }
}
