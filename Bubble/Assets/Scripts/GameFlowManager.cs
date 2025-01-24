using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    public GameObject instructionsPanel; // The Instructions Panel
    public MonoBehaviour[] scriptsToDisable; // List of scripts (like Enemy AI) to disable when paused

    private bool isGameStarted = false; // Tracks whether the game has started

    private void Start()
    {
        // Show the Instructions Panel at the start
        ShowInstructionsPanel();
    }

    public void ShowInstructionsPanel()
    {
        Time.timeScale = 0f; // Pause the game
        instructionsPanel.SetActive(true); // Show the Instructions Panel
        isGameStarted = false; // Game cannot start while the panel is open

        // Disable scripts like EnemyAI or firing logic
        SetScriptsEnabled(false);
    }

    public void StartGame()
    {
        Time.timeScale = 1f; // Resume the game
        instructionsPanel.SetActive(false); // Hide the Instructions Panel
        isGameStarted = true; // Game has started

        // Re-enable scripts like EnemyAI or firing logic
        SetScriptsEnabled(true);
    }

    private void SetScriptsEnabled(bool enabled)
    {
        foreach (MonoBehaviour script in scriptsToDisable)
        {
            if (script != null)
            {
                script.enabled = enabled;
            }
        }
    }
}
