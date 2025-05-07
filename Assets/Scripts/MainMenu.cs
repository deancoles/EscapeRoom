using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private AudioSource audios;
    public AudioClip clickSFX;

    // Start is called before the first frame update
    void Start()
    {
        audios = GetComponent<AudioSource>();

    }

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
        audios.PlayOneShot(clickSFX);
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
        audios.PlayOneShot(clickSFX);
    }

}
