using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    public MovementState state;
    
    [Header("Movement")] 
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float driftSpeed;
    public float groundDrag;
    public Transform orientation;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    private bool readyToJump;

    [Header("Crouching")] 
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Keybinds")] 
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;
    public KeyCode driftKey = KeyCode.Mouse1;

    [Header("Ground Check")] 
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool isGrounded;

    [Header("Slope Handling")] 
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("Stats")] 
    public float maxHealth = 100f;
    public float health = 100f;
    public float maxMana = 100f;
    public float mana = 100f;
    
    [Header("Particle Handling")]
    public GameObject driftingParticles;
    private bool spawnParticles = false;
    private float particleCooldown = 0.05f; // Cooldown time in seconds
    private float nextSpawnTime = 0f;
    
    [Header("UI")]
    public Slider healthBar;
    public Slider manaBar;
    
    private float horizontalInput;
    private float verticalInput;

    private Vector3 moveDirection;
    private Rigidbody rb;

    public Transform playerObject;
    
    

    

    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        drifting,
        air,
    }

    private void Start()
    {
        // transform.position = SpawnManager.spawnPosition + Vector3.up * 2f;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        startYScale = transform.localScale.y;
        
    }

    private void Update()
    {
        //Grounded Check
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        playerObject.rotation = orientation.rotation;
        
        MyInput();
        SpeedControl();
        StateHandler();
        
        //handle Drag
        if (isGrounded)
            rb.linearDamping = groundDrag;
        else
            rb.linearDamping = 0;

        if (mana < 100)
            mana += 1 * Time.deltaTime;

        if (spawnParticles && Time.time >= nextSpawnTime)
        {
            // Spawn particle
            GameObject particle = Instantiate(driftingParticles, transform.position + Vector3.down * 1f, transform.rotation);
            Destroy(particle, 2f);

            // Set the next spawn time
            nextSpawnTime = Time.time + particleCooldown;
        }
        
        if (healthBar != null)
            healthBar.value = health;

        if (manaBar != null)
            manaBar.value = mana;
        
        health = Mathf.Clamp(health, 0, 100);
        mana = Mathf.Clamp(mana, 0, 100);

        if (transform.position.y < -15)
            KillPlayer();
        if(health <= 0)
            KillPlayer();

    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
        // When to Jump
        if (Input.GetKey(jumpKey) && readyToJump && isGrounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        
        // Start Crouch
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    private void StateHandler()
    {
        if (Input.GetKey(driftKey) && mana > 0f && GameManager.Instance.HasAbility(GameManager.Abilities.TurboBoost))
        {
            state = MovementState.drifting;
            moveSpeed = driftSpeed;
            spawnParticles = true;
            // Debug.Log("Drifting");
        }
        else if (Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
            spawnParticles = false;
            // Debug.Log("Crouching");
        }
        // Sprinting
        else if (isGrounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
            spawnParticles = false;
            // Debug.Log("Sprinting");
        }
        // Walking
        else if (isGrounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
            spawnParticles = false;
            // Debug.Log("Walking");
        }
        //Air
        else
        {
            if(state == MovementState.drifting)
                if (Input.GetKey(sprintKey))
                    moveSpeed = sprintSpeed;
                else
                    moveSpeed = walkSpeed;
            state = MovementState.air;
            spawnParticles = false;
            // Debug.Log("Airborne");
        }
    }

    private void MovePlayer()
    {
        // Calculate Movement Direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (state == MovementState.drifting)
        {
            mana -= 20f * Time.deltaTime;
        }
        
        // On Slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * (moveSpeed * 20f), ForceMode.Force);
            
            if(rb.linearVelocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }
        // On Ground
        else if(isGrounded)
            rb.AddForce(moveDirection.normalized * (moveSpeed * 10f), ForceMode.Force);
        // In Air
        else
            rb.AddForce(moveDirection.normalized * (moveSpeed * 10f * airMultiplier), ForceMode.Force);
        
        // Turn Gravity off while on slope
        rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        //Limit speed on a slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.linearVelocity.magnitude > moveSpeed)
                rb.linearVelocity = rb.linearVelocity.normalized * moveSpeed;
        }
        //Limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            
            // To Limit the velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
            }

        }
    }

    private void Jump()
    {
        exitingSlope = true;
        
        //Reset y velocity
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f * 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }

    public void TakeDamage(float damage)
    { 
        health -= damage;
        Debug.Log("Health: " + health);
    }

    public void KillPlayer()
    {
        Respawn();
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    void Respawn()
    {
        // if (Checkpoint.lastCheckpointPosition != Vector3.zero)
        // {
        //     transform.position = SpawnManager.spawnPosition + Vector3.up * 1f; // Raise a bit to avoid clipping
        // }
        // else
        // {
        //     Debug.Log("No checkpoint set. Respawn failed or using default position.");
        // }
        GameManager.Instance.Respawn();
        

        health = maxHealth;
        mana = maxMana;
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.gameObject.CompareTag("Health"))
    //     {
    //         Destroy(other.gameObject);
    //         health = maxHealth;
    //     }
    //     else if (other.gameObject.CompareTag("Mana"))
    //     {
    //         Destroy(other.gameObject);
    //         mana = maxMana;
    //     }
    //         
    // }
}
