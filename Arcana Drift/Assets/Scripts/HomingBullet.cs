using UnityEngine;

public class HomingBullet : MonoBehaviour
{
    public float damage = 10f;
    public float speed = 2f;                // Slow movement speed
    public float turnSpeed = 2f;            // How quickly it rotates toward the target
    public float lifetime = 10f;            // Auto-destroy after a while

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (!player)
        {
            Debug.LogWarning("HomingBullet couldn't find the player. Make sure the player has the 'Player' tag.");
            Destroy(gameObject);
        }

        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (player == null) return;

        // Direction to player
        Vector3 direction = (player.position - transform.position).normalized;

        // Smoothly rotate toward the player
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);

        // Move forward slowly
        transform.position += transform.forward * (speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
            Debug.Log("Player hit by homing bullet!");

            Destroy(gameObject); // Destroy bullet on hit
        }
    }
}