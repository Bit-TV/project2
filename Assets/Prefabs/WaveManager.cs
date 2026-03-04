using System.Collections.Generic;
using UnityEngine;

// Spawns enemies in waves and detects when a wave is cleared.
public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public int currentWave = 1;
    public int baseEnemies = 3;         // Enemies in wave 1
    public int enemiesPerWave = 2;      // Extra enemies each wave

    [Header("Spawning")]
    public GameObject enemyPrefab;      // Drag your EnemyBasic prefab here
    public Transform[] spawnPoints;     // Drag your Spawn_Top/Bottom/Left/Right here

    // Track spawned enemies so we know when the wave is cleared
    private readonly List<GameObject> aliveEnemies = new List<GameObject>();

    void Start()
    {
        StartWave();
    }

    void Update()
    {
        // Remove enemies that were destroyed
        aliveEnemies.RemoveAll(e => e == null);

        // If none remain, wave is complete
        if (aliveEnemies.Count == 0)
        {
            // For now: automatically start next wave after a short delay
            // Will replace this later with a Continue/Quit UI prompt.
            Invoke(nameof(NextWave), 1f);
        }
    }

    void StartWave()
    {
        int enemyCount = baseEnemies + (currentWave - 1) * enemiesPerWave;

        for (int i = 0; i < enemyCount; i++)
        {
            Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];

            GameObject enemy = Instantiate(enemyPrefab, spawn.position, Quaternion.identity);
            aliveEnemies.Add(enemy);
        }

        Debug.Log("Wave " + currentWave + " started with " + enemyCount + " enemies.");
    }

    void NextWave()
    {
        // Prevent double-calling if Invoke stacks
        if (aliveEnemies.Count > 0) return;

        currentWave++;
        StartWave();
    }
}
