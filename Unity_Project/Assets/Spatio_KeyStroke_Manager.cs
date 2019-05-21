using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is where general keystroke watchers go. To centralize the watching of Keys primarily for UI UX management
public class Spatio_KeyStroke_Manager : MonoBehaviour
{
    public GameObject Instructions;
    public Canvas DocumentsTab;
    public Canvas GeneralTab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            print("space key was pressed");
            Instructions.SetActive(true);
            //Hide both UI tabs?
            //show Instructions Panel
        }
        if (Input.GetKeyDown("a"))
        {
            print("a key was pressed");
            //send a ray from the camera to the current mouse cursor
            //query the result
            //send the result to the block documents
        }

    }
}
