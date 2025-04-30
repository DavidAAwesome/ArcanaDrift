using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Instance")] 
    public static GameManager Instance;
    
    [Header("Ability Tracking")] 
    public HashSet<Abilities> unlockedAbilities = new HashSet<Abilities>();
    
    [Header("Checkpoint tracking")] 
    public Vector3 lastCheckpointPosition;
    public bool hasCheckpoint = false;

    [Header("Controls")] 
    public float sensitivity = 500f;
    
    public GameObject player;
    public GameObject turboBoostIcon;
    public GameObject specialBox;
    private Vector3 specialBoxStartPosition;

    public enum Abilities
    {
        TurboBoost,
        Teleport,
        Fireburst,
    }

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scene loads
        }
        else
        {
            Destroy(gameObject); // Only allow one instance
        }
    }

    private void Start()
    {
        player = GameObject.Find("PlayerPrefab");
        
        if(specialBox != null)
            specialBoxStartPosition = specialBox.transform.position;

        lastCheckpointPosition = player.transform.position;
    }

    void Update()
    {
        if (HasAbility(Abilities.TurboBoost))
            turboBoostIcon.SetActive(true);
    }

    // ========== ABILITIES ==========
    public void UnlockAbility(Abilities ability)
    {
        unlockedAbilities.Add(ability);
        Debug.Log($"Unlocked ability: {ability}");
    }

    public bool HasAbility(Abilities ability)
    {
        return unlockedAbilities.Contains(ability);
    }

    // ========== CHECKPOINTS ==========
    public void SetCheckpoint(Vector3 checkpointPosition)
    {
        lastCheckpointPosition = checkpointPosition;
        hasCheckpoint = true;
        Debug.Log($"Checkpoint set at: {checkpointPosition}");
    }

    public Vector3 GetCheckpointPosition()
    {
        return lastCheckpointPosition;
    }

    public bool HasCheckpoint()
    {
        return hasCheckpoint;
    }

    // ========== RESET ==========
    public void ResetProgress()
    {
        unlockedAbilities.Clear();
        hasCheckpoint = false;
        Debug.Log("Game progress reset.");
    }

    public void Respawn()
    {
        player.GetComponent<Rigidbody>().MovePosition(GetCheckpointPosition() + Vector3.up * 3f);
        specialBox.transform.position = specialBoxStartPosition;
    }
}