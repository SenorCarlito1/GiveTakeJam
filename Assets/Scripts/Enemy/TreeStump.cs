using UnityEngine;
using UnityEngine.AI;

public class TreeStump : MonoBehaviour
{
    [Header("---Patrol Settings---")]
    [SerializeField] private float walkPointRange = 5f;

    [Header("---Detection Settings---")]
    [SerializeField] private float sightRange = 10f;
    [SerializeField] private float fieldOfView = 110f;

    [Header("---Chase Settings---")]
    [SerializeField] private float chaseStoppingDistance = 2f;
    [SerializeField] private float minDistanceToPlayer = 1.5f;

    [Header("---Attack Settings---")]
    [SerializeField] private float spinAttackDistance = 2f; // Distance to perform spin attack
    [SerializeField] private float spinAttackDamage = 30f; // Damage of spin attack

    private Vector3 walkPoint;
    private bool walkPointSet;
    private NavMeshAgent navAgent;
    private bool isChasing;
    private Vector3 lastKnownPlayerPosition;
    private float memoryTimer;
    private Animator animator;
    private EnemyHealth enemyHealth;
    private GameObject player; // Changed to GameObject

    private bool isAttacking; // Flag to indicate if currently attacking

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("Player not found. Make sure the player object is tagged as 'Player'.");
            enabled = false; // Disable this script if player is not found
            return;
        }

        SetNewWalkPoint();
    }

    void Update()
    {
        if (enemyHealth.currHealth <= 0)
        {
            return;
        }

        if (CanSeePlayer())
        {
            lastKnownPlayerPosition = player.transform.position;
            memoryTimer = sightRange;

            // Only chase if not currently attacking
            if (!isAttacking)
            {
                ChasePlayer();
            }
        }
        else
        {
            if (isChasing && !isAttacking)
            {
                if (memoryTimer > 0)
                {
                    navAgent.SetDestination(lastKnownPlayerPosition);
                    memoryTimer -= Time.deltaTime;
                }
                else
                {
                    isChasing = false;
                    Patrol();
                }
            }
            else if (!isAttacking)
            {
                Patrol();
            }
        }

        animator.SetFloat("Speed", navAgent.velocity.magnitude);

        // Check if the spin attack animation is currently playing
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("SpinAttack"))
        {
            // Set attacking flag to true during attack animation
            isAttacking = true;

            // Stop chasing and rotating towards player during attack animation
            navAgent.isStopped = true;
            navAgent.velocity = Vector3.zero;
            RotateTowards(transform.forward); // Rotate towards current facing direction
        }
        else
        {
            // Reset attacking flag when not attacking
            isAttacking = false;

            // Regular behavior when not in spin attack
            if (Vector3.Distance(transform.position, player.transform.position) <= spinAttackDistance)
            {
                animator.SetTrigger("SpinAttack");
            }
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
        if (walkPointSet)
        {
            navAgent.SetDestination(walkPoint);
            RotateTowards(walkPoint);

            if (navAgent.remainingDistance <= navAgent.stoppingDistance && !navAgent.pathPending)
            {
                walkPointSet = false;
                SetNewWalkPoint();
            }
        }
        else
        {
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

    private void RotateTowards(Vector3 target)
    {
        Vector3 directionToTarget = (target - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * navAgent.angularSpeed);
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

    // Called by animation event for spin attack
    public void PerformSpinAttack()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= spinAttackDistance)
        {
            // Apply damage to the player
            player.GetComponent<PlayerHealth>().TakeDamage(spinAttackDamage);
        }
    }
}
