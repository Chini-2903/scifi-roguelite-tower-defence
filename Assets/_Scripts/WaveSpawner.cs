using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [Header("Setup")]
    public GameObject enemyPrefab;
    public Transform spawnPoint; // Where the enemies appear

    [Header("Wave Settings")]
    public float timeBetweenWaves = 5.5f; // Wait time between waves
    private float countdown = 2f;         // Wait time before the VERY FIRST wave starts

    private int waveIndex = 1; // The current wave number

    void Update()
    {
        // If the timer hits zero, spawn a wave
        if (countdown <= 0f)
        {
            // StartCoroutine allows us to pause code execution (to put gaps between enemy spawns)
            StartCoroutine(SpawnWave());

            // Reset the timer for the next wave
            countdown = timeBetweenWaves;
        }

        if (GameManager.GameIsOver)
            return;

        // Slowly tick the timer down every frame
        countdown -= Time.deltaTime;
    }

    IEnumerator SpawnWave()
    {
        // A loop that runs 'waveIndex' number of times
        for (int i = 0; i < waveIndex; i++)
        {
            SpawnEnemy();
            // Wait for 0.5 seconds before spawning the next enemy in this wave
            yield return new WaitForSeconds(0.5f);
        }

        // After the wave finishes, increase the wave number so the next one is harder!
        waveIndex++;
    }

    void SpawnEnemy()
    {
        // Create an enemy at the spawn point exactly
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}