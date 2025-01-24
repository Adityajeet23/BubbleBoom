using UnityEngine;
using UnityEngine.SceneManagement;

public class TwoPlayerPause : MonoBehaviour
{
    private bool isPaused = false;
    public GameObject panel;
    public TwoPlayerController p1;
    void Update()
    {
        Debug.LogWarning(isPaused);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            p1.enabled = false;
            panel.SetActive(true);
        }
        else
        {
            p1.enabled = true;
            panel.SetActive(false);
        }
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        panel.SetActive(false);
        p1.enabled = true;
    }
}