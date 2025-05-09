using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class PreviousItem
{
    public Item requiredItem;       // Required item for this interaction.
    public Item interactionItem;    // The resulting item after interaction.
    public UnityEvent OnInteract;   // Event triggered when interaction occurs.
}

// Represents objects in the game world that the player can interact with.
public class Interactable : MonoBehaviour
{
    public Item item;                   // The item data associated with this interactable object.
  
    public PreviousItem[] previousItem; // List of conditional interactions depending on items player has collected.
    public UnityEvent onInteract;       // Called when the player interacts with the object and no item requirements are needed.
    public UnityEvent CollectItem;      // Called when the player collects the item.

    [HideInInspector]
    public bool isMoving;   // Prevents interaction while the object is moving.




}
