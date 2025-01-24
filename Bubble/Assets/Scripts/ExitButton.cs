using UnityEngine;

public class ExitButton : MonoBehaviour
{
    // Method to close the game
    public void ExitGame()
    {
        Debug.Log("Game is exiting..."); // This will only be visible in the editor
        Application.Quit(); // Quits the application
    }
}
