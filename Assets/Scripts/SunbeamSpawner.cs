using System.Collections;
using UnityEngine;

public class SunbeamSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject[] sunRayPrefabs;
    public DayNightCycle dNC;

    private GameObject currentSunray;

    void Start()
    {
        // Start spawning sunrays in intervals
        StartCoroutine(SpawnSunrays());
    }

    IEnumerator SpawnSunrays()
    {
        while (true)
        {
            if (!dNC.isNight)
            {
                // Destroy the current sunray if it exists
                if (currentSunray != null)
                {
                    Destroy(currentSunray);
                }

                // Choose a random spawn point and prefab
                Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                GameObject randomPrefab = sunRayPrefabs[Random.Range(0, sunRayPrefabs.Length)];

                // Instantiate the sunbeam prefab
                currentSunray = Instantiate(randomPrefab, randomSpawnPoint.position, Quaternion.identity);

                // Get the Sunbeam script from the instantiated object and start fading
                Sunbeam sunbeamScript = currentSunray.GetComponent<Sunbeam>();
                if (sunbeamScript != null)
                {
                    // Start fading in and out with specified durations
                    sunbeamScript.StartFading(0.75f, 2.5f); // Fade-in duration, display duration
                }
                else
                {
                    Debug.LogError("Sunbeam prefab does not have Sunbeam script attached.");
                }
            }

            // Wait for a while before spawning the next sunbeam
            yield return new WaitForSeconds(4.5f); // Adjust spawn interval
        }
    }
}
