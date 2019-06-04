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
    public Material swapMaterial;
    public Material oldMaterial;
    public GameObject highlightObject;
    public bool highlightActive;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void AddHighlight(GameObject model)
    {
        //Debug.Log("AddHighLight");
        //if nothing is being highlighted then swap materials
        if (highlightActive == false)
        {
            //Debug.Log("swapping Materials");
            oldMaterial = model.GetComponent<Renderer>().material;
            //set the models material to the proper swapMaterial
            model.GetComponent<Renderer>().material = swapMaterial;
            //set the marker of current highlight active
            highlightObject = model;
            highlightActive = true;
            return;
        }
        if(highlightActive == true)
        {
            if(highlightObject == model)
            {
                //do nothing because the model already highlighted is the model to be highlighted
                //actually this should run a remove Highlight and restore all documents
                RemoveHighlight(highlightObject);
                DocumentsPanel.ResetDocumentsToVisible();
                return;
            }
            if (highlightObject != model)
            {
                //Debug.Log("New model needs highlight");
                RemoveHighlight(highlightObject);
                DocumentsPanel.ResetDocumentsToVisible();
                AddHighlight(model);
            }
        }
    }
    void RemoveHighlight(GameObject model)
    {
        //Debug.Log("RemoveHighLight");
        /*
        if (model.GetComponent<Renderer>().material == null)
        {
            //do nothing because there is no existing material
        }
        if (model.GetComponent<Renderer>().material != null)
        {
            oldMaterial = model.GetComponent<Renderer>().material;
        }
        */
        model.GetComponent<Renderer>().material = oldMaterial;
        highlightActive = false;
        highlightObject = null;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
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

        //for highlighting objects and using their block value to cull the documents
        if (Input.GetKeyDown("a"))
        {
            //Debug.Log("a key was pressed");
            //send a ray from the camera to the current mouse cursor
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out hit)) {
            Transform objectHit = hit.transform;

                //set material to outline
                AddHighlight(objectHit.gameObject);

                if(highlightObject == hit.collider.gameObject)
                {
                    //then wait for a click to actually change the panel
                    //Debug.Log(objectHit.ToString());
                    try
                    {
                        string BlockNumber = objectHit.GetComponent<SpatioModel>().block.ToString();
                        //Debug.Log(BlockNumber);
                        DocumentsPanel.GetComponent<SpatioDocuments>().ToggleButtonDisplay(BlockNumber);

                    }
                    catch
                    {

                    }
                }

                
                return;
            }
            //query the result
            //send the result to the block documents
        }
        //o is keyed to camera movement, that script is on the main scene camera
        if (Input.GetKeyDown("o"))
        {
            //print("o key was pressed");
            //orbit mode
        }

    }
}
