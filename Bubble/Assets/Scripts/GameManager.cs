using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject youLosePanel; // Reference to the "You Lose" panel
    public GameObject youWinPanel;  // Reference to the "You Win" panel

    [Header("Game Objects")]
    public GameObject player; // Reference to the Player GameObject
    public GameObject enemy;  // Reference to the Enemy GameObject

    private bool isGameOver = false; // Prevent multiple triggers

    void Update()
    {
        // Check if the player or enemy is destroyed
        if (!isGameOver)
        {
            CheckGameStatus();
        }
    }

    void CheckGameStatus()
    {
        // Check if the player is destroyed
        if (player == null)
        {
            StartCoroutine(ShowYouLosePanel());
            ShowYouLosePanel();
        }

        // Check if the enemy is destroyed
        if (enemy == null)
        {
            StartCoroutine(ShowYouWinPanel());
            ShowYouWinPanel();
        }
    }

    IEnumerator ShowYouLosePanel()
    {
        yield return new WaitForSeconds(1);
        isGameOver = true; // Prevent further triggers
        youLosePanel.SetActive(true); // Show "You Lose" panel
        Time.timeScale = 0; // Pause the game
        Debug.Log("You Lose!");
    }

    IEnumerator ShowYouWinPanel()
    {
        yield return new WaitForSeconds(1);
        isGameOver = true; // Prevent further triggers
        youWinPanel.SetActive(true); // Show "You Win" panel
        Time.timeScale = 0; // Pause the game
        Debug.Log("You Win!");
    }
}
