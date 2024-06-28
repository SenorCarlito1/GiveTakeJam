using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour, IDamage
{
    [Header("----Enemy Health----")]
    [SerializeField] public float maxHealth;
    public float currHealth;
    private float origHealth;

    private bool isDead;


    private void Start()
    {
        origHealth = maxHealth;
        currHealth = maxHealth;
    }

    public void TakeDamage(float dmg)
    {
        if (isDead)
            return;

        currHealth -= dmg;

        if (currHealth <= 0)
        {
            Die();
        }
        else
        {
            EnemyPatrol rockGolem = GetComponent<EnemyPatrol>();
            if (rockGolem != null)
            {
                rockGolem.TakeDamageAnimation();
            }
            GruntEnemy grunt = GetComponent<GruntEnemy>();
            if (grunt != null)
            {
                grunt.TakeDamageAnimation();
            }
            // Make sure to put in audio to play for getting hurt
            // Make sure to play animation or flash red for getting hurt
        }
    }

    private void Die()
    {
        isDead = true;

        // Stop enemy movement and attacks
        NavMeshAgent navAgent = GetComponent<NavMeshAgent>();
        if (navAgent != null)
        {
            navAgent.enabled = false;
        }

        // Optionally, disable other components like attack scripts
        GruntEnemy grunt = GetComponent<GruntEnemy>();
        if (grunt != null)
        {
            grunt.enabled = false;
            grunt.DeathAnimation();
        }

        EnemyPatrol rockGolem = GetComponent<EnemyPatrol>();
        if (rockGolem != null)
        {
            rockGolem.enabled = false;
            rockGolem.DeathAnimation();
        }

        Destroy(gameObject, 5f);
    }
}
