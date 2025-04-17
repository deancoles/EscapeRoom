using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    public bool grabbable;
    
    public AudioClip audioClip;

    [TextArea(4,1)]
    public string text;
}
