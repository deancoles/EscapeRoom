using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class PreviousItem
{
    public Item requiredItem;
    public Item interactionItem;
    public UnityEvent OnInteract;
}

// Represents objects in the game world that the player can interact with.
public class Interactable : MonoBehaviour
{
    public Item item;               // The item data associated with this interactable object.

    public PreviousItem[] previousItem;
    public UnityEvent onInteract;
    public UnityEvent CollectItem;

    [HideInInspector]
    public bool isMoving;           // Used to prevent interaction while the object is moving.

    

  
}
