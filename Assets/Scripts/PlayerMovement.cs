using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float jumpSpeed = 20f;
    [SerializeField] float climbingSpeed = 5f;

    [Header("Masks")]
    [SerializeField] LayerMask isJumpable;
    [SerializeField] LayerMask isClimbable;
    [SerializeField] LayerMask canKillPlayer;

    [Header("Death")]
    [SerializeField] Vector2 deathForce = new Vector2(0f, 20f);

    [Header("Shooting")]
    [SerializeField] GameObject bullet;
    [SerializeField] Transform bulletSpawn;

    bool playerHasHorizontalSpeed = false;
    bool playerHasVerticalSpeed = false;
    bool isAlive = true;
    Vector2 moveInput;
    bool isJumping;
    bool isClimbing;
    float gravityAtStart;
    Rigidbody2D rb2d;
    Animator animator;
    CapsuleCollider2D capsuleCollider2D;
    BoxCollider2D boxCollider2D;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        gravityAtStart = rb2d.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) { return; }
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    void Run()
    {
        animator.SetBool("isRunning", playerHasHorizontalSpeed);
        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, rb2d.velocity.y);
        rb2d.velocity = playerVelocity;
    }

    void FlipSprite()
    {
        playerHasHorizontalSpeed = Mathf.Abs(rb2d.velocity.x) > Mathf.Epsilon; //Mathf.Epsilon returns the smaller float value that nears zero;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb2d.velocity.x), 1f);
        }

    }

    void ClimbLadder()
    {
        playerHasVerticalSpeed = Mathf.Abs(rb2d.velocity.y) > Mathf.Epsilon; //Mathf.Epsilon returns the smaller float value that nears zero;
        isClimbing = boxCollider2D.IsTouchingLayers(isClimbable);

        if (isClimbing)
        {
            Vector2 climbingVelocity = new Vector2(rb2d.velocity.x, moveInput.y * climbingSpeed);
            rb2d.velocity = climbingVelocity;
            rb2d.gravityScale = 0f;
            animator.SetBool("isClimbing", playerHasVerticalSpeed);
        }
        else
        {
            rb2d.gravityScale = gravityAtStart;
            animator.SetBool("isClimbing", false);
        }
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) { return; }

        isJumping = !boxCollider2D.IsTouchingLayers(isJumpable);

        if (value.isPressed && !isJumping)
        {
            rb2d.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    void OnFire(InputValue value)
    {
        if (!isAlive) { return; }

        if (value.isPressed)
        {
            Instantiate(bullet, bulletSpawn.position, transform.rotation);
        }
    }

    void Die()
    {
        if (capsuleCollider2D.IsTouchingLayers(canKillPlayer))
        {
            isAlive = false;
            animator.SetTrigger("Dying");
            rb2d.velocity = deathForce;
            capsuleCollider2D.sharedMaterial = null;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }
}
