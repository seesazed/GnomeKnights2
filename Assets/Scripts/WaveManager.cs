using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;

public class WaveManager : MonoBehaviour
{
    [Header("Zombie Spawning")]
    public GameObject zombiePrefab;
    public Transform[] spawnPoints;
    public int baseZombiesPerWave = 5;
    public float spawnDelay = 0.5f;

    [Header("Wave Scaling")]
    public float zombieCountMultiplier = 1.5f;
    public float waveHealthBonus = 0.05f; // Additive health increase per wave (5%)
    public float waveDamageBonus = 0.03f; // Additive damage increase per wave (3%)
    public float waveSpeedBonus = 0.02f;

    [Header("Wave Rewards")]
    public int baseWaveCompletionReward = 50;
    public float waveRewardMultiplier = 1.2f;

    [Header("UI Elements")]
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI zombiesRemainingText;
    public GameObject waveCompletePanel;
    public TextMeshProUGUI waveCompleteRewardText;

    [Header("References")]
    public PlayerCurrency playerCurrency;
    public float timeBetweenWaves = 10f;

    private bool waveInProgress = false;
    private List<GameObject> activeZombies = new List<GameObject>();
    public int currentWave = 1;
    private int totalZombiesInWave;
    private int remainingZombiesInWave;

    void Start()
    {
        UpdateWaveUI();
        if (waveCompletePanel != null)
            waveCompletePanel.SetActive(false);

        if (playerCurrency == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerCurrency = player.GetComponent<PlayerCurrency>();
            }
        }
    }

    void Update()
    {
        activeZombies.RemoveAll(zombie => zombie == null);
    }

    public bool IsWaveInProgress()
    {
        return waveInProgress;
    }

    public bool IsWaveCleared()
    {
        activeZombies.RemoveAll(z => z == null);
        remainingZombiesInWave = activeZombies.Count;
        UpdateZombiesRemainingUI();
        return activeZombies.Count == 0;
    }

    public void StartNextWave()
    {
        if (waveCompletePanel != null)
            waveCompletePanel.SetActive(false);

        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        waveInProgress = true;
        UpdateWaveUI();

        // Additive scaling formulas
        float healthMultiplier = 1 + (waveHealthBonus * (currentWave - 1));
        float damageMultiplier = 1 + (waveDamageBonus * (currentWave - 1));
        float speedMultiplier = 1 + (waveSpeedBonus * (currentWave - 1));

        int zombiesToSpawn = Mathf.RoundToInt(baseZombiesPerWave * Mathf.Pow(zombieCountMultiplier, currentWave - 1));
        totalZombiesInWave = zombiesToSpawn;
        remainingZombiesInWave = zombiesToSpawn;
        UpdateZombiesRemainingUI();

        for (int i = 0; i < zombiesToSpawn; i++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject zombie = Instantiate(zombiePrefab, spawnPoint.position, spawnPoint.rotation);

            ZombieHealth zombieHealth = zombie.GetComponent<ZombieHealth>();
            if (zombieHealth != null)
            {
                zombieHealth.SetWaveMultiplier(healthMultiplier);
            }

            ZombieEnemy zombieEnemy = zombie.GetComponent<ZombieEnemy>();
            if (zombieEnemy != null)
            {
                zombieEnemy.attackDamage = Mathf.RoundToInt(zombieEnemy.attackDamage * damageMultiplier);
                zombieEnemy.GetComponent<NavMeshAgent>().speed *= speedMultiplier;
            }

            activeZombies.Add(zombie);
            yield return new WaitForSeconds(spawnDelay);
        }

        yield return new WaitUntil(() => IsWaveCleared());

        int waveReward = Mathf.RoundToInt(baseWaveCompletionReward * Mathf.Pow(waveRewardMultiplier, currentWave - 1));
        if (playerCurrency != null)
        {
            playerCurrency.AddMoney(waveReward);
        }

        if (waveCompletePanel != null)
        {
            waveCompletePanel.SetActive(true);
            if (waveCompleteRewardText != null)
                waveCompleteRewardText.text = $"+{waveReward} coins!";
        }

        waveInProgress = false;
        currentWave++;
        UpdateWaveUI();

        yield return new WaitForSeconds(timeBetweenWaves);

        if (!waveInProgress && waveCompletePanel != null && waveCompletePanel.activeSelf)
        {
            waveCompletePanel.SetActive(false);
            StartNextWave();
        }
    }

    private void UpdateWaveUI()
    {
        if (waveText != null)
        {
            waveText.text = $"Wave: {currentWave}";
        }
    }

    private void UpdateZombiesRemainingUI()
    {
        if (zombiesRemainingText != null)
        {
            zombiesRemainingText.text = $"Zombies: {remainingZombiesInWave}/{totalZombiesInWave}";
        }
    }

    // New method to start multiple waves after delay
    public void StartWavesAfterShopClose(float delaySeconds, int wavesToStart)
    {
        StartCoroutine(StartMultipleWaves(delaySeconds, wavesToStart));
    }

    private IEnumerator StartMultipleWaves(float delaySeconds, int wavesToStart)
    {
        yield return new WaitForSeconds(delaySeconds);

        for (int i = 0; i < wavesToStart; i++)
        {
            if (!waveInProgress)
            {
                StartNextWave();
                yield return new WaitUntil(() => IsWaveCleared());
            }
            else
            {
                yield return new WaitUntil(() => !waveInProgress);
            }
        }
    }
}
