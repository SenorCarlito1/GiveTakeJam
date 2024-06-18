using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    public float walkPointRange = 5f; // Range within which to choose the next patrol point
    public float sightRange = 10f; // Range within which the enemy can detect the player
    public float fieldOfView = 110f; // Field of view angle in degrees
    public float chaseStoppingDistance = 2f; // Distance at which the enemy stops when chasing
    public float minDistanceToPlayer = 1.5f; // Minimum distance to maintain from the player
    public float memoryDuration = 3f; // Duration to remember player's last known position after losing sight
    public Transform player; // Reference to the player transform

    private Vector3 walkPoint;
    private bool walkPointSet;
    private NavMeshAgent navAgent;
    private bool isChasing;
    private Vector3 lastKnownPlayerPosition;
    private float memoryTimer;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        SetNewWalkPoint();
    }

    void Update()
    {
        if (CanSeePlayer())
        {
            // Update last known player position
            lastKnownPlayerPosition = player.position;
            memoryTimer = memoryDuration;

            ChasePlayer();
        }
        else
        {
            if (isChasing)
            {
                // If no longer chasing but still in memory duration, continue towards last known position
                if (memoryTimer > 0)
                {
                    navAgent.SetDestination(lastKnownPlayerPosition);
                    memoryTimer -= Time.deltaTime;
                }
                else
                {
                    isChasing = false;
                    SetNewWalkPoint();
                }
            }
            else
            {
                Patrol();
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
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Vector3 targetPosition = player.position - directionToPlayer * minDistanceToPlayer;
        navAgent.SetDestination(targetPosition);

        // Ensure enemy stops before colliding with the player
        if (Vector3.Distance(transform.position, player.position) <= chaseStoppingDistance)
        {
            // Add logic to attack or handle close proximity to the player
            // For example, you can add attack behavior or trigger game over here
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

    private void OnDrawGizmosSelected()
    {
        // Draw patrol range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, walkPointRange);

        // Draw sight range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        // Draw last known position
        if (isChasing && memoryTimer > 0)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, lastKnownPlayerPosition);
        }
    }
}
