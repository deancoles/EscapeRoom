using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager instance;
    public GameObject handCursor;
    public GameObject backImage;

    private void Awake()
    {
        instance = this;
    }
    
    

    public void SetHandCursor(bool state)
    {
        handCursor.SetActive(state);
    }

    public void SetBackImage(bool state)
    {
        backImage.SetActive(state);
    }
}
