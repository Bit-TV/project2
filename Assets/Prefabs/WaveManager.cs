using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Spawns enemies in waves and starts the next wave after a short break.
// Enemies are spawned gradually (pacing) instead of instantly.
public class WaveManager : MonoBehaviour
{
    [Header("Wave")]
    public int currentWave = 0;
    public int baseEnemies = 3;
    public float enemiesPerWave = 1.2f;

    [Header("Timing")]
    public float timeBetweenWaves = 2f;
    public float timeBetweenSpawns = 0.2f;

    [Header("Spawning")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    private readonly List<GameObject> aliveEnemies = new List<GameObject>();

    private bool waveInProgress = false;
    private bool spawningWave = false;      // ✅ NEW: prevents early wave-advance
    private Coroutine waveRoutine;

    public int CurrentWave => currentWave;

    private void Start()
    {
        waveRoutine = StartCoroutine(BeginNextWave());
    }

    private void Update()
    {
        aliveEnemies.RemoveAll(e => e == null);

        //  Only advance when:
        // - wave is in progress
        // - we are NOT still spawning
        // - and all tracked enemies are gone
        if (waveInProgress && !spawningWave && aliveEnemies.Count == 0)
        {
            waveInProgress = false;

            if (waveRoutine == null)
                waveRoutine = StartCoroutine(BeginNextWave());
        }
    }

    private IEnumerator BeginNextWave()
    {
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

        yield return new WaitForSeconds(timeBetweenWaves);

        currentWave++;
        int enemyCount = GetEnemyCountForWave(currentWave);

        Debug.Log($"Wave {currentWave} starting with {enemyCount} enemies.");

        waveInProgress = true;
        spawningWave = true;

        for (int i = 0; i < enemyCount; i++)
        {
            SpawnOneEnemy();
            yield return new WaitForSeconds(timeBetweenSpawns);
        }

        spawningWave = false;
        waveRoutine = null;
    }

    private void SpawnOneEnemy()
    {
        Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
        if (spawn == null)
        {
            Debug.LogError("WaveManager: A spawn point entry is missing (None).");
            return;
        }

        GameObject enemy = Instantiate(enemyPrefab, spawn.position, Quaternion.identity);
        aliveEnemies.Add(enemy);
    }

    private int GetEnemyCountForWave(int wave)
    {
        return baseEnemies + Mathf.FloorToInt(wave * enemiesPerWave);
    }
}