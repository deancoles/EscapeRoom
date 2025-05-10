using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private AudioSource audios;
    public AudioClip clickSFX;

    void Start()
    {
        audios = GetComponent<AudioSource>();

        // Show the cursor and unlock it on the main menu
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
    }

    // Plays the button click sound and loads the first playable scene.
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
        audios.PlayOneShot(clickSFX);
    }

    // Exits the game and plays a sound. Note: this won't work in the Unity Editor.
    public void OnApplicationQuit()
    {
        Application.Quit();
        audios.PlayOneShot(clickSFX);
    }

}
