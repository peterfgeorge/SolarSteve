using UnityEngine;
using TMPro;
using System.Collections;
using System;

public class DayNightCycle : MonoBehaviour
{
    public TextMeshProUGUI phaseText;  // For "Morning", "Midday", etc.
    public TextMeshProUGUI dayText;    // For "Day 1", "Day 2", etc.
    
    private float dayLength = 60f;     // Total day length in seconds
    private float phaseLength;         // Length of each phase (15 seconds)
    private float currentTime = 0f;    // Time elapsed in the current day
    private int currentDay = 1;        // Start at Day 1
    
    public bool isNight = false;      // Track if it is currently night
    private bool nightFinished = false; // Track if night enemies are defeated

    public DayNightPostProcessing postProcessingController;


    public Transform[] spawnPoints;
    public GameObject[] sunRayPrefabs;
    private EnemySpawner enemySpawner;


    void Start()
    {
        phaseLength = dayLength / 4;  // Divide day into 4 phases
        UpdateDayText();
        UpdatePhaseText("Morning");

        enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    void Update()
    {
        if (nightFinished)
        {
            StartNewDay();
        }
        else if (!isNight)
        {
            RunDayCycle();
        }
        if(enemySpawner.nightEnemyCount == enemySpawner.enemiesDestroyed){
            Debug.Log("Night finished!!");
            OnNightFinished();
        }
    }

    void RunDayCycle()
    {
        currentTime += Time.deltaTime;

        if (currentTime < phaseLength)
        {
            UpdatePhaseText("Morning");
        }
        else if (currentTime < phaseLength * 2)
        {
            UpdatePhaseText("Midday");
        }
        else if (currentTime < phaseLength * 3)
        {
            UpdatePhaseText("Afternoon");
        }
        else if (currentTime < dayLength)
        {
            UpdatePhaseText("Sunset");
        }
        else
        {
            StartNight();
        }
    }

    void StartNight()
    {
        currentTime = 0f;  // Reset time for night phase
        isNight = true;    // Set isNight to true to stop day cycle
        UpdatePhaseText("Night");

        if (postProcessingController != null)
        {
            postProcessingController.SetNight(true);
        }        
    }

    //  IEnumerator WaitAndPrint()
    // {
    //     Debug.Log("Start waiting...");
    //     yield return new WaitForSeconds(60);  // Wait for 5 seconds
    //     Debug.Log("5 seconds passed!");
    //     Debug.Log("starting new day!");
    //     StartNewDay();
    //     postProcessingController.SetNight(false);
    // }

    void StartNewDay()
    {
        nightFinished = false;
        isNight = false;   // Reset to day cycle
        currentDay++;
        UpdateDayText();
        currentTime = 0f;  // Reset day timer
    }

    void UpdateDayText()
    {
        dayText.text = "Day " + currentDay;
    }

    void UpdatePhaseText(string phase)
    {
        phaseText.text = phase;
    }

    public void OnNightFinished()  // Call this when all night enemies are defeated
    {
        nightFinished = true;
        StartNewDay();
        postProcessingController.SetNight(false);
        enemySpawner.enemiesDestroyed = 0;
        enemySpawner.enemiesSpawned = 0;
        enemySpawner.nightEnemyCount = (int)(enemySpawner.nightEnemyCount * 1.4);
        Debug.Log("Will spawn " + enemySpawner.nightEnemyCount + " next night");
    }
}
