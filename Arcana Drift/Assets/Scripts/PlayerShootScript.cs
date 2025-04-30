// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using TMPro;
//
// public class PlayerShootScript : MonoBehaviour
// {
//     [Header("References")]
//     public Transform cam;
//     public Transform attackPoint;
//     public GameObject objectToThrow;
//
//     [Header("Settings")]
//     public int totalThrows;
//     public float throwCooldown;
//
//     [Header("Throwing")]
//     public KeyCode throwKey = KeyCode.Mouse0;
//     public float throwForce;
//     public float throwUpwardForce;
//
//     bool readyToThrow;
//
//     private void Start()
//     {
//         readyToThrow = true;
//     }
//
//     private void Update()
//     {
//         if(Input.GetKeyDown(throwKey) && readyToThrow && totalThrows > 0)
//         {
//             Throw();
//         }
//     }
//
//     private void Throw()
//     {
//         readyToThrow = false;
//
//         // instantiate object to throw
//         GameObject projectile = Instantiate(objectToThrow, attackPoint.position, cam.rotation);
//
//         // get rigidbody component
//         Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
//
//         // calculate direction
//         Vector3 forceDirection = cam.transform.forward;
//
//         RaycastHit hit;
//
//         if(Physics.Raycast(cam.position, cam.forward, out hit, 500f))
//         {
//             forceDirection = (hit.point - attackPoint.position).normalized;
//         }
//
//         // add force
//         Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;
//
//         projectileRb.AddForce(forceToAdd, ForceMode.Impulse);
//
//         totalThrows--;
//
//         // implement throwCooldown
//         Invoke(nameof(ResetThrow), throwCooldown);
//     }
//
//     private void ResetThrow()
//     {
//         readyToThrow = true;
//     }
// }

// using System.Collections;
// using UnityEngine;
//
// public class PlayerShootScript : MonoBehaviour
// {
//     [Header("References")]
//     public Transform cam;
//     public Transform attackPoint;
//     public GameObject objectToThrow;
//
//     [Header("Settings")]
//     public KeyCode throwKey = KeyCode.Mouse0;
//     public int totalThrows = 10;
//     public float throwCooldown = 0.5f;
//     public float throwForce = 20f;
//     public float throwUpwardForce = 5f;
//
//     [Header("Charging")]
//     public float maxChargeTime = 2f;             // Max hold duration
//     public float maxScaleMultiplier = 2f;        // Max scale relative to default
//     public float minProjectileScale = 0.5f;      // Minimum starting scale
//     public float baseDamage = 10f;
//
//     private float chargeTime = 0f;
//     private bool isCharging = false;
//     private bool readyToThrow = true;
//
//     private void Update()
//     {
//         if (Input.GetKeyDown(throwKey) && readyToThrow && totalThrows > 0)
//         {
//             isCharging = true;
//             chargeTime = 0f;
//         }
//
//         if (Input.GetKey(throwKey) && isCharging)
//         {
//             chargeTime += Time.deltaTime;
//             chargeTime = Mathf.Clamp(chargeTime, 0f, maxChargeTime);
//         }
//
//         if (Input.GetKeyUp(throwKey) && isCharging)
//         {
//             Throw();
//             isCharging = false;
//         }
//     }
//
//     private void Throw()
//     {
//         readyToThrow = false;
//
//         // Calculate scale and damage from chargeTime
//         float chargePercent = chargeTime / maxChargeTime;
//         float scaleMultiplier = Mathf.Lerp(minProjectileScale, maxScaleMultiplier, chargePercent);
//         float scaledDamage = Mathf.Lerp(baseDamage, baseDamage * maxScaleMultiplier, chargePercent);
//
//         GameObject projectile = Instantiate(objectToThrow, attackPoint.position, cam.rotation);
//
//         // Scale the projectile
//         projectile.transform.localScale *= scaleMultiplier;
//
//         // Set damage
//         PlayerProjectile projScript = projectile.GetComponent<PlayerProjectile>();
//         if (projScript != null)
//         {
//             projScript.damage = Mathf.RoundToInt(scaledDamage);
//         }
//
//         // Launch
//         Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
//         Vector3 forceDirection = cam.forward;
//
//         if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, 500f))
//         {
//             forceDirection = (hit.point - attackPoint.position).normalized;
//         }
//
//         Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;
//         projectileRb.AddForce(forceToAdd, ForceMode.Impulse);
//
//         totalThrows--;
//
//         Invoke(nameof(ResetThrow), throwCooldown);
//     }
//
//     private void ResetThrow()
//     {
//         readyToThrow = true;
//     }
// }

using UnityEngine;

public class PlayerShootScript : MonoBehaviour
{
    [Header("References")]
    public Transform cam;
    public Transform attackPoint;
    public GameObject objectToThrow;

    [Header("Settings")]
    public int totalThrows = 10;
    public float throwCooldown = 0.5f;
    public float throwForce = 20f;
    public float throwUpwardForce = 5f;

    [Header("Charging")]
    public float maxChargeTime = 2f;
    public float maxScaleMultiplier = 2f;
    public float minProjectileScale = 0.5f;
    public float baseDamage = 10f;

    public KeyCode throwKey = KeyCode.Mouse0;

    private float chargeTime = 0f;
    private bool isCharging = false;
    private bool readyToThrow = true;

    private GameObject currentChargingProjectile;

    private void Update()
    {
        if (Input.GetKeyDown(throwKey) && readyToThrow && totalThrows > 0)
        {
            StartCharging();
        }

        if (Input.GetKey(throwKey) && isCharging)
        {
            ChargeProjectile();
        }

        if (Input.GetKeyUp(throwKey) && isCharging)
        {
            ReleaseProjectile();
        }
    }

    private void StartCharging()
    {
        isCharging = true;
        chargeTime = 0f;

        // Spawn the projectile at attack point but don't launch it yet
        currentChargingProjectile = Instantiate(objectToThrow, attackPoint.position, cam.rotation, attackPoint);
        currentChargingProjectile.transform.localScale = Vector3.one * minProjectileScale;
    }

    private void ChargeProjectile()
    {
        if (currentChargingProjectile == null) return;

        chargeTime += Time.deltaTime;
        chargeTime = Mathf.Clamp(chargeTime, 0f, maxChargeTime);

        float chargePercent = chargeTime / maxChargeTime;
        float currentScale = Mathf.Lerp(minProjectileScale, maxScaleMultiplier, chargePercent);
        currentChargingProjectile.transform.localScale = Vector3.one * currentScale;
    }

    private void ReleaseProjectile()
    {
        readyToThrow = false;
        isCharging = false;

        if (currentChargingProjectile == null) return;

        // Detach from player
        currentChargingProjectile.transform.parent = null;

        // Set damage based on charge
        float chargePercent = chargeTime / maxChargeTime;
        float scaledDamage = Mathf.Lerp(baseDamage, baseDamage * maxScaleMultiplier, chargePercent);

        PlayerProjectile projScript = currentChargingProjectile.GetComponent<PlayerProjectile>();
        if (projScript != null)
        {
            projScript.damage = Mathf.RoundToInt(scaledDamage);
        }

        // Apply force
        Rigidbody projectileRb = currentChargingProjectile.GetComponent<Rigidbody>();
        Vector3 forceDirection = cam.forward;

        if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, 500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }

        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;
        projectileRb.isKinematic = false;
        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        // Clean up
        currentChargingProjectile = null;
        totalThrows--;

        Invoke(nameof(ResetThrow), throwCooldown);
    }

    private void ResetThrow()
    {
        readyToThrow = true;
    }
}

