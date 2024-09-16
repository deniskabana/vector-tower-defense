using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner main;

    [Header("References")]
    [SerializeField]
    private GameObject[] enemyPrefabs;

    [Header("Attributes")]
    [SerializeField]
    private int baseEnemies = 8;

    [SerializeField]
    private float enemiesPerSecond = 0.5f;

    [SerializeField]
    private float difficultyScalingFactor = 0.75f;

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    [Header("Wave Configurations")]
    [SerializeField]
    private WaveConfig[] waveConfigs;

    [System.Serializable]
    public struct WaveConfig
    {
        public int waveNumber;
        public GameObject[] enemyTypes;
    }

    private float timeSinceLastSpawn;
    public int enemiesAlive;
    public int enemiesLeftToSpawn;
    private bool isSpawning = false;

    public void AddCustomEnemy()
    {
        enemiesAlive++;
    }

    void Awake()
    {
        main = this;
    }

    void Start()
    {
        if (main == null)
            main = this;

        onEnemyDestroy.AddListener(EnemyDestroyCallback);
    }

    public void StartSpawning()
    {
        StartWave();
    }

    private void StartWave()
    {
        isSpawning = true;
        enemiesLeftToSpawn = EnemiesPerWave();
    }

    private void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;
        LevelManager.main.EndWave();
    }

    void Update()
    {
        if (!isSpawning)
            return;

        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= 1f / enemiesPerSecond && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();
            timeSinceLastSpawn = 0f;
        }

        if (enemiesAlive <= 0 && enemiesLeftToSpawn <= 0)
        {
            EndWave();
        }
    }

    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(
            baseEnemies * Mathf.Pow(LevelManager.main.currentWave, difficultyScalingFactor)
        );
    }

    private void SpawnEnemy()
    {
        enemiesAlive++;
        enemiesLeftToSpawn--;

        GameObject[] currentWaveEnemies = GetEnemiesForCurrentWave();
        GameObject enemyPrefab = currentWaveEnemies[Random.Range(0, currentWaveEnemies.Length)];
        Instantiate(enemyPrefab, LevelManager.main.startPoint.position, Quaternion.identity);
    }

    private GameObject[] GetEnemiesForCurrentWave()
    {
        int currentWave = LevelManager.main.currentWave;

        if (currentWave >= waveConfigs.Length)
        {
            return enemyPrefabs;
        }

        foreach (var waveConfig in waveConfigs)
        {
            if (waveConfig.waveNumber == currentWave)
            {
                return waveConfig.enemyTypes;
            }
        }

        // If no specific configuration is found for the current wave, return all enemy types
        return enemyPrefabs;
    }

    private void EnemyDestroyCallback()
    {
        enemiesAlive--;
    }
}
