using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is where general keystroke watchers go. To centralize the watching of Keys primarily for UI UX management
public class Spatio_KeyStroke_Manager : MonoBehaviour
{
    public GameObject Instructions;
    public Canvas DocumentsTab;
    public Canvas GeneralTab;
    public Camera camera;
    public SpatioDocuments DocumentsPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            //print("space key was pressed");
            if(Instructions.active == false)
            {
                Instructions.SetActive(true);
            }
            else 
                if(Instructions.active == true)
            {
                Instructions.SetActive(false);
            }
            //Hide both UI tabs?
            //show Instructions Panel
        }
        if (Input.GetKeyDown("a"))
        {
            //print("a key was pressed");
            //send a ray from the camera to the current mouse cursor
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out hit)) {
            Transform objectHit = hit.transform;
                //Debug.Log(objectHit.ToString());
                //Debug.Log(objectHit.GetComponent<SpatioModel>().block.ToString());
                DocumentsPanel.GetComponent<SpatioDocuments>().ToggleButtonDisplay(objectHit.GetComponent<SpatioModel>().block.ToString());
            }
            //query the result
            //send the result to the block documents
        }
        if (Input.GetKeyDown("o"))
        {
            //print("o key was pressed");
            //orbit mode
        }

    }
}
