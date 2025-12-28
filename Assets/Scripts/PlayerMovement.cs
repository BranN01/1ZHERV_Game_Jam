using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [Header("Movement Settings")]
    public float speed = 5f;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator anim;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        // Ensure top-down physics settings
        rb.gravityScale = 0;
        rb.freezeRotation = true; // Prevent the player from spinning after collisions
    }

    void Update() 
    {
        // Capture movement input from WASD or Arrow keys
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        // Flip the sprite based on the direction of horizontal movement
        if (moveInput.x > 0) spriteRenderer.flipX = true;
        else if (moveInput.x < 0) spriteRenderer.flipX = false;
    }

    void FixedUpdate()
    {
        // Apply movement physics
        // .normalized ensures the player doesn't move faster diagonally
        rb.velocity = moveInput.normalized * speed;

        if (anim != null) {
            anim.SetFloat("Speed", rb.velocity.magnitude);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Logic for infecting humans upon touch
        NPCMovement npc = collision.gameObject.GetComponent<NPCMovement>();

        // If the object is an NPC and it's not already a zombie, infect it
        if (npc != null && !npc.isZombie) {
            npc.BecomeZombie();
        }
    }
}