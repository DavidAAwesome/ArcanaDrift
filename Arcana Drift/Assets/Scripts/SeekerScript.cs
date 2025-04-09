// using UnityEngine;
// using UnityEngine.AI;
//
// public class SeekerScript : MonoBehaviour
// {
//     public float detectionRange = 15f;
//     public float attackRange = 2f;
//     public float attackCooldown = 1.5f;
//     public int damage = 10;
//
//     private Transform player;
//     private NavMeshAgent agent;
//     private float lastAttackTime;
//
//     void Start()
//     {
//         agent = GetComponent<NavMeshAgent>();
//         player = GameObject.FindGameObjectWithTag("Player")?.transform;
//
//         if (player == null)
//         {
//             Debug.LogError("Player not found! Make sure your player is tagged 'Player'.");
//         }
//     }
//
//     void Update()
//     {
//         if (player == null) return;
//
//         float distanceToPlayer = Vector3.Distance(transform.position, player.position);
//
//         if (distanceToPlayer <= detectionRange)
//         {
//             agent.SetDestination(player.position);
//
//             if (distanceToPlayer <= attackRange)
//             {
//                 agent.isStopped = true;
//
//                 if (Time.time - lastAttackTime >= attackCooldown)
//                 {
//                     Attack();
//                     lastAttackTime = Time.time;
//                 }
//             }
//             else
//             {
//                 agent.isStopped = false;
//             }
//         }
//         else
//         {
//             agent.isStopped = true;
//         }
//     }
//
//     void Attack()
//     {
//         // Placeholder attack behavior
//         Debug.Log($"{gameObject.name} attacks the player!");
//
//         // Here you could call a method on a player health script, like:
//         // player.GetComponent<PlayerHealth>().TakeDamage(damage);
//     }
// }