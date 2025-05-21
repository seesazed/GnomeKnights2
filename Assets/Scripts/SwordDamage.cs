using UnityEngine;

public class SwordDamage : MonoBehaviour
{
    [Header("Sword Settings")]
    public int baseDamage = 20; // Default sword damage
    public int currentDamage; // Damage that can change with upgrades
    public bool canDamage = true; // Flag to enable/disable damage

    // For attack speed or cooldown purposes
    private float nextAttackTime = 0f;
    public float attackCooldown = 1f; // Cooldown time for attacks (in seconds)

    // Reference to the PlayerStats to get the current damage from PlayerStats
    public PlayerStats playerStats;

    void Start()
    {
        // Initialize current damage to base damage at the start
        currentDamage = baseDamage;
        if (playerStats != null)
        {
            currentDamage = playerStats.attackDamage; // Get damage from PlayerStats
        }
    }

    // Detect collisions with zombies
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie") && canDamage && Time.time >= nextAttackTime)
        {
            ZombieHealth zombieHealth = other.GetComponent<ZombieHealth>();
            if (zombieHealth != null)
            {
                zombieHealth.TakeDamage(currentDamage);
                Debug.Log("🗡️ Dealt " + currentDamage + " damage to zombie");

                // Handle attack cooldown
                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }
}
