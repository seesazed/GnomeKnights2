using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Player Stats")]
    public int maxHealth = 100;
    public int currentHealth;
    public int attackDamage = 40;
    public float attackRange = 1.5f;
    public float attackSpeed = 1f; // The speed at which the attack animation plays
    public float attackCooldown = 1f; // Attack cooldown in seconds
    public float moveSpeed = 5f; // Player's movement speed

    // Other player stats can be added as needed

    void Start()
    {
        // Initialize player health
        currentHealth = maxHealth;
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth); // Ensure health doesn't exceed maxHealth
    }

    public void IncreaseAttackDamage(int amount)
    {
        attackDamage += amount;
    }

    public void IncreaseAttackSpeed(float amount)
    {
        attackSpeed += amount;
    }

    public void IncreaseAttackCooldown(float amount)
    {
        attackCooldown -= amount;  // Lower the cooldown (faster attacks)
    }

    public void IncreaseMoveSpeed(float amount)
    {
        moveSpeed += amount;
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }
}
