using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 2f;
    public bool isZombie = false; // Tracks if the NPC is currently infected
    
    [Header("Sprites")]
    public Sprite zombieSprite;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector2 currentDirection;

    [Header("Anti-Stuck Logic")]
    private Vector2 lastPosition;
    private float stuckCheckTimer;
    private float stuckThreshold = 0.2f; // Time interval to check if the NPC moved
    private float minMovement = 0.05f; // Minimum distance required to not be considered "stuck"

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        
        // Setup physics for top-down movement
        rb.gravityScale = 0;
        rb.freezeRotation = true;

        if (isZombie) 
        {
            BecomeZombie();
        }
        else 
        {
            SetRandomDirection();
        }
        lastPosition = transform.position;
    }

    void FixedUpdate()
    {
        // Decide behavior based on state
        if (isZombie) ChaseHuman();
        else Wander();

        CheckIfStuck();
    }

    // Move in the current random direction
    void Wander()
    {
        rb.velocity = currentDirection * speed;
    }

    // Logic for zombie AI to find and follow the nearest human
    void ChaseHuman()
    {
        NPCMovement[] allNPCs = Object.FindObjectsByType<NPCMovement>(FindObjectsSortMode.None);
        float closestDistance = Mathf.Infinity;
        Transform target = null;

        foreach (NPCMovement npc in allNPCs)
        {
            // Only target those who are still human
            if (!npc.isZombie)
            {
                float dist = Vector2.Distance(transform.position, npc.transform.position);
                if (dist < closestDistance) { closestDistance = dist; target = npc.transform; }
            }
        }

        // Move towards target if found, otherwise wander
        if (target != null)
            rb.velocity = (target.position - transform.position).normalized * speed;
        else 
            Wander();
    }

    // Prevents NPCs from walking into walls forever
    void CheckIfStuck()
    {
        stuckCheckTimer += Time.fixedDeltaTime;
        if (stuckCheckTimer >= stuckThreshold)
        {
            // If movement is too small, pick a new direction
            if (Vector2.Distance(transform.position, lastPosition) < minMovement) SetRandomDirection();
            lastPosition = transform.position;
            stuckCheckTimer = 0;
        }
    }

    // Converts a human NPC into a zombie
    public void BecomeZombie()
    {
        if (isZombie) return;

        isZombie = true;
        
        if (zombieSprite != null)
        {
            sr.sprite = zombieSprite;
        }

        speed *= 1.1f;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        NPCMovement other = collision.gameObject.GetComponent<NPCMovement>();

        // Spread the infection if a zombie touches a human
        if (isZombie && other != null && !other.isZombie)
        {
            other.BecomeZombie();
        }
        
        if (!isZombie) SetRandomDirection();
    }

    // Generates a random normalized vector for movement
    void SetRandomDirection()
    {
        currentDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        if (currentDirection == Vector2.zero) currentDirection = Vector2.right;
    }

    // Reverts a zombie back to human state (used by the fountain)
    public void BecomeHuman(Sprite humanSprite)
    {
        if (!isZombie) return;

        isZombie = false;
    
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        sr.sprite = humanSprite;

        speed /= 1.1f;
    }
}