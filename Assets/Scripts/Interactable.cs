using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Represents objects in the game world that the player can interact with.
public class Interactable : MonoBehaviour
{
    public Item item;               // The item data associated with this interactable object.
    [HideInInspector]
    public bool isMoving;           // Used to prevent interaction while the object is moving.
    public UnityEvent onInteract;

  
}
