using UnityEngine;

public class Smokescreen : MonoBehaviour
{
    public float damagePerSecond = 10f;
    public float tickRate = 0.5f; // Damage interval
    private float tickTimer = 0f;

    private bool playerInside = false;
    private GameObject player;
    private PlayerController pc;

    void OnCollisionEnter(Collision other)
    {
        Debug.Log("Triggered");
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Found Player");
            player = other.gameObject;
            pc = other.gameObject.GetComponent<PlayerController>();
            Debug.Log(pc == null);
            playerInside = true;
            pc.TakeDamage(damagePerSecond);
            // tickTimer = tickRate; // Start with an immediate tick
            Destroy(gameObject);
        }
    }

    // void OnTriggerExit(Collider other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         playerInside = false;
    //         player = null;
    //         pc = null;
    //     }
    // }
    //
    // void Update()
    // {
    //     if (playerInside && pc != null)
    //     {
    //         // tickTimer -= Time.deltaTime;
    //         // if (tickTimer <= 0f)
    //         // {
    //         //     ApplyDamage();
    //         //     tickTimer = tickRate;
    //         // }
    //         Debug.Log("About to apply damage");
    //         pc.TakeDamage(damagePerSecond * Time.deltaTime);
    //         // ApplyDamage();
    //     }
    // }
    //
    // void ApplyDamage()
    // {
    //     // Replace with your own health system
    //     PlayerController health = player.GetComponent<PlayerController>();
    //     health.TakeDamage(damagePerSecond * Time.deltaTime);
    //     // Debug.Log("pc:" + health);
    //     // if (health != null)
    //     // {
    //     //     Debug.Log("Trying to deal damage");
    //     //     health.TakeDamage(damagePerSecond * Time.deltaTime);//tickRate);
    //     // }
    // }
}