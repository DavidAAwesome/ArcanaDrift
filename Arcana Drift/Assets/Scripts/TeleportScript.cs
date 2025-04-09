using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TeleportScript : MonoBehaviour
{
    public GameObject TeleportPlatform;
    public KeyCode TeleportKey = KeyCode.F;
    public float teleportCooldown = 3f;

    private bool teleporterPlaced = false;
    private bool canTeleport = true;
    private float cooldownTimer = 0f;

    private GameObject placedTeleporter;
    
    public Image cooldownImage;

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
                transform.position = placedTeleporter.transform.position + Vector3.up * 1.5f; // Ensure player isn't stuck
                Destroy(placedTeleporter);
                teleporterPlaced = false;

                canTeleport = false;
                cooldownTimer = teleportCooldown;
            }
        }
    }
}