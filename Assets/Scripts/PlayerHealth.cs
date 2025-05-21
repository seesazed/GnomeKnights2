using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] public int maxHealth = 100;
    public int currentHealth;

    [Header("UI References")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private GameObject deathScreen;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
        if (deathScreen != null) deathScreen.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        UpdateHealthUI();

        if (currentHealth <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        if (isDead) return;

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UpdateHealthUI();
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthSlider == null) return;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("Player has died!");

        if (deathScreen != null)
            deathScreen.SetActive(true);

        // Pause the game
        Time.timeScale = 0f;

        // Unlock cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Disable control scripts
        var movement = GetComponent<PlayerMovement>();
        if (movement != null) movement.enabled = false;
        var attack = GetComponentInChildren<PlayerAttack>();
        if (attack != null) attack.enabled = false;
    }

    /// <summary>
    /// Call this from your Restart button OnClick().
    /// </summary>
    public void RestartGame()
    {
        // Unpause
        Time.timeScale = 1f;
        // Reload current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
}
