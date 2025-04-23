using UnityEngine;
using System.Collections;

public class ExplosionScript : MonoBehaviour
{
    private SeekerScript sc;
    private Rigidbody rb;
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered");
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<SeekerScript>()?.TakeDamage(100f);
            other.GetComponent<ChannelerScript>()?.TakeDamage(100f);
            // sc = other.GetComponent<SeekerScript>();
            // sc.TakeDamage(100f);
            // sc.agent.updatePosition = false;
            // sc.agent.updateRotation = false;
            // sc.agent.isStopped = true;
            // rb = other.GetComponent<Rigidbody>();
            // rb.isKinematic = false;
            // rb.AddForce(other.transform.position.normalized - transform.position.normalized * 20f, ForceMode.Force);
            // StartCoroutine(RecoverFromKnockback());
        }
    }
    
    IEnumerator RecoverFromKnockback()
    {
        yield return new WaitForSeconds(2f);

        // Stop Rigidbody movement
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;

        // Snap NavMeshAgent back to Rigidbody position
        sc.agent.Warp(transform.position);

        // Resume NavMeshAgent control
        sc.agent.updatePosition = true;
        sc.agent.updateRotation = true;
        sc.agent.isStopped = false;
    }
}
