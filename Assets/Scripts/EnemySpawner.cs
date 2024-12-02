using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject[] enemyPrefabs;
    public DayNightCycle dNC;

    public int nightEnemyCount = 20;
    public int enemiesDestroyed = 0;
    
    public int enemiesSpawned = 0;

    void Start()
    { 
        // Start spawning sunrays in intervals
        StartCoroutine(SpawnSunrays());
    }

    IEnumerator SpawnSunrays()
    {
        while (true)
        {
            if (dNC.isNight && enemiesSpawned < nightEnemyCount)
            {
                // Choose a random spawn point and prefab
                Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                GameObject randomPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

                // Instantiate the sunbeam prefab
                Instantiate(randomPrefab, randomSpawnPoint.position, Quaternion.identity);
                enemiesSpawned++;
            }

            // Wait for a while before spawning the next sunbeam
            yield return new WaitForSeconds(1f); // Adjust spawn interval
        }
    }
}
