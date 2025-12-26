using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameStateController : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject pauseMenuPanel;
    public GameObject gameHUD;
    public GameObject endMenuPanel;

    [Header("Text Settings")]
    public TextMeshProUGUI endMenuStatusText;
    
    private bool isPaused = false;

    void Start()
    {
        // Freeze time and show the main menu when the game starts
        Time.timeScale = 0f;
        mainMenuPanel.SetActive(true);
        pauseMenuPanel.SetActive(false);
        gameHUD.SetActive(false);
    }

    void Update()
    {
        // Toggle pause when Escape is pressed (only if not in the main menu)
        if (Input.GetKeyDown(KeyCode.Escape) && !mainMenuPanel.activeSelf)
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    // Called by the Start Button in the Main Menu
    public void StartGame()
    {
        mainMenuPanel.SetActive(false);
        gameHUD.SetActive(true);
        Time.timeScale = 1f;
    }

    void PauseGame()
    {
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
    
    public void ExitGame()
    {
        Application.Quit();
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Displays the final result screen (Win or Loss)
    public void ShowEndMenu(string message, Color textColor)
    {
        endMenuPanel.SetActive(true);
        Time.timeScale = 0f;

        if (endMenuStatusText != null)
        {
            endMenuStatusText.text = message;
            endMenuStatusText.color = textColor; // Set dynamic color
        }
    }
}