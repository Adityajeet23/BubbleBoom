using UnityEngine;

public class PersistentBGM : MonoBehaviour
{
    private static PersistentBGM instance;

    private void Awake()
    {
        // Check if an instance of this script already exists
        if (instance == null)
        {
            instance = this; // Set the instance to this script
            DontDestroyOnLoad(gameObject); // Prevent this GameObject from being destroyed on scene load
            GetComponent<AudioSource>().Play(); // Start playing the BGM
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate BGMManagers
        }
    }
}
