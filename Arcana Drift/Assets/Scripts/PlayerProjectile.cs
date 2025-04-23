using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public int damage;

    private Rigidbody rb;

    private bool targetHit;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // make sure only to stick to the first target you hit
        if (targetHit)
            return;
        else
            targetHit = true;

        // check if you hit an enemy
        SeekerScript seeker;
        seeker = collision.gameObject.GetComponent<SeekerScript>();
        if (seeker != null)
        {
            seeker.TakeDamage(damage);
            Destroy(gameObject);
        }
        ChannelerScript channeler;
        channeler = collision.gameObject.GetComponent<ChannelerScript>();
        if (channeler != null)
        {
            channeler.TakeDamage(damage);
            Destroy(gameObject);
        }

        // make sure projectile sticks to surface
        rb.isKinematic = true;

        // make sure projectile moves with target
        transform.SetParent(collision.transform);
    }
}