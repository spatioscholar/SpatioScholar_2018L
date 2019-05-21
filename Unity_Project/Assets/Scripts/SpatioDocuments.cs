using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SpatioDocuments : MonoBehaviour {

    public Toggle BlockAToggle;
    public SpatioDocuments DocumentsPanel;
    public GameObject temporaryNull;

    //this List, which is used to display documents should be populated with only the SpatioAssets keyed to the active "Block"
    List<SpatioButton> buttons;
    bool changed;


	// Use this for initialization
	void Start () {
        //checks the List holding SpatioButtons and generates if the reference is null
        if (buttons == null)
            buttons = new List<SpatioButton>();
    }

    public void ToggleButtonDisplay()
    {
        foreach (SpatioButton b in buttons)
        {
            if (BlockAToggle.isOn == false)
            {
                //b.gameObject.SetActive(false);
                //b.transform.SetParent(null, false);
                b.transform.SetParent(temporaryNull.transform, false);
                //continue;
            }
            if (BlockAToggle.isOn == true)
            {
                Debug.Log(b.ToString());
                //b.gameObject.SetActive(true);
                b.transform.SetParent(DocumentsPanel.transform, false);
                //continue;
            }
        }
        changed = true;
    }
	
    //What does this function do? 
    void UpdateButtons()
    {
        int count = 0;
        //for each button in the List
        foreach (SpatioButton b in buttons)
        {
            //if the button and associated imagepanel are not active then....increment count and continue
            if (b.gameObject.active == false) { 
                count++;
                continue;
            }
            //check what this does - Might be making the double columns?
            RectTransform r = b.GetComponent<RectTransform>();
            if (count % 2 == 0)
            {
                r.offsetMin = new Vector2(-90, -125 - count / 2 * 75);
                r.offsetMax = new Vector2(0, -50 - count / 2 * 75);
            }
            else
            {
                r.offsetMin = new Vector2(0, -125 - count / 2 * 75);
                r.offsetMax = new Vector2(90, -50 - count / 2 * 75);
            }
            count++;
        }
    }

    //adds a SpatioButton - really a document reference into the List/Panel
    public void AddButton(SpatioButton b)
    {
        if (buttons == null)
            buttons = new List<SpatioButton>();
        buttons.Add(b);
        changed = true;
    }
    
    //removes a SpatioButton from the buttons display set
    public void RemoveButton(SpatioButton b)
    {
        if (buttons == null)
            buttons = new List<SpatioButton>();
        buttons.Remove(b);
        changed = true;
    }

    // Update is called once per frame
    //if a new SpatioButton has been added changed = true, then run the update buttons script.
    void Update () {
        if (changed)
        {
            UpdateButtons();
            changed = false;
        }
	}
}
