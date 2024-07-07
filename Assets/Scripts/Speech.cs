using UnityEngine;

public class MicrophoneTest : MonoBehaviour
{
    void Start()
    {
        // Check if a microphone is available
        if (Microphone.devices.Length > 0)
        {
            Debug.Log("Microphone detected.");

            // Create a new AudioSource component if not already attached
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = Microphone.Start(null, true, 10, 44100);
            audioSource.loop = true;

            // Wait until the recording has started
            while (!(Microphone.GetPosition(null) > 0)) { }
            Debug.Log("Microphone recording started.");

            // Play the audio source
            audioSource.Play();
        }
        else
        {
            Debug.Log("No microphone detected.");
        }
    }
}
