using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager instance;          // Instance for easy access by other scripts.
    public GameObject handCursor;               // Hand icon shown when hovering over interactable objects.
    public GameObject backImage;                // Background image shown during object viewing.
    public TextMeshProUGUI captionText;         // Text field showing item descriptions.
    public Image interactionImage;              // Image displaying associated item visuals.

    private void Awake()
    {
        instance = this;                        // Initialise the instance of the UI_Manager.
    }
    
    public void SetCaptionText(string text)
    {
        captionText.text = text;                // Update the caption text displayed on screen.
    }

    public void SetHandCursor(bool state)
    {
        handCursor.SetActive(state);            // Enable or disable the hand cursor.
    }

    public void SetBackImage(bool state)
    {
        backImage.SetActive(state);             // Enable or disable the background image during item viewing.

        // Hide the interaction image if background is disabled.
        if (!state)
        {
            interactionImage.enabled = false;
        }
    }

    public void SetImage(Sprite sprite)
    {
        // Update and show the interaction image based on the current item.
        interactionImage.sprite = sprite;
        interactionImage.enabled = true;
    }
}
