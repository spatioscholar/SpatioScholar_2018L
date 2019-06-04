using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is tied to the Model Information Panel
//This primarily will handle drawing a line in 3d space in the direction of the 3d model and when clicked return the information of that model object
public class SScholar_ModelInformationPanel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //do nothing.
    }

    // Update is called once per frame
    void Update()
    {
        //do nothing
    }

    //This turns on a raydrawing function in the direction of the mouse coordinates transformed into world coordinates
    void InitiatePointer()
    {

    }

    //This method will be called when the Mouse is Clicked in the Pointer mode
    void PointerClicked()
    {
        //do nothing
        //gather information on the model component clicked
        //tell the text field above to display information on that model
        //tell the documents panel to only show documents relating to that Block (override available in the Documents Panel)
    }
}
