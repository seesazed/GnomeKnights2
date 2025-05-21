using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightManager : MonoBehaviour
{
    public WaveManager waveManager;
    public Light directionalLight;
    public GameObject shopUI;
    public MonoBehaviour playerControlsScript;

    private int currentDay = 1;
    private int wavesCompleted = 0; // Track completed waves, not spawned zombies
    private int wavesPerNight;
    private bool isNight = true;
    private bool inShop = false;

    private readonly List<int> wavePattern = new List<int> { 3, 4, 6, 10 }; // Waves per night

    void Start()
    {
        BeginNight();
    }

    void Update()
    {
        if (isNight && waveManager.IsWaveCleared() && !waveManager.IsWaveInProgress())
        {
            wavesCompleted++;

            if (wavesCompleted >= wavesPerNight)
            {
                BeginDay();
            }
            else
            {
                waveManager.StartNextWave();
            }
        }
    }

    void BeginNight()
    {
        isNight = true;
        inShop = false;
        wavesCompleted = 0; // Reset wave counter
        wavesPerNight = GetWaveCountForDay(currentDay); // Gets waves required (3,4,6,10...)

        Debug.Log($"Night {currentDay} - Waves required: {wavesPerNight}");

        directionalLight.color = Color.black;
        shopUI.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerControlsScript != null)
            playerControlsScript.enabled = true;

        waveManager.StartNextWave(); // Start first wave
    }

    void BeginDay()
    {
        isNight = false;
        inShop = true;
        Debug.Log("Daytime! Shop is open.");

        directionalLight.color = Color.white;
        shopUI.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (playerControlsScript != null)
            playerControlsScript.enabled = false;
    }

    public void OnShopDone()
    {
        if (inShop)
        {
            currentDay++;
            BeginNight();
        }
    }

    int GetWaveCountForDay(int day)
    {
        if (day - 1 < wavePattern.Count)
            return wavePattern[day - 1]; // Use predefined wave counts
        else
            return Mathf.RoundToInt(Mathf.Pow(day, 1.5f)); // Fallback formula
    }

    public void ForceEndNight()
    {
        if (isNight)
        {
            wavesCompleted = wavesPerNight; // Force completion
            BeginDay();
        }
    }
}