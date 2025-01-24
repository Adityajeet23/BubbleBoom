using UnityEngine;

public class ButtonSoundManager : MonoBehaviour
{
    public AudioClip buttonClickSound; // The sound to play when a button is clicked
    private AudioSource audioSource;  // The AudioSource component

    private void Start()
    {
        // Add an AudioSource component to the GameObject if it doesn't already exist
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // Ensure the sound doesn't play on start
        audioSource.clip = buttonClickSound; // Assign the sound clip
    }

    // Method to play the sound
    public void PlaySound()
    {
        if (buttonClickSound != null)
        {
            audioSource.Play(); // Play the sound
        }
        else
        {
            Debug.LogWarning("No sound clip assigned for the button!");
        }
    }
}
