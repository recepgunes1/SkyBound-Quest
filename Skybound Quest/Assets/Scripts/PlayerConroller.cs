using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float jumpVelocity;
    [SerializeField] private float moveSpeed = 5f;
    private float moveInput;

    Rigidbody2D rb;
    public float currentSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        currentSpeed = GetCurrentSpeed();
        if (currentSpeed != 0)
        {
            Debug.Log("Current Speed: " + currentSpeed);
        }
        
        moveInput = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        Run();
        SetJumpFallSpeed();
    }

    void Jump()
    {
        rb.velocity = Vector2.up * jumpVelocity;
    }

    void SetJumpFallSpeed()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    void Run()
    {
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }
    
    float GetCurrentSpeed()
    {
        return rb.velocity.magnitude;
    }

}