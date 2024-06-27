using UnityEngine;
using UnityEngine.AI;

public class GruntEnemy : MonoBehaviour
{
    [Header("---Patrol Settings---")]
    [SerializeField] private float walkPointRange = 5f;

    [Header("---Detection Settings---")]
    [SerializeField] private float sightRange = 10f;
    [SerializeField] private float fieldOfView = 110f;

    [Header("---Chase Settings---")]
    [SerializeField] private float chaseStoppingDistance = 2f;
    [SerializeField] private float minDistanceToPlayer = 1.5f;
    [SerializeField] private float runSpeed = 6f; // Speed when chasing the player
    [SerializeField] private float walkSpeed = 3.5f; // Speed when patrolling

    [Header("---Behavior Settings---")]
    [SerializeField] private float memoryDuration = 3f;
    [SerializeField] private float idleDuration = 2f;
    [SerializeField] private GameObject player; // Changed to GameObject

    [Header("---Attack Settings---")]
    [SerializeField] private float punchDistance = 2f; // Distance at which the enemy will attack
    [SerializeField] private float damageAmount = 20f; // Amount of damage to apply on attack

    private Vector3 walkPoint;
    private bool walkPointSet;
    private NavMeshAgent navAgent;
    private bool isChasing;
    private Vector3 lastKnownPlayerPosition;
    private float memoryTimer;
    private float idleTimer;
    private bool isIdle;
    private Animator animator;
    private bool hasAttacked; // Flag to track if attack has been performed

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player"); // Find player GameObject
        SetNewWalkPoint();
        hasAttacked = false;
    }

    void Update()
    {
        if (CanSeePlayer())
        {
            lastKnownPlayerPosition = player.transform.position;
            memoryTimer = memoryDuration;
            ChasePlayer();
        }
        else
        {
            if (isChasing)
            {
                if (memoryTimer > 0)
                {
                    navAgent.SetDestination(lastKnownPlayerPosition);
                    memoryTimer -= Time.deltaTime;
                }
                else
                {
                    isChasing = false;
                    GoIdle();
                }
            }
            else
            {
                if (isIdle)
                {
                    Idle();
                }
                else
                {
                    Patrol();
                }
            }
        }

        animator.SetFloat("Speed", navAgent.velocity.magnitude);

        // Trigger attack if within punch distance and hasn't attacked yet
        if (Vector3.Distance(transform.position, player.transform.position) <= punchDistance && !hasAttacked)
        {
            animator.SetTrigger("Attack");
            hasAttacked = true; // Set flag to true to prevent repeated attacks
        }
    }

    private bool CanSeePlayer()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < sightRange)
        {
            Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
            float angleBetweenEnemyAndPlayer = Vector3.Angle(transform.forward, directionToPlayer);

            if (angleBetweenEnemyAndPlayer < fieldOfView / 2)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, directionToPlayer, out hit, sightRange))
                {
                    if (hit.transform == player.transform)
                    {
                        lastKnownPlayerPosition = player.transform.position; // Update last known position
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private void ChasePlayer()
    {
        isChasing = true;
        isIdle = false;
        navAgent.speed = runSpeed; // Increase speed when chasing
        animator.SetBool("isRunning", true); // Switch to run animation
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        Vector3 targetPosition = player.transform.position - directionToPlayer * minDistanceToPlayer;
        navAgent.SetDestination(targetPosition);

        if (Vector3.Distance(transform.position, player.transform.position) <= chaseStoppingDistance)
        {
            Debug.Log("Player too close! Implement attack or game over logic.");
        }
    }

    private void Patrol()
    {
        navAgent.speed = walkSpeed; // Normal speed when patrolling
        animator.SetBool("isRunning", false); // Switch to walk animation
        if (walkPointSet)
        {
            navAgent.SetDestination(walkPoint);

            if (navAgent.remainingDistance <= navAgent.stoppingDistance && !navAgent.pathPending)
            {
                walkPointSet = false;
                GoIdle();
            }
        }
        else
        {
            SetNewWalkPoint();
        }
    }

    private void GoIdle()
    {
        isIdle = true;
        idleTimer = idleDuration;
    }

    private void Idle()
    {
        if (idleTimer > 0)
        {
            idleTimer -= Time.deltaTime;
        }
        else
        {
            isIdle = false;
            SetNewWalkPoint();
        }
    }

    private void SetNewWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f))
        {
            walkPointSet = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, walkPointRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        if (isChasing && memoryTimer > 0)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, lastKnownPlayerPosition);
        }
    }

    // Method to deal damage to the player, called by animation event
    public void DealDamageToPlayer()
    {
        if (IsPlayerInFront())
        {
            Debug.Log("Enemy dealing damage to player.");
            player.GetComponent<PlayerHealth>().TakeDamage(damageAmount);
        }
        else
        {
            Debug.Log("Player not in front, no damage dealt.");
        }
    }

    public void TakeDamageAnimation()
    {
        // Trigger your damage animation using the Animator component
        animator.SetTrigger("TakeDamage");
    }
    public void DeathAnimation()
    {
        animator.SetTrigger("Die");
    }

    // Reset attack flag, called by animation event or timer
    public void ResetAttackFlag()
    {
        hasAttacked = false;
    }

    private bool IsPlayerInFront()
    {
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        float angleBetweenEnemyAndPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        return angleBetweenEnemyAndPlayer < fieldOfView / 2;
    }
}
