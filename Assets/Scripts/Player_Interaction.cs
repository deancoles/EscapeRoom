using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.UIElements.UxmlAttributeDescription;

// This script allows the player to detect and interact with objects by hovering the mouse over them.
public class Player_Interaction : MonoBehaviour
{
    private Camera myCam;           // Reference to the main camera (used to cast rays from the player view)
    public float rayDistance = 2f;  // Distance which the player can interact with objects
    public float rotateSpeed;
    public UnityEvent onView;
    public UnityEvent onFinishView;
    private Interactable currentInteractable;
    private bool isViewing;
    private bool canFinish;
    public Transform objectViewer;
    private Vector3 originPosition;
    private Quaternion originRotation;

    // Start is called once at the very beginning of the game
    void Start()
    {
        myCam = Camera.main;        // Find and store a reference to the Main Camera at the start
    }

    // Update is called once per frame
    void Update()
    {
        CheckInteractables();       // Continuously check if player is aiming at an interactable object
    }

    // Function to check if the player is looking at an interactable object
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

        RaycastHit hit; // Variable to store information about what the ray hits
        Vector3 rayOrigin = myCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.5f));  // Define the origin point of the ray — centre of the screen (viewport 0.5, 0.5)

        // Cast a ray forward from the camera
        if (Physics.Raycast(rayOrigin,myCam.transform.forward,out hit,rayDistance))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();      // Check if the object hit has an 'Interactable' component

            if (interactable !=null)
            {
                UI_Manager.instance.SetHandCursor(true);        // If the object is interactable, set the UI cursor to a 'hand' icon
                if (Input.GetMouseButtonDown(0))
                {
                    if(interactable.isMoving)
                    {
                        return;
                    }

                    onView.Invoke();
                    currentInteractable = interactable;

                    isViewing = true;
                    Invoke("CanFinish", 1f);

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
                UI_Manager.instance.SetHandCursor(false);       // If the object is NOT interactable, revert to the default cursor
            }
        }
        else
        {
            UI_Manager.instance.SetHandCursor(false);           // If nothing is hit by the raycast, revert to the default cursor
        }
    }

    void CanFinish()
    {
        canFinish = true;
        UI_Manager.instance.SetBackImage(true);
    }

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

    private void RotateObject()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");
        currentInteractable.transform.Rotate(myCam.transform.right, -Mathf.Deg2Rad * y * rotateSpeed, Space.World);
        currentInteractable.transform.Rotate(myCam.transform.up, -Mathf.Deg2Rad * x * rotateSpeed, Space.World);
    }
}
