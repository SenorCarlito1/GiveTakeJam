using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private GameObject rockGolemPrefab;
    [SerializeField] private float rockGolemSpawnChance = 0.0f;

    [SerializeField] private GameObject treePrefab;
    [SerializeField] private float treeSpawnChance = 0.0f;

    [SerializeField] private GameObject gruntPrefab;
    [SerializeField] private float gruntSpawnChance = 1.0f;

    [SerializeField] private float regularSpawnTime = 2f;
    [SerializeField] private float maxSpawnRateMultiplier = 1f;

    [SerializeField] private float spawnRadius = 10f;

    private float currentSpawnTime;
    private float timeSinceLastSpawn = 0f;

    private int previousWoodCount; // Store the previous wood count to detect changes
    private int previousStoneCount;

    private void Start()
    {
        // Calculate the initial spawn time based on maxSpawnRateMultiplier
        UpdateCurrentSpawnTime();
        Debug.Log($"Initial spawn rate: {currentSpawnTime} seconds");

        // Initialize previousWoodCount to current woodCount
        previousWoodCount = GameManager.instance.woodCount;
        previousStoneCount = GameManager.instance.stoneCount;
    }

    private void Update()
    {
        AdjustSpawnChances();

        // Check for changes in woodCount
        if (GameManager.instance.woodCount != previousWoodCount)
        {
            // Update treeSpawnChance based on woodCount change
            UpdateTreeSpawnChance();
            previousWoodCount = GameManager.instance.woodCount; // Update previousWoodCount
        }
        if (GameManager.instance.stoneCount != previousStoneCount)
        {
            UpdateRockGolemSpawnChance();
            previousStoneCount = GameManager.instance.stoneCount;
        }

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
        float totalSpawnChance = rockGolemSpawnChance + treeSpawnChance + gruntSpawnChance;

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
        float spawnRoll = Random.value;

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

        // Update current spawn time after each spawn
        UpdateCurrentSpawnTime();
    }

    private void SpawnSingleMonster(GameObject prefab)
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        Instantiate(prefab, spawnPosition, Quaternion.identity);
        Debug.Log($"Spawned {prefab.name}. Next spawn in {currentSpawnTime} seconds.");
    }

    private void UpdateCurrentSpawnTime()
    {
        currentSpawnTime = regularSpawnTime * maxSpawnRateMultiplier;
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
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }

    private void UpdateTreeSpawnChance()
    {
        treeSpawnChance += 0.25f * (GameManager.instance.woodCount - previousWoodCount);
        treeSpawnChance = Mathf.Clamp01(treeSpawnChance);

        Debug.Log($"Updated treeSpawnChance: {treeSpawnChance}");
    }

    private void UpdateRockGolemSpawnChance()
    {
        rockGolemSpawnChance += 0.25f * (GameManager.instance.stoneCount - previousStoneCount);
        rockGolemSpawnChance = Mathf.Clamp01(rockGolemSpawnChance);

        Debug.Log($"Updated rockGolemSpawnChance: {rockGolemSpawnChance}");
    }
}
