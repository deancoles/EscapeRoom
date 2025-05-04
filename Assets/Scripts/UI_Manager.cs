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
    public GameObject inventoryImage;
    public TextMeshProUGUI[] inventoryItems;
    public TextMeshProUGUI infoText;

    private void Awake()
    {
        instance = this;                        // Initialise the instance of the UI_Manager.
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryImage.SetActive(!inventoryImage.activeInHierarchy);
        }
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

    public void SetItems(Item item, int index)
    {
        inventoryItems[index].text = item.collectMessage;
        infoText.text = item.collectMessage;
        StartCoroutine(FadingText());
    }

    IEnumerator FadingText()
    {
        Color newColor = infoText.color;
        while (newColor.a <1)
        {
            newColor.a += Time.deltaTime;
            infoText.color = newColor;
            yield return null;
        }

        yield return new WaitForSeconds(2f);

        while (newColor.a > 0)
        {
            newColor.a -= Time.deltaTime;
            infoText.color = newColor;
            yield return null;
        }
    }
}
