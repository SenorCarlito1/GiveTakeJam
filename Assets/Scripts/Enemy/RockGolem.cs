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
    [SerializeField] private GameObject player;

    [Header("---Attack Settings---")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private float throwTriggerDistance = 10f;
    [SerializeField] private float punchDistance = 2f;
    [SerializeField] private float punchDamageAmount = 20f;
    [SerializeField] private float punchKnockbackForce = 20f;

    [Header("---Rotation Settings---")]
    [SerializeField] private float rotationSpeed = 5f; // Adjust as needed

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

    private List<Vector3> playerPositions = new List<Vector3>();
    private List<float> positionTimestamps = new List<float>();

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("Player GameObject not found. Make sure to tag your player GameObject with 'Player'.");
        }
    }

    void Update()
    {
        if (enemyHealth.currHealth <= 0)
        {
            return;
        }

        UpdatePlayerPositionHistory();

        if (player != null && CanSeePlayer())
        {
            lastKnownPlayerPosition = player.transform.position;
            memoryTimer = memoryDuration;

            if (isChasing && Vector3.Distance(transform.position, player.transform.position) > throwTriggerDistance)
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

        if (player != null && Vector3.Distance(transform.position, player.transform.position) <= punchDistance)
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
        if (player != null && Vector3.Distance(transform.position, player.transform.position) < sightRange)
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
                        lastKnownPlayerPosition = player.transform.position;
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private bool IsPlayerInThrowArc()
    {
        if (player != null)
        {
            Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
            float angleBetweenEnemyAndPlayer = Vector3.Angle(transform.forward, directionToPlayer);
            return angleBetweenEnemyAndPlayer <= 45f; // 90 degrees arc
        }
        return false;
    }

    private bool IsTargetWithinFOV(Vector3 targetPosition)
    {
        Vector3 directionToTarget = (targetPosition - transform.position).normalized;
        float angleBetweenEnemyAndTarget = Vector3.Angle(transform.forward, directionToTarget);
        return angleBetweenEnemyAndTarget < fieldOfView / 2;
    }

    private void ChasePlayer()
    {
        if (player != null)
        {
            isChasing = true;
            isIdle = false;
            Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
            Vector3 targetPosition = player.transform.position - directionToPlayer * minDistanceToPlayer;
            navAgent.SetDestination(targetPosition);

            if (Vector3.Distance(transform.position, player.transform.position) <= chaseStoppingDistance)
            {
                Debug.Log("Player too close! Implement attack or game over logic.");
            }
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
        if (player == null)
        {
            return;
        }

        Vector3 targetPosition = player.transform.position;

        if (!CanSeePlayer())
        {
            targetPosition = GetPositionOneSecondAgo();
        }

        if (!IsTargetWithinFOV(targetPosition))
        {
            isThrowing = false;
            return;
        }

        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        Vector3 directionToPlayer = (targetPosition - projectileSpawnPoint.position).normalized;

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(directionToPlayer * throwForce, ForceMode.Impulse);
        }

        ProjectileController controller = projectile.GetComponent<ProjectileController>();
        if (controller != null)
        {
            controller.target = player.transform;
        }

        isThrowing = false;
        hasThrownProjectile = true;
        throwTimer = 1f;
    }

    private void UpdatePlayerPositionHistory()
    {
        if (player != null)
        {
            playerPositions.Add(player.transform.position);
            positionTimestamps.Add(Time.time);

            while (positionTimestamps.Count > 0 && Time.time - positionTimestamps[0] > 5f)
            {
                playerPositions.RemoveAt(0);
                positionTimestamps.RemoveAt(0);
            }
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
        return lastKnownPlayerPosition;
    }

    public void PerformPunch()
    {
        if (player != null && Vector3.Distance(transform.position, player.transform.position) <= punchDistance)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(punchDamageAmount);

                Vector3 knockbackDirection = (player.transform.position - transform.position).normalized;
                playerHealth.TakeKnockback(knockbackDirection, punchKnockbackForce);
            }
        }
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("ThrowBoulder"))
        {
            RotateTowardsPlayer();
        }
    }

    public void TakeDamageAnimation()
    {
        animator.SetTrigger("TakeDamage");
    }

    public void DeathAnimation()
    {
        animator.SetTrigger("Die");
    }

    private void RotateTowardsPlayer()
    {
        if (player != null)
        {
            Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
