using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject npcPrefab; // The NPC template to be cloned
    public int count = 20;       // Total number of NPCs to spawn at the start

    [Header("Map Boundaries")]
    public Vector2 minBounds;
    public Vector2 maxBounds;

    [Header("Safety Settings")]
    public float spawnRadius = 0.5f; // Area to check for obstacles
    public LayerMask obstacleLayer;
    public int maxAttempts = 15;     // Maximum tries per NPC to find a free spot

    void Start()
    {
        // Loop to create the specified amount of NPCs
        for (int idx = 0; idx < count; idx++)
        {
            Vector2 randomPos = Vector2.zero;
            bool spawnPointFound = false;
            int attempts = 0;

            // Loop until we find an empty spot or reach max attempts
            while (!spawnPointFound && attempts < maxAttempts)
            {
                randomPos = new Vector2(
                    Random.Range(minBounds.x, maxBounds.x),
                    Random.Range(minBounds.y, maxBounds.y)
                );

                // checks if the 'spawnRadius' overlaps with any collider on 'obstacleLayer'
                Collider2D hit = Physics2D.OverlapCircle(randomPos, spawnRadius, obstacleLayer);
                
                if (hit == null) 
                {
                    spawnPointFound = true;
                }
                attempts++;
            }

            if (spawnPointFound)
            {
                Instantiate(npcPrefab, randomPos, Quaternion.identity);
            }
        }
    }
}