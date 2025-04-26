using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager instance;
    public GameObject handCursor;
    public GameObject backImage;
    public TextMeshProUGUI captionText;

    private void Awake()
    {
        instance = this;
    }
    
    public void SetCaptionText(string text)
    {
        captionText.text = text;
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
