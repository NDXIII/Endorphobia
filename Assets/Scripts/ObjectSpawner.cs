using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectToSpawn; // Prefab to spawn
    public int gridSize = 18; // Size of the grid (18x18)
    public float cellSize = 5.0f; // Size of each grid cell
    public LayerMask wallLayer; // LayerMask to identify walls
    public int numberOfObjects = 10; // Number of objects to spawn

    private int spawnedCount = 0;

    private void Start()
    {
        SpawnObjects();
    }

    private void SpawnObjects()
    {
        while (spawnedCount < numberOfObjects)
        {
            // Generate a random grid position
            Vector3 randomPosition = GetRandomPosition();

            // Check if the position is valid (not inside a wall)
            if (IsValidPosition(randomPosition))
            {
                Instantiate(objectToSpawn, randomPosition, GetRandomRotation());
                spawnedCount++;
            }
        }
    }

    private Vector3 GetRandomPosition()
    {
        // Compute the min and max bounds for the grid
        float halfGrid = gridSize / 2.0f;
        float minX = -halfGrid * cellSize;
        float maxX = halfGrid * cellSize;

        float minZ = -halfGrid * cellSize;
        float maxZ = halfGrid * cellSize;

        // Generate random positions within these bounds
        float x = Random.Range(minX, maxX);
        float z = Random.Range(minZ, maxZ);

        // Snap position to the center of a grid cell
        x = Mathf.Floor(x / cellSize) * cellSize + cellSize / 2.0f;
        z = Mathf.Floor(z / cellSize) * cellSize + cellSize / 2.0f;



        // Randomize position within the grid cell
        x += Random.Range(-cellSize / 2.5f, cellSize / 2.5f);
        z += Random.Range(-cellSize / 2.5f, cellSize / 2.5f);

        return new Vector3(x, 0, z); // Assuming y = 0 is the floor level
    }

    private Quaternion GetRandomRotation()
    {
        return Quaternion.Euler(0, Random.Range(0, 360), 0);
    }

    private bool IsValidPosition(Vector3 position)
    {
        // Check if there's a wall at the position
        Collider[] colliders = Physics.OverlapSphere(position, 0.5f, wallLayer);
        return colliders.Length == 0;
    }

    public void SpawnAdditionalObjects(int count)
    {
        numberOfObjects += count;
        SpawnObjects();
    }

    // Can easily be called from the editor by right clicking on the ObjectSpawner script
    [ContextMenu("Spawn Additional Objects")]
    public void SpawnOneAdditionalObject()
    {
        SpawnAdditionalObjects(1);
    }
}
