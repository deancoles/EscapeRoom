using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.UIElements.UxmlAttributeDescription;

// This script allows the player to detect and interact with objects.
public class Player_Interaction : MonoBehaviour
{
    private Camera myCam;                           // Reference to the main camera (used to cast rays from the player view).
    public float rayDistance = 2f;                  // Distance which the player can interact with objects.
    public float rotateSpeed;                       // Speed at which the player can rotate grabbed objects.
    public UnityEvent onView;                       // Event triggered when starting an object view.
    public UnityEvent onFinishView;                 // Event triggered when finishing an object view.
    private Interactable currentInteractable;       // Currently focused interactable object.
    private bool isViewing;                         // Whether the player is currently examining an object.
    private bool canFinish;                         // Whether the player is allowed to exit the examination.
    public Transform objectViewer;                  // Target position to move objects for close viewing.
    private Vector3 originPosition;                 // Original position of the object before viewing.
    private Quaternion originRotation;              // Original rotation of the object before viewing.
    private AudioPlayer audioPlayer;                // Reference to the AudioPlayer for playing sounds.


    private void Awake()
    {
        audioPlayer = GetComponent<AudioPlayer>();  // Get reference to the AudioPlayer component.
    }

    void Start()
    {
        myCam = Camera.main;                        // Find and store the Main Camera at the start.
    }

    void Update()
    {
        CheckInteractables();                       // Check for interactable objects every frame.
    }

    // Casts a ray to detect interactable objects and handles interaction logic.
    void CheckInteractables()
    {
        if(isViewing)
        {
            if(currentInteractable.item.grabbable && Input.GetMouseButton(0))
            {
                RotateObject();
            }

            if(canFinish && Input.GetMouseButtonDown(1))
            {
                FinishView();
            }
            return;
        }

        RaycastHit hit;                                                                 // Variable to store information about what the ray hits.
        Vector3 rayOrigin = myCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.5f));  // Define the origin point of the ray — centre of the screen (viewport 0.5, 0.5).

        // Cast a ray forward from the camera
        if (Physics.Raycast(rayOrigin,myCam.transform.forward,out hit,rayDistance))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();      // Check if the object hit has an 'Interactable' component.

            if (interactable !=null)
            {
                UI_Manager.instance.SetHandCursor(true);                                // If the object is interactable, set the UI cursor to a 'hand' icon.
                if (Input.GetMouseButtonDown(0))
                {
                    if(interactable.isMoving)
                    {
                        return;
                    }

                    onView.Invoke();
                    currentInteractable = interactable;
                    isViewing = true;
                    Interact(currentInteractable.item);

                    if(currentInteractable.item.grabbable)
                    {
                        originPosition = currentInteractable.transform.position;
                        originRotation = currentInteractable.transform.rotation;
                        StartCoroutine(MovingObject(currentInteractable, objectViewer.position));
                    }
                } 
                
            }
            else
            {
                UI_Manager.instance.SetHandCursor(false);                               // If the object is NOT interactable, revert to the default cursor.
            }
        }
        else
        {
            UI_Manager.instance.SetHandCursor(false);                                   // If nothing is hit by the raycast, revert to the default cursor.
        }
    }

    // Handles what happens when interacting with an item.
    void Interact(Item item)
    {
        if(item.image != null) 
            {
            UI_Manager.instance.SetImage(item.image);
            }

        audioPlayer.PlayAudio(item.audioClip);
        UI_Manager.instance.SetCaptionText(item.text);
        Invoke("CanFinish",item.audioClip.length +0.5f);
    }

    // Called after interaction finishes, allowing player to exit.
    void CanFinish()
    {
        canFinish = true;
        UI_Manager.instance.SetBackImage(true);
        UI_Manager.instance.SetCaptionText("");
    }

    // Handles logic when player chooses to finish viewing an object.
    void FinishView()
    {
        canFinish = false;
        isViewing = false;
        UI_Manager.instance.SetBackImage(false);  
        if(currentInteractable.item.grabbable)
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
        while (timer < 1)
        {
            obj.transform.position = Vector3.Lerp(obj.transform.position, position, Time.deltaTime * 5);
            timer += Time.deltaTime;
            yield return null;
        }

        obj.transform.position = position;
        obj.isMoving = false;
    }

    // Allows player to rotate the object while examining it.
    private void RotateObject()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");
        currentInteractable.transform.Rotate(myCam.transform.right, -Mathf.Deg2Rad * y * rotateSpeed, Space.World);
        currentInteractable.transform.Rotate(myCam.transform.up, -Mathf.Deg2Rad * x * rotateSpeed, Space.World);
    }
}
