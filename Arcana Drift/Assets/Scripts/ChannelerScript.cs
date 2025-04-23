using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ChannelerScript : MonoBehaviour
{
    public float health = 100f;
    public enum State { Wandering, Chasing, Attacking }
    public State currentState;

    public Transform player;
    public float detectionRange = 20f;
    public float attackRange = 10f;
    public float moveSpeed = 5f;
    public float wanderRadius = 15f;
    public float shootCooldown = 2f;
    public GameObject projectilePrefab;
    public Transform shootPoint;

    public float fixedY = 3f; // <- The fixed world Y position for the floating eye
    public float obstacleCheckDistance = 2f;
    public LayerMask obstacleLayers;

    private float shootTimer;
    private Vector3 wanderTarget;
    private float wanderTimer;
    private Rigidbody rb;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        currentState = State.Wandering;
        PickNewWanderTarget();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(new Vector3(transform.position.x, 0f, transform.position.z), new Vector3(player.position.x, 0f, player.position.z));
        shootTimer -= Time.deltaTime;

        // State Transitions
        if (distanceToPlayer <= attackRange)
            currentState = State.Attacking;
        else if (distanceToPlayer <= detectionRange)
            currentState = State.Chasing;
        else
            currentState = State.Wandering;

        switch (currentState)
        {
            case State.Wandering:
                Wander();
                break;
            case State.Chasing:
                ChasePlayer();
                break;
            case State.Attacking:
                AttackPlayer();
                break;
        }

        LookAtPlayer();
    }

    void Wander()
    {
        wanderTimer += Time.deltaTime;
        if (wanderTimer > 5f || Vector3.Distance(transform.position, wanderTarget) < 2f)
        {
            PickNewWanderTarget();
            wanderTimer = 0f;
        }

        MoveTowards(wanderTarget);
    }

    void PickNewWanderTarget()
    {
        Vector3 randomDir = Random.insideUnitSphere * wanderRadius;
        randomDir.y = 0; // Keep it flat
        randomDir += transform.position;
        wanderTarget = new Vector3(randomDir.x, fixedY, randomDir.z);
    }

    void ChasePlayer()
    {
        Vector3 target = new Vector3(player.position.x, fixedY, player.position.z);
        MoveTowards(target);
    }

    void AttackPlayer()
    {
        rb.linearVelocity = Vector3.zero;

        if (shootTimer <= 0f)
        {
            Shoot();
            shootTimer = shootCooldown;
        }
    }

    void MoveTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;

        if (!Physics.Raycast(transform.position, direction, obstacleCheckDistance, obstacleLayers))
        {
            rb.linearVelocity = direction * moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector3.zero;
        }

        // Maintain fixed Y position
        transform.position = new Vector3(transform.position.x, fixedY, transform.position.z);
    }

    void LookAtPlayer()
    {
        Vector3 lookDir = player.position - transform.position;
        lookDir.y = 0f; // Look only horizontally
        if (lookDir != Vector3.zero)
        {
            Quaternion lookRot = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 5f);
        }
    }

    void Shoot()
    {
        if (projectilePrefab && shootPoint)
        {
            GameObject proj = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
            Rigidbody rb = proj.GetComponent<Rigidbody>();
            if (rb != null)
                rb.linearVelocity = shootPoint.forward * 20f;
        }
    }
    
    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
