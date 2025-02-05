using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float jumpVelocity;

    [SerializeField] private float moveSpeed = 5f;
    private float _moveInput;
    private Rigidbody2D _rb;
    public float currentSpeed;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private Vector2 wallCheckSize;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isTouchingWall;

    [Header("Dash Settings")] [SerializeField]
    private float dashVelocity = 15f;

    [SerializeField] private float dashTime = 0.1f;
    private Vector2 _dashDirection;
    private bool _isDashing;
    private bool _canDash = true;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _moveInput = Input.GetAxisRaw("Horizontal");
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        isTouchingWall = Physics2D.OverlapBox(wallCheck.position, wallCheckSize, 0, groundLayer);

        if (Input.GetKeyDown(KeyCode.Period) && isGrounded)
        {
            Jump();
        }

        if (Input.GetButtonDown("Dash") && _canDash)
        {
            StartDash();
        }

        if (isGrounded && !_isDashing)
        {
            _canDash = true;
        }
    }

    private void FixedUpdate()
    {
        if (!_isDashing)
        {
            Run();
            SetJumpFallSpeed();
        }
    }

    private void StartDash()
    {
        _isDashing = true;
        _canDash = false;

        _dashDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (_dashDirection == Vector2.zero)
        {
            _dashDirection = new Vector2(transform.localScale.x, 0); // Default to horizontal dash
        }

        _dashDirection.Normalize();
        _rb.velocity = _dashDirection * dashVelocity;

        StartCoroutine(StopDash());
    }

    private IEnumerator StopDash()
    {
        yield return new WaitForSeconds(dashTime);
        _isDashing = false;

        _rb.velocity = new Vector2(0, _rb.velocity.y);
    }

    private void Jump()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, jumpVelocity);
    }

    private void SetJumpFallSpeed()
    {
        if (_rb.velocity.y < 0)
        {
            _rb.velocity += Vector2.up * (( Physics2D.gravity.y * (fallMultiplier - 1)) * Time.fixedDeltaTime);
        }
        else if (_rb.velocity.y > 0 && !(Input.GetKey(KeyCode.Period)))
        {
            _rb.velocity += Vector2.up * ((Physics2D.gravity.y * (lowJumpMultiplier - 1)) * Time.fixedDeltaTime);

        }
    }

    private void Run()
    {
        if (_moveInput != 0)
        {
            _rb.velocity = new Vector2(_moveInput * moveSpeed, _rb.velocity.y);
        }
        else
        {
            _rb.velocity = new Vector2(0, _rb.velocity.y);
        }
    }

    private void OnDrawGizmos()
    {
        if (wallCheck == null) return;
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
        }
        if (groundCheck == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}