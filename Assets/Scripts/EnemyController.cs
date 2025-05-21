using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieEnemy : MonoBehaviour
{
    public Transform player;
    public float attackRange = 2f;
    public float attackCooldown = 2f;
    public int attackDamage = 10;

    private NavMeshAgent agent;
    private float lastAttackTime;
    private PlayerHealth playerHealth;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        playerHealth = player.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else
        {
            agent.isStopped = true;
            Attack();
        }
    }

    void Attack()
    {
        if (Time.time - lastAttackTime > attackCooldown)
        {
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }
            else
            {
                Debug.LogWarning("PlayerHealth component not found!");
            }

            lastAttackTime = Time.time;
        }
    }
}
