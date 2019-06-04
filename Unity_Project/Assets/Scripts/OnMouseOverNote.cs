using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMouseOverNote : MonoBehaviour
{
    void OnMouseOver()
    {
        //If your mouse hovers over the GameObject with the script attached, output this message
        
    }

    void OnMouseEnter()
    {
        Debug.Log("Mouse is over GameObject.");
        Debug.Log(gameObject.GetComponent<SpatioNote>().brief);
    }

    void OnMouseExit()
    {
        //The mouse is no longer hovering over the GameObject so output this message each frame
        //Debug.Log("Mouse is no longer on GameObject.");
    }
}
