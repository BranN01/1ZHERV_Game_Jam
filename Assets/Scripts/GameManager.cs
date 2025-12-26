using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    public float timeLimit = 30f; // Total time allowed to finish the objective
    public TextMeshProUGUI timerText; // UI element to display remaining time
    public TextMeshProUGUI counterText; // UI element to display remaining humans

    private int totalHumans;
    private int infectedCount;
    private bool gameEnded = false;

    void Update()
    {
        if (gameEnded) return;

        // Manage countdown timer
        if (timeLimit > 0)
        {
            timeLimit -= Time.deltaTime;
            timerText.text = "Time: " + Mathf.Ceil(timeLimit).ToString() + "s";
        }
        else
        {
            // Time ran out, trigger lose state
            GameOver("Out of time. The world remains safe from your plague.");
        }

        UpdateCounts();
    }

    void UpdateCounts()
    {
        // Find all NPCs in the scene to check their current state
        NPCMovement[] allNPCs = Object.FindObjectsByType<NPCMovement>(FindObjectsSortMode.None);
        int humansLeft = 0;
        infectedCount = 0;

        foreach (var npc in allNPCs)
        {
            if (npc.isZombie) infectedCount++;
            else humansLeft++;
        }

        counterText.text = "People: " + humansLeft;

        // If no humans are left, the player wins
        if (humansLeft == 0)
        {
            WinGame();
        }
    }

    public void GameOver(string message)
    {
        if (gameEnded) return;
        gameEnded = true;
    
        GameStateController controller = Object.FindAnyObjectByType<GameStateController>();
        if (controller != null)
        {
            controller.ShowEndMenu(message, new Color(0.7f, 0.1f, 0.1f));
        }
    }

    void WinGame()
    {
        if (gameEnded) return;
        gameEnded = true;

        GameStateController controller = Object.FindAnyObjectByType<GameStateController>();
        if (controller != null)
        {
            // Trigger win screen
            controller.ShowEndMenu("No more pulse, no more breath. You have conquered the world. Victory!", new Color(0.1f, 0.5f, 0.1f));
        }
    }

    public void RestartGame()
    {
        // Reset time scale to normal before reloading the scene
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}