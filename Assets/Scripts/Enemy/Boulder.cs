using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float throwSpeed = 10f; // Speed of the projectile when thrown
    public float gravityMultiplier = 1f; // Gravity multiplier for realistic projectile motion
    public bool useBounce = true; // Enable/disable bouncing
    public float bounceForce = 5f; // Force applied on bounce
    public int maxBounces = 3; // Maximum number of bounces before destroying the projectile
    public LayerMask groundLayer; // Layer mask for detecting ground
    public Transform target; // Target transform (player's position)
    public float damageAmount = 10f; // Amount of damage to apply on collision

    private Rigidbody rb;
    private Vector3 startPoint;
    private Vector3 apexPoint; // Point of maximum height in the arc
    private Vector3 targetPoint;
    private int bounceCount = 0;

    // Timer variables
    private float destroyTimer = 5f; // Time in seconds before projectile is destroyed

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPoint = transform.position;
        targetPoint = target.position;

        // Calculate apex point for the rainbow-like arc
        CalculateApexPoint();

        // Apply initial velocity to start the rainbow-like arc
        rb.velocity = CalculateInitialVelocity();

        // Start the destroy timer
        Invoke("DestroyProjectile", destroyTimer);
    }

    void Update()
    {
        // Apply gravity
        rb.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the projectile should bounce and if it has not exceeded the maximum bounces
        if (useBounce && bounceCount < maxBounces)
        {
            // Check if collided with ground layer
            if (IsGround(collision.gameObject))
            {
                // Calculate reflection direction
                Vector3 reflectedDir = Vector3.Reflect(rb.velocity, collision.contacts[0].normal);

                // Apply bounce force
                rb.velocity = reflectedDir.normalized * bounceForce;

                bounceCount++;

                // You can add particle effects, sound effects, or other behavior here
            }
        }
        else
        {
            // Destroy the projectile if bouncing is disabled or it has exceeded the maximum bounces
            DestroyProjectile();
        }

        // Check if the projectile collides with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Apply damage to the player
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damageAmount);

            // Destroy the projectile after damaging the player
            DestroyProjectile();
        }
    }

    void DestroyProjectile()
    {
        // Destroy the projectile game object
        Destroy(gameObject);
    }

    bool IsGround(GameObject obj)
    {
        // Check if collided object is in the ground layer
        return (groundLayer.value & (1 << obj.layer)) != 0;
    }

    void CalculateApexPoint()
    {
        // Calculate a point above the midpoint between start and target to create a rainbow-like arc
        float apexHeight = Vector3.Distance(startPoint, targetPoint) * 0.25f;
        apexPoint = (startPoint + targetPoint) * 0.25f + Vector3.up * apexHeight;
    }

    Vector3 CalculateInitialVelocity()
    {
        // Calculate initial velocity to achieve the rainbow-like arc
        Vector3 direction = (targetPoint - startPoint).normalized;
        float distance = Vector3.Distance(startPoint, targetPoint);

        // Adjust apex point
        float apexHeight = distance * 0.1f; // Adjust this multiplier to control peak height
        apexPoint = (startPoint + targetPoint) * 0.5f + Vector3.up * apexHeight;

        // Calculate time to apex and total time
        float timeToApex = Mathf.Sqrt(2f * apexHeight / Mathf.Abs(Physics.gravity.y));
        float totalTime = timeToApex * 2f; // Total time for full arc (up and down)

        // Calculate initial velocity
        Vector3 velocity = direction * throwSpeed;
        velocity.y = (apexPoint.y - startPoint.y) / timeToApex - 0.5f * Physics.gravity.y * timeToApex;

        return velocity;
    }
}
