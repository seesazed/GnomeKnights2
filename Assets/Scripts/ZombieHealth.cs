using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ZombieHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int startingBaseHealth = 100; // Never modified
    private int _baseHealth; // Used for calculation per wave
    private int _currentHealth;
    public int coinReward = 5;

    [Header("Visual Effects")]
    public GameObject damagePopupPrefab;
    public GameObject bloodSplatPrefab;
    public Material damageMaterial;
    public float damageFlashTime = 0.2f;

    [Header("Audio")]
    public AudioClip deathSound;
    public AudioClip hitSound;

    // Components
    private Renderer _renderer;
    private Material _originalMaterial;
    private AudioSource _audio;
    private PlayerCurrency _playerCurrency;

    void Awake()
    {
        _baseHealth = startingBaseHealth;
        _currentHealth = _baseHealth;

        _renderer = GetComponentInChildren<Renderer>();
        if (_renderer != null) _originalMaterial = _renderer.material;

        _audio = GetComponent<AudioSource>();
        _playerCurrency = FindObjectOfType<PlayerCurrency>();
    }

    public void SetWaveMultiplier(float multiplier)
    {
        _baseHealth = startingBaseHealth; // Reset to original base
        _currentHealth = Mathf.RoundToInt(_baseHealth * multiplier);
        Debug.Log($"Zombie health set to: {_currentHealth} (Base: {_baseHealth} * {multiplier})");
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        ShowDamageEffects(damage);

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void ShowDamageEffects(int damage)
    {
        if (damagePopupPrefab != null)
        {
            var popup = Instantiate(damagePopupPrefab,
                                    transform.position + Vector3.up * 1.5f,
                                    Quaternion.identity);
            popup.transform.LookAt(Camera.main.transform);
            popup.GetComponentInChildren<TMPro.TextMeshPro>().text = damage.ToString();
            Destroy(popup, 1f);
        }

        if (bloodSplatPrefab != null)
        {
            Destroy(Instantiate(bloodSplatPrefab, transform.position, Quaternion.identity), 2f);
        }

        if (_renderer != null && damageMaterial != null)
        {
            StartCoroutine(FlashDamage());
        }

        if (hitSound != null && _audio != null)
        {
            _audio.PlayOneShot(hitSound);
        }
    }

    private IEnumerator FlashDamage()
    {
        _renderer.material = damageMaterial;
        yield return new WaitForSeconds(damageFlashTime);
        _renderer.material = _originalMaterial;
    }

    private void Die()
    {
        if (_playerCurrency != null)
        {
            _playerCurrency.AddMoney(coinReward);
        }

        if (deathSound != null && _audio != null)
        {
            _audio.PlayOneShot(deathSound);
        }

        GetComponent<Collider>().enabled = false;
        if (TryGetComponent(out ZombieEnemy ai)) ai.enabled = false;

        Destroy(gameObject, deathSound != null ? deathSound.length : 0.1f);
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        startingBaseHealth = Mathf.Max(1, startingBaseHealth);
    }
#endif
}
