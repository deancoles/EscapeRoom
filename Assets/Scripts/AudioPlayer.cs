using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles audio playback for interactions and item examination.
public class AudioPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    // Get and store the AudioSource component attached to this GameObject.
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();  
    }

    // Plays a single sound clip once
    public void PlayAudio(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
