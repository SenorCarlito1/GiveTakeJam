using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private GameObject rockGolemPrefab; // Monster prefab for Rock Golem
    [SerializeField] private float rockGolemSpawnChance = 0.1f; // Spawn chance for Rock Golem (10%)

    [SerializeField] private GameObject treePrefab; // Monster prefab for Tree
    [SerializeField] private float treeSpawnChance = 0.2f; // Spawn chance for Tree (20%)

    [SerializeField] private GameObject gruntPrefab; // Monster prefab for Grunt
    [SerializeField] private float gruntSpawnChance = 0.7f; // Spawn chance for Grunt (70%)

    [SerializeField] private float regularSpawnTime = 2f; // Regular spawn time in seconds
    [SerializeField] private float maxSpawnRateMultiplier = 1f; // 1 means normal spawn time, 2 means double the spawn time

    [SerializeField] private float spawnRadius = 10f; // Radius of the spawn circle

    private float currentSpawnTime;
    private float timeSinceLastSpawn = 0f;

    private void Start()
    {
        // Calculate the spawn time based on maxSpawnRateMultiplier
        currentSpawnTime = regularSpawnTime * maxSpawnRateMultiplier;
        Debug.Log($"Initial spawn rate: {currentSpawnTime} seconds");
    }

    private void Update()
    {
        AdjustSpawnChances();

        // Increment time since last spawn
        timeSinceLastSpawn += Time.deltaTime;

        // Check if enough time has passed to spawn a new monster
        if (timeSinceLastSpawn >= currentSpawnTime)
        {
            SpawnMonster();
            timeSinceLastSpawn = 0f;
        }
    }

    private void AdjustSpawnChances()
    {
        // Ensure total spawn chance always sums up to 1
        float totalSpawnChance = rockGolemSpawnChance + treeSpawnChance + gruntSpawnChance;

        // Adjust spawn chances proportionally if total is not 1
        if (totalSpawnChance != 1f)
        {
            float adjustmentFactor = 1f / totalSpawnChance;
            rockGolemSpawnChance *= adjustmentFactor;
            treeSpawnChance *= adjustmentFactor;
            gruntSpawnChance *= adjustmentFactor;
        }
    }

    private void SpawnMonster()
    {
        float spawnRoll = Random.value; // Random value between 0 and 1

        if (spawnRoll < rockGolemSpawnChance)
        {
            SpawnSingleMonster(rockGolemPrefab);
        }
        else if (spawnRoll < rockGolemSpawnChance + treeSpawnChance)
        {
            SpawnSingleMonster(treePrefab);
        }
        else
        {
            SpawnSingleMonster(gruntPrefab);
        }
    }

    private void SpawnSingleMonster(GameObject prefab)
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        Instantiate(prefab, spawnPosition, Quaternion.identity);
        Debug.Log($"Spawned {prefab.name}. Next spawn in {currentSpawnTime} seconds.");
    }

    private Vector3 GetRandomSpawnPosition()
    {
        // Generate random angle in radians
        float angle = Random.Range(0f, Mathf.PI * 2f);
        // Calculate position on circle using polar coordinates
        float x = Mathf.Cos(angle) * spawnRadius;
        float z = Mathf.Sin(angle) * spawnRadius;
        // Offset by spawner's position
        Vector3 offset = new Vector3(x, 0f, z);
        return transform.position + offset;
    }

    private void OnDrawGizmos()
    {
        // Draw wireframe circle to visualize spawn area
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
