using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using static UnityEngine.UIElements.UxmlAttributeDescription;

// This script allows the player to detect and interact with objects.
public class Player_Interaction : MonoBehaviour
{
    // Camera and interaction settings
    private Camera myCam;                       // Reference to the main camera (used to cast rays from the player view).
    public float rayDistance = 2f;              // Distance which the player can interact with objects.
    public float rotateSpeed;                   // Speed at which the player can rotate interactable objects.

    // UnityEvents for interaction flow
    public UnityEvent onView;                   // Event triggered when starting an object view.
    public UnityEvent onFinishView;             // Event triggered when finishing an object view.

    // Interaction state tracking
    private Interactable currentInteractable;   // Currently focused interactable object.
    private Item currentItem;                   // Item being interacted with.
    private bool isViewing;                     // Whether the player is currently examining an object.
    private bool canFinish;                     // Whether the player is allowed to exit the examination.

    // Object viewing transform
    public Transform objectViewer;              // Target position to move objects for close viewing.
    private Vector3 originPosition;             // Original position of the object before viewing.
    private Quaternion originRotation;          // Original rotation of the object before viewing.

    // Audio
    private AudioPlayer audioPlayer;            // Reference to the AudioPlayer for playing sounds.
    private Player_Inventory inventory;         // Reference to Player_Inventory script.
    public AudioClip writingSound;              // The sound that plays when adding an item to inventory.


    private void Awake()
    {
        audioPlayer = GetComponent<AudioPlayer>();      // Find and store the AudioPlayer component on the same GameObject.
        inventory = GetComponent<Player_Inventory>();   // Find and store the PlayerInventory component on the same GameObject.
    }

    void Start()
    {
        myCam = Camera.main;    // Set the Camera reference to the main camera in the scene.
    }

    void Update()
    {
        CheckInteractables();   // Call the function to check for interactable objects in front of the camera.
    }

    // Function to check for interactable objects using a raycast and handle interaction logic.
    void CheckInteractables()
    {
        // If the player is already viewing an interactable object
        if (isViewing)
        {
            // Allow the player to rotate the object if it is marked as grabbable and the left mouse button is held.
            if (currentInteractable.item.grabbable && Input.GetMouseButton(0))
            {
                RotateObject();
            }

            // Allow the player to finish viewing if the right mouse button is pressed and finishing is enabled.
            if (canFinish && Input.GetMouseButtonDown(1))
            {
                FinishView();
            }

            return;
        }

        RaycastHit hit;     // Declare a RaycastHit object to store information about the object hit by the ray.
        Vector3 rayOrigin = myCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.5f));      // Ray origin from centre of camera view..

        // Perform a raycast from the calculated origin in the camera's forward direction, up to the specified rayDistance.
        if (Physics.Raycast(rayOrigin, myCam.transform.forward, out hit, rayDistance))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();      // Check if the object supports interaction.

            // If the hit object is interactable,
            if (interactable != null)
            {
                UI_Manager.instance.SetHandCursor(true);        // Highlight object as interactable.

                // If the left mouse button is clicked.
                if (Input.GetMouseButtonDown(0))
                {
                    // If the object is currently moving, do not allow interaction.
                    if (interactable.isMoving)
                    {
                        return;
                    }

                    // Store interactable and invoke its event.
                    currentInteractable = interactable;
                    currentInteractable.onInteract.Invoke();

                    // If the interactable has an associated item.
                    if (currentInteractable.item != null)
                    {
                        // Invoke the onView UnityEvent and set the isViewing flag to true.
                        onView.Invoke();
                        isViewing = true;

                        bool hasPreviousItem = false;

                        // Loops through interactable's list of previous items
                        for (int i = 0; i < currentInteractable.previousItem.Length; i++)
                        {
                            // Checks if inventory contains required previous item, interact with item and invoke interaction event.
                            if (inventory.items.Contains(currentInteractable.previousItem[i].requiredItem))
                            {
                                // Interact with item and invoke interaction event.
                                Interact(currentInteractable.previousItem[i].interactionItem);
                                currentInteractable.previousItem[i].OnInteract.Invoke();

                                inventory.RemoveItem(currentInteractable.previousItem[i].requiredItem);
                                currentInteractable.item = currentInteractable.previousItem[i].interactionItem;

                                hasPreviousItem = true;
                                break;
                            }
                        }

                        // If the item is grabbable, store its original position and rotation, and move it to the objectViewer.
                        if (currentInteractable.item.grabbable)
                        {
                            originPosition = currentInteractable.transform.position;
                            originRotation = currentInteractable.transform.rotation;
                            StartCoroutine(MovingObject(currentInteractable, objectViewer.position));
                        }

                        // If a previous item interaction has happened, exit function.
                        if (hasPreviousItem)
                        {
                            return;
                        }

                        Interact(currentInteractable.item);     // Interact with current item if no previous item interaction has happened.
                    }
                }

            }

            // If the object is NOT interactable, revert to the default cursor.
            else
            {
                UI_Manager.instance.SetHandCursor(false);
            }
        }
        // If nothing is hit by the raycast, revert to the default cursor.
        else
        {
            UI_Manager.instance.SetHandCursor(false);
        }
    }

    // Handles what happens when interacting with an item.
    void Interact(Item item)
    {
        currentItem = item;     // Track the item being interacted with.              

        // Display item image to UI if it has one.
        if (item.image != null)
        {
            UI_Manager.instance.SetImage(item.image);
        }

        // Play the item's audio clip and set its associated text in the UI.
        audioPlayer.PlayAudio(item.audioClip);
        UI_Manager.instance.SetCaptionText(item.text);

        Invoke("CanFinish", item.audioClip.length + 0.5f);
    }

    // Called after interaction finishes, allowing player to finish viewing an item.
    void CanFinish()
    {
        canFinish = true;

        // Automatically finish viewing if the item has no image and is not grabbable.
        if (currentItem.image == null && !currentItem.grabbable)
        {
            FinishView();
        }

        // Show the 'Back' UI option to finish viewing.
        else
        {
            UI_Manager.instance.SetBackImage(true);
        }

        UI_Manager.instance.SetCaptionText("");
    }

    // Function to finish viewing the current item and reset the viewing state.
    void FinishView()
    {
        // Reset the canFinish and isViewing flags.
        canFinish = false;
        isViewing = false;

        UI_Manager.instance.SetBackImage(false);

        // Add item to player inventory if marked as collectible.
        if (currentItem.inventoryItem)
        {
            inventory.AddItem(currentItem);
            audioPlayer.PlayAudio(writingSound);
            currentInteractable.CollectItem.Invoke();
        }

        // If the item is grabbable, return it to its original position and rotation.
        if (currentItem.grabbable)
        {
            currentInteractable.transform.rotation = originRotation;
            StartCoroutine(MovingObject(currentInteractable, originPosition));
        }

        onFinishView.Invoke();
    }

    // Smoothly moves an object to a target position over time.
    IEnumerator MovingObject(Interactable obj, Vector3 position)
    {
        obj.isMoving = true;
        float timer = 0;

        // Gradually move object to target position over time.
        while (timer < 1)
        {
            obj.transform.position = Vector3.Lerp(obj.transform.position, position, Time.deltaTime * 5);
            timer += Time.deltaTime;
            yield return null;
        }

        obj.transform.position = position;
        obj.isMoving = false;
    }

    // Rotate the interactable object based on mouse input.
    private void RotateObject()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");
        currentInteractable.transform.Rotate(myCam.transform.right, -Mathf.Deg2Rad * y * rotateSpeed, Space.World); // Rotate object around camera's right axis based on vertical mouse movement.
        currentInteractable.transform.Rotate(myCam.transform.up, -Mathf.Deg2Rad * x * rotateSpeed, Space.World);    // Rotate object around camera's up axis based on horizontal movement.
    }
}
