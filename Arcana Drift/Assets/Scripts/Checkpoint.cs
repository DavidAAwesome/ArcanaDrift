using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public static Vector3 lastCheckpointPosition;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        lastCheckpointPosition = Vector3.zero;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            lastCheckpointPosition = transform.position;
            // SpawnManager.spawnPosition = lastCheckpointPosition;
            gameManager.SetCheckpoint(lastCheckpointPosition);
            Debug.Log("Checkpoint activated at: " + lastCheckpointPosition);
        }
    }
}