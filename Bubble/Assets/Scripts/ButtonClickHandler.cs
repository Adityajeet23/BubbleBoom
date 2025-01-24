using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonClickHandler : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("The index of the scene to load.")]
    public int sceneIndex; // Scene index to be loaded

    // Method to load the specified scene


    public void LoadScene()
    {
        // Load the scene based on the index specified in the Inspector
        if (sceneIndex >= 0 && sceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(sceneIndex);
            Time.timeScale = 1.0f;
        }
        else
        {
            Debug.LogError("Invalid scene index! Please ensure the index is valid and added to Build Settings.");
        }
    }
}
