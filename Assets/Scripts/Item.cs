using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Defines the properties of an item that can be interacted with.
[CreateAssetMenu]
public class Item : ScriptableObject
{
    public bool grabbable;              // Whether the player can grab and manipulate this item.
    public AudioClip audioClip;         // Audio to play when the item is examined.

    [TextArea(4,1)]
    public string text;                 // Descriptive text shown during interaction.
    
    public Sprite image;                // Image displayed in the UI when interacting with the item.

    [Header("Inventory")]
    public bool inventoryItem;
    public string collectMessage;
    public Sprite itemIcon;
}
