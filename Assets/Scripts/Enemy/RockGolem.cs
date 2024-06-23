using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    [Header("---Patrol Settings---")]
    [SerializeField] private float walkPointRange = 5f;

    [Header("---Detection Settings---")]
    [SerializeField] private float sightRange = 10f;
    [SerializeField] private float fieldOfView = 110f;

    [Header("---Chase Settings---")]
    [SerializeField] private float chaseStoppingDistance = 2f;
    [SerializeField] private float minDistanceToPlayer = 1.5f;

    [Header("---Behavior Settings---")]
    [SerializeField] private float memoryDuration = 3f;
    [SerializeField] private float idleDuration = 2f;
    [SerializeField] private Transform player;

    [Header("---Attack Settings---")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private float throwTriggerDistance = 10f;
    [SerializeField] private float punchDistance = 2f; // Adjust as needed
    [SerializeField] private float punchDamageAmount = 20f; // Amount of damage to apply on punch
    [SerializeField] private float punchKnockbackForce = 20f; // Amount of damage to apply on punch

    private Vector3 walkPoint;
    private bool walkPointSet;
    private NavMeshAgent navAgent;
    private bool isChasing;
    private Vector3 lastKnownPlayerPosition;
    private float memoryTimer;
    private float idleTimer;
    private bool isIdle;
    private Animator animator;
    private EnemyHealth enemyHealth;

    private bool hasThrownProjectile;
    private float throwTimer;
    private bool isThrowing;

    // Player position history
    private List<Vector3> playerPositions = new List<Vector3>();
    private List<float> positionTimestamps = new List<float>();

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
        SetNewWalkPoint();
    }

    void Update()
    {
        if (enemyHealth.currHealth <= 0)
        {
            return;
        }

        // Update player position history
        UpdatePlayerPositionHistory();

        if (CanSeePlayer())
        {
            lastKnownPlayerPosition = player.position;
            memoryTimer = memoryDuration;

            if (isChasing && Vector3.Distance(transform.position, player.position) > throwTriggerDistance)
            {
                if (!isThrowing && !hasThrownProjectile && IsPlayerInThrowArc())
                {
                    isThrowing = true;
                    animator.SetTrigger("ThrowBoulder");
                }
            }

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

        // Check punch distance and trigger punch animation
        if (Vector3.Distance(transform.position, player.position) <= punchDistance)
        {
            animator.SetTrigger("Punch");
        }

        if (throwTimer > 0)
        {
            throwTimer -= Time.deltaTime;
            if (throwTimer <= 0)
            {
                hasThrownProjectile = false;
            }
        }
    }

    private bool CanSeePlayer()
    {
        if (Vector3.Distance(transform.position, player.position) < sightRange)
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            float angleBetweenEnemyAndPlayer = Vector3.Angle(transform.forward, directionToPlayer);

            if (angleBetweenEnemyAndPlayer < fieldOfView / 2)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, directionToPlayer, out hit, sightRange))
                {
                    if (hit.transform == player)
                    {
                        lastKnownPlayerPosition = player.position; // Update last known position
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private bool IsPlayerInThrowArc()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angleBetweenEnemyAndPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        return angleBetweenEnemyAndPlayer <= 45f; // 90 degrees arc
    }

    private bool IsTargetWithinFOV(Vector3 targetPosition)
    {
        Vector3 directionToTarget = (targetPosition - transform.position).normalized;
        float angleBetweenEnemyAndTarget = Vector3.Angle(transform.forward, directionToTarget);
        return angleBetweenEnemyAndTarget < fieldOfView / 2;
    }

    private void ChasePlayer()
    {
        isChasing = true;
        isIdle = false;
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Vector3 targetPosition = player.position - directionToPlayer * minDistanceToPlayer;
        navAgent.SetDestination(targetPosition);

        if (Vector3.Distance(transform.position, player.position) <= chaseStoppingDistance)
        {
            Debug.Log("Player too close! Implement attack or game over logic.");
        }
    }

    private void Patrol()
    {
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

    public void SpawnAndThrowProjectile()
    {
        Transform targetTransform = player;
        Vector3 targetPosition = player.position;

        // Determine if the player is within sight
        if (!CanSeePlayer())
        {
            targetPosition = GetPositionOneSecondAgo();
        }

        // Check if the target position is within the FOV
        if (!IsTargetWithinFOV(targetPosition))
        {
            isThrowing = false;
            return; // Do not throw the projectile if the target is out of FOV
        }

        // Create an empty GameObject at the target position if player is not visible
        if (targetTransform == null)
        {
            GameObject lastKnownPositionMarker = new GameObject("LastKnownPositionMarker");
            lastKnownPositionMarker.transform.position = targetPosition;
            targetTransform = lastKnownPositionMarker.transform;

            // Optionally, you can destroy this GameObject after a short duration to clean up
            Destroy(lastKnownPositionMarker, 5f); // Adjust the duration as needed
        }

        Vector3 directionToTarget = (targetTransform.position - projectileSpawnPoint.position).normalized;

        // Spawn projectile at the projectileSpawnPoint
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

        // Apply force to the projectile
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(directionToTarget * throwForce, ForceMode.Impulse);
        }

        // Set the target of the ProjectileController to the determined transform
        ProjectileController controller = projectile.GetComponent<ProjectileController>();
        if (controller != null)
        {
            controller.target = targetTransform;
        }

        // Reset throw flags
        isThrowing = false;
        hasThrownProjectile = true;
        throwTimer = 1f; // Cooldown timer before the next throw can happen
    }

    private void UpdatePlayerPositionHistory()
    {
        playerPositions.Add(player.position);
        positionTimestamps.Add(Time.time);

        // Remove positions older than 5 seconds to keep the list manageable
        while (positionTimestamps.Count > 0 && Time.time - positionTimestamps[0] > 5f)
        {
            playerPositions.RemoveAt(0);
            positionTimestamps.RemoveAt(0);
        }
    }

    private Vector3 GetPositionOneSecondAgo()
    {
        float targetTime = Time.time - 1f;

        for (int i = positionTimestamps.Count - 1; i >= 0; i--)
        {
            if (positionTimestamps[i] <= targetTime)
            {
                return playerPositions[i];
            }
        }

        return lastKnownPlayerPosition; // Fallback if no position is found
    }

    // Called by animation event for punch
    public void PerformPunch()
    {
        if (Vector3.Distance(transform.position, player.position) <= punchDistance)
        {
            // Apply damage to the player
            player.GetComponent<PlayerHealth>().TakeDamage(punchDamageAmount);

            // Apply knockback to the player
            Vector3 knockbackDirection = (player.position - transform.position).normalized;
            player.GetComponent<PlayerHealth>().TakeKnockback(knockbackDirection, punchKnockbackForce);
        }
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("ThrowBoulder"))
        {
            RotateTowardsPlayer();
        }
    }

    private void RotateTowardsPlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }
}
