using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Spawns enemies in waves and starts the next wave after a short break.
// Enemies are spawned gradually (pacing) instead of instantly.
public class WaveManager : MonoBehaviour
{
    [Header("Wave")]
    public int currentWave = 0;          // Starts at 0 so Wave 1 is the first wave that spawns
    public int baseEnemies = 3;          // Enemies on Wave 1
    public float enemiesPerWave = 1.2f;  // How fast enemy count grows each wave (gentler than +2 per wave)

    [Header("Timing")]
    public float timeBetweenWaves = 2f;  // Rest time after a wave ends
    public float timeBetweenSpawns = 0.2f; // Delay between each enemy spawn within a wave

    [Header("Spawning")]
    public GameObject enemyPrefab;       // Drag EnemyBasic prefab here
    public Transform[] spawnPoints;      // Drag your 4 spawn point transforms here

    // Tracks enemies spawned this wave so we know when the wave is cleared
    private readonly List<GameObject> aliveEnemies = new List<GameObject>();

    private bool waveInProgress = false;
    private Coroutine waveRoutine;

    private void Start()
    {
        // Start the first wave
        waveRoutine = StartCoroutine(BeginNextWave());
    }

    private void Update()
    {
        // Clean out destroyed enemies from the list
        aliveEnemies.RemoveAll(e => e == null);

        // If the wave is running and all enemies are gone, start the next wave
        if (waveInProgress && aliveEnemies.Count == 0)
        {
            waveInProgress = false;

            // Start next wave (only if we're not already starting one)
            if (waveRoutine == null)
                waveRoutine = StartCoroutine(BeginNextWave());
        }
    }

    private IEnumerator BeginNextWave()
    {
        // Safety checks so we don't throw errors
        if (enemyPrefab == null)
        {
            Debug.LogError("WaveManager: Enemy Prefab is not assigned.");
            waveRoutine = null;
            yield break;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("WaveManager: No spawn points assigned.");
            waveRoutine = null;
            yield break;
        }

        // Rest period between waves (pacing)
        yield return new WaitForSeconds(timeBetweenWaves);

        // Advance wave number
        currentWave++;

        // Calculate how many enemies to spawn this wave
        int enemyCount = GetEnemyCountForWave(currentWave);

        Debug.Log($"Wave {currentWave} starting with {enemyCount} enemies.");

        waveInProgress = true;

        // Spawn enemies gradually for better pacing
        for (int i = 0; i < enemyCount; i++)
        {
            SpawnOneEnemy();
            yield return new WaitForSeconds(timeBetweenSpawns);
        }

        // Done starting this wave
        waveRoutine = null;
    }

    private void SpawnOneEnemy()
    {
        // Pick a random spawn point
        Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
        if (spawn == null)
        {
            Debug.LogError("WaveManager: A spawn point entry is missing (None).");
            return;
        }

        // Spawn enemy and track it
        GameObject enemy = Instantiate(enemyPrefab, spawn.position, Quaternion.identity);
        aliveEnemies.Add(enemy);
    }

    private int GetEnemyCountForWave(int wave)
    {
        // Gentle scaling:
        // Wave 1: baseEnemies + floor(1 * enemiesPerWave)
        // Wave 10: baseEnemies + floor(10 * enemiesPerWave)
        // Tune enemiesPerWave in Inspector to control pacing.
        return baseEnemies + Mathf.FloorToInt(wave * enemiesPerWave);
    }
}