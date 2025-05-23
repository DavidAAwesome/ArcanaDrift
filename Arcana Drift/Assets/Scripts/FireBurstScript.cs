// using UnityEngine;
//
// public class FireburstAbility : MonoBehaviour
// {
//     [Header("Burst Settings")]
//     public float burstForce = 15f;
//     public float airDashForce = 20f;
//     public float burstRadius = 5f;
//     public float burstDamage = 20f;
//     public float cooldown = 2f;
//     public float pushForce = 5f;
//     public LayerMask enemyMask;
//     public GameObject explosionVFX;
//     public GameObject explosionParticles;
//
//     private Rigidbody rb;
//     private bool canBurst = true;
//     private bool isGrounded;
//
//     private PlayerController pc;
//
//     void Start()
//     {
//         rb = GetComponent<Rigidbody>();
//         pc = GetComponent<PlayerController>();
//     }
//
//     void Update()
//     {
//         CheckGrounded();
//
//         if (Input.GetKeyDown(KeyCode.Q) && canBurst)
//         {
//             pc.mana -= 20f;
//             StartCoroutine(ActivateFireburst());
//         }
//     }
//
//     private void CheckGrounded()
//     {
//         isGrounded = pc.isGrounded;
//     }
//
//     private System.Collections.IEnumerator ActivateFireburst()
//     {
//         canBurst = false;
//         
//         GameObject particle = Instantiate(explosionParticles, transform.position + Vector3.down * 1f, transform.rotation);
//         Destroy(particle, 2f);
//         
//         // Instantiate explosion VFX
//         if (explosionVFX != null)
//             Instantiate(explosionVFX, transform.position, Quaternion.identity);
//
//         // Damage nearby enemies
//         Collider[] hitEnemies = Physics.OverlapSphere(transform.position, burstRadius, enemyMask);
//         foreach (Collider enemy in hitEnemies)
//         {
//             if (enemy.CompareTag("Enemy"))
//             {
//                 enemy.GetComponent<SeekerScript>()?.TakeDamage(burstDamage);
//                 enemy.GetComponent<ChannelerScript>()?.TakeDamage(burstDamage);
//                 enemy.GetComponent<Rigidbody>()?.AddForce(enemy.transform.position.normalized - transform.position.normalized * pushForce, ForceMode.Force);
//             }
//         }
//
//         // Apply force
//         if (isGrounded)
//         {
//             rb.AddForce(Vector3.up * burstForce, ForceMode.Impulse);
//         }
//         else
//         {
//             Vector3 direction = transform.forward + Vector3.up * 0.5f;
//             rb.AddForce(direction.normalized * airDashForce, ForceMode.Impulse);
//         }
//
//         yield return new WaitForSeconds(cooldown);
//         canBurst = true;
//     }
//
//     void OnDrawGizmosSelected()
//     {
//         Gizmos.color = Color.red;
//         Gizmos.DrawWireSphere(transform.position, burstRadius);
//     }
// }


using UnityEngine;
using System.Collections;

public class FireburstAbility : MonoBehaviour
{
    [Header("Burst Settings")]
    public float burstForce = 15f;
    public float airDashForce = 20f;
    public float burstRadius = 5f;
    public float burstDamage = 20f;
    public float cooldown = 2f;
    public float pushForce = 5f;
    public LayerMask enemyMask;
    public GameObject explosionVFX;
    public GameObject explosionParticles;

    private Rigidbody rb;
    private bool canBurst = true;
    private bool isGrounded;

    private PlayerController pc;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pc = GetComponent<PlayerController>();
    }

    void Update()
    {
        CheckGrounded();

        if (Input.GetKeyDown(KeyCode.Q) && canBurst)
        {
            pc.mana -= 20f;
            StartCoroutine(ActivateFireburst());
        }
    }

    private void CheckGrounded()
    {
        isGrounded = pc.isGrounded;
    }

    private IEnumerator ActivateFireburst()
    {
        canBurst = false;

        GameObject particle = Instantiate(explosionParticles, transform.position + Vector3.down * 1f, transform.rotation);
        Destroy(particle, 2f);

        if (explosionVFX != null)
            Instantiate(explosionVFX, transform.position, Quaternion.identity);

        // Damage nearby enemies
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, burstRadius, enemyMask);
        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                enemy.GetComponent<SeekerScript>()?.TakeDamage(burstDamage);
                enemy.GetComponent<ChannelerScript>()?.TakeDamage(burstDamage);
                enemy.GetComponent<Rigidbody>()?.AddForce(enemy.transform.position.normalized - transform.position.normalized * pushForce, ForceMode.Force);
            }
        }

        // Let PlayerController know not to override y-velocity temporarily
        pc.overrideYVelocity = true;

        // Apply force
        if (isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z); // Optional reset for consistency
            rb.AddForce(Vector3.up * burstForce, ForceMode.Impulse);
        }
        else
        {
            Vector3 direction = transform.forward + Vector3.up * 0.5f;
            rb.AddForce(direction.normalized * airDashForce, ForceMode.Impulse);
        }

        // Allow burst to take effect before restoring control
        yield return new WaitForSeconds(0.2f);
        pc.overrideYVelocity = false;

        yield return new WaitForSeconds(cooldown - 0.2f);
        canBurst = true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, burstRadius);
    }
}

