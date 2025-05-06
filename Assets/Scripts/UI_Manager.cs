using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager instance;          // Singleton instance of UIManager, allowing access to its methods and variables from other scripts.

    // UI elements for different interaction feedbacks and the inventory system.
    public GameObject handCursor;               // Hand icon shown when hovering over interactable objects.
    public GameObject backImage;                // Background image shown during object viewing.
    public TextMeshProUGUI captionText;         // Text field showing item descriptions.
    public Image interactionImage;              // Image displaying associated item visuals.
    public GameObject inventoryImage;           // The entire inventory UI panel.
    public TextMeshProUGUI[] inventoryItems;    // Array of TextMeshProUGUI for showing item names in inventory slots.
    public Image[] itemImage;                   // Array of UI Images to display item icons in the inventory.
    public TextMeshProUGUI infoText;            // Text UI for providing information or feedback to the player.

    // Called when the script instance is being loaded.
    private void Awake()
    {
        instance = this;        // Initialise the instance of the UI_Manager.
    }

    // Called once per frame.
    private void Update()
    {
        // Toggles the inventory UI on or off when the "I" key is pressed.
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryImage.SetActive(!inventoryImage.activeInHierarchy);    // Switch the active state of the inventory UI.
        }
    }

    // Sets the caption text displayed on the UI.
    public void SetCaptionText(string text)
    {
        captionText.text = text;                // Updates the captionText UI element with the provided text.
    }

    // Toggles the visibility of the hand cursor UI element.
    public void SetHandCursor(bool state)
    {
        handCursor.SetActive(state);            // Enable or disable the hand cursor.
    }

    public void SetBackImage(bool state)
    {
        backImage.SetActive(state);             // Enable or disable the background image during item viewing.

        // Hides the interaction image when the back button is not active
        if (!state)
        {
            interactionImage.enabled = false;   
        }
    }

    // Updates the interaction image to display a specific sprite and enables it.
    public void SetImage(Sprite sprite)
    {
        // Update and show the interaction image based on the current item.
        interactionImage.sprite = sprite;
        interactionImage.enabled = true;
    }

    // Updates the inventory slot with the given item's information at the specified index.
    public void SetItems(Item item, int index)
    {
        inventoryItems[index].text = item.collectMessage;   // Sets the inventory slot text to the item's collection message.
        infoText.text = item.collectMessage;                // Displays the same message as temporary feedback to the player.
        itemImage[index].enabled = true;                    // Enables the item image at the given index.
        itemImage[index].sprite = item.itemIcon;            // Sets the image sprite to the item's icon.

        StartCoroutine(FadingText());                       // Starts the coroutine to fade in and out the infoText
    }

    // Fades the feedback text in and out over a short delay.
    IEnumerator FadingText()
    {
        Color newColor = infoText.color;

        // Fade in: Increase the alpha value of the text color from 0 to 1 over time.
        while (newColor.a <1)
        {
            newColor.a += Time.deltaTime;
            infoText.color = newColor;
            yield return null;
        }

        yield return new WaitForSeconds(2f);    // Wait for 2 seconds with the text fully visible

        // Fade out: Decrease the alpha value of the text color from 1 to 0 over time.
        while (newColor.a > 0)
        {
            newColor.a -= Time.deltaTime;
            infoText.color = newColor;
            yield return null;
        }
    }

    // Remove an item from the UI.
    public void RemoveItem(int index)
    {
        if (index < 0 || index >= inventoryItems.Length)
        {
            return;
        }

        inventoryItems[index].text = "";    // Clears item text.
        itemImage[index].enabled = false;   // Hides item image.
        itemImage[index].sprite = null;     // Reset the sprite/
    }

    public void RefreshItems(List<Item> items)
    {
        // Clears the UI inventory and updates it with the new list.
        for (int i = 0;  i < inventoryItems.Length; i++)
        {
            if (i < items.Count)
            {
                inventoryItems[i].text = items[i].collectMessage;
                itemImage[i].enabled = true;
                itemImage[i].sprite = items[i].itemIcon;
            }
            else
            {
                inventoryItems[i].text = "";   
                itemImage[i].enabled = false;   
                itemImage[i].sprite = null;    
            }
        }
    }
}
