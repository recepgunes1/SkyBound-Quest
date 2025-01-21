using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float jumpVelocity;
    [SerializeField] private float moveSpeed = 5f;
    private float moveInput;

    private Rigidbody2D rb;
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

        moveInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
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
        rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
    }

    void SetJumpFallSpeed()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.velocity.y > 0 && !(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    void Run()
    {
        if (moveInput != 0)
        {
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y); // Instant response to input
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y); // Stop horizontal movement immediately
        }
    }

    float GetCurrentSpeed()
    {
        return rb.velocity.x;
    }
}