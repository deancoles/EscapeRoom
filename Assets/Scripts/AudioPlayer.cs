using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles audio playback for interactions and item examination.
public class AudioPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();  // Get and store the AudioSource component attached to this GameObject.
    }

    // Plays a single sound clip once
    public void PlayAudio(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
