using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

// This script allows the player to detect and interact with objects by hovering the mouse over them.
public class Player_Interaction : MonoBehaviour
{
    private Camera myCam;           // Reference to the main camera (used to cast rays from the player view)
    public float rayDistance = 2f;  // Distance which the player can interact with objects

    // Start is called before the first frame update
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
        RaycastHit hit; // Variable to store information about what the ray hits
        Vector3 rayOrigin = myCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.5f));  // Define the origin point of the ray — centre of the screen (viewport 0.5, 0.5)

        // Cast a ray forward from the camera
        if (Physics.Raycast(rayOrigin,myCam.transform.forward,out hit,rayDistance))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();      // Check if the object hit has an 'Interactable' component

            if (interactable !=null)
            {
                UI_Manager.instance.SetHandCursor(true);        // If the object is interactable, set the UI cursor to a 'hand' icon
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
}
