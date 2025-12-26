using UnityEngine;

public class Fountain : MonoBehaviour
{
    public Sprite humanSprite; // Sprite to be applied when a zombie is cured

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Object.FindAnyObjectByType<GameManager>().GameOver("The holy water burned away your infection. Mission failed.");
        }

        // Check if the object is an NPC and if it is currently a zombie
        NPCMovement npc = other.GetComponent<NPCMovement>();
        if (npc != null && npc.isZombie)
        {
            // Revert the zombie back to its human
            npc.BecomeHuman(humanSprite);
        }
    }
}