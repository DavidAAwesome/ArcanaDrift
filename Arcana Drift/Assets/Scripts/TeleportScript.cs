using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TeleportScript : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject TeleportPlatform;
    public KeyCode TeleportKey = KeyCode.F;
    public float teleportCooldown = 3f;

    private bool teleporterPlaced = false;
    private bool canTeleport = true;
    private float cooldownTimer = 0f;

    private static GameObject placedTeleporter;
    
    public Image cooldownImage;
    public bool justTeleported = false;

    void Update()
    {
        // Cooldown logic
        if (!canTeleport)
        {
            cooldownTimer -= Time.deltaTime;
            cooldownImage.fillAmount = 1f - (cooldownTimer / teleportCooldown);
            if (cooldownTimer <= 0f)
            {
                canTeleport = true;
                cooldownImage.fillAmount = 1f; // Show it's full
            }
        }
        else
        {
            cooldownImage.fillAmount = 1f; // always full when ready
        }

        // Teleport input logic
        if (Input.GetKeyDown(TeleportKey))
        {
            if (!teleporterPlaced)
            {
                // Spawn teleporter at player's feet
                Vector3 spawnPosition = transform.position + Vector3.down * 1f; // Adjust 1f as needed
                placedTeleporter = Instantiate(TeleportPlatform, spawnPosition, Quaternion.identity);
                teleporterPlaced = true;
            }
            else if (canTeleport && placedTeleporter != null)
            {
                // Teleport to teleporter and start cooldown
                Rigidbody rb = GetComponent<Rigidbody>();
                if (rb != null)
                {
                    // rb.isKinematic = true;
                    // rb.linearVelocity = Vector3.zero;
                    // rb.angularVelocity = Vector3.zero;
                    rb.MovePosition(placedTeleporter.transform.position + Vector3.up * 3f);
                    //transform.position = placedTeleporter.transform.position + Vector3.up * 3f;
                    Debug.Log("Teleported to: " + transform.position);
                    // rb.isKinematic = false;
                }
                // transform.position = placedTeleporter.transform.position + Vector3.up * 1.5f; // Ensure player isn't stuck
                Destroy(placedTeleporter);
                teleporterPlaced = false;

                canTeleport = false;
                cooldownTimer = teleportCooldown;
            }
        }
    }
    
    public void TeleportTo(Vector3 pos) {
        rb.isKinematic = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = pos;
        rb.isKinematic = false;
    
        justTeleported = true;
        // teleportBufferTime = 0.1f;
    }
}