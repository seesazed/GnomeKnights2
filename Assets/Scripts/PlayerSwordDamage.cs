using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public bool canDamage = false;
    public float attackCooldown = 1f;
    private float nextAttackTime = 0f;
    public PlayerStats playerStats;

    void Start()
    {
        if (playerStats == null)
        {
            playerStats = GetComponentInParent<PlayerStats>();
            if (playerStats == null)
            {
                Debug.LogError("⚠️ PlayerStats reference is missing on PlayerAttack!");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canDamage || Time.time < nextAttackTime || playerStats == null) return;

        if (other.CompareTag("Zombie"))
        {
            ZombieHealth zombieHealth = other.GetComponent<ZombieHealth>();
            if (zombieHealth != null)
            {
                zombieHealth.TakeDamage(playerStats.attackDamage);
                nextAttackTime = Time.time + playerStats.attackCooldown;
            }
        }
    }

    public void EnableDamage()
    {
        canDamage = true;
    }

    public void DisableDamage()
    {
        canDamage = false;
    }
}