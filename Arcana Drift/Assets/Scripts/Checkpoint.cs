using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public static Vector3 lastCheckpointPosition;

    private void Start()
    {
        lastCheckpointPosition = Vector3.zero;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            lastCheckpointPosition = transform.position;
            SpawnManager.spawnPosition = lastCheckpointPosition;
            Debug.Log("Checkpoint activated at: " + lastCheckpointPosition);
        }
    }
}