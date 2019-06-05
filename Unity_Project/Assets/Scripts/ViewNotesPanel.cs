using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using SimpleJSON;

public class ViewNotesPanel : MonoBehaviour
{
    public GameObject canvas;
    public NotesPanel view;
    GameObject blurbResource;
    GameObject viewportNote;
    //array to hold notes, better to have a flexible system like a dictionary
    public List<NoteObject> NoteList = new List<NoteObject>();
    public List<GameObject> VisibleNoteObjects;
    public bool NoteVisibility = false;
    public DBLoader dbloader;
    public GameObject manager;

    // Use this for initialization
    void Start()
    {
        //Debug.Log("running start routine for ViewNotesPanel");
        //Assign ViewController object
        manager = GameObject.Find("SpatioManager");
        dbloader = manager.GetComponent<DBLoader>();
    }

    void InitNote()
    {
        //loads an instance of the prefab for use
        if (blurbResource == null)
        {
            blurbResource = Resources.Load<GameObject>("Note Blurb");
        }
        if (viewportNote == null)
        {
            viewportNote = Resources.Load<GameObject>("Note_In_Scene");
        }
    }

    public void ToggleNoteVisibility()
    {
        Debug.Log("ToggleNoteVisibility method called");
        if(NoteVisibility == false)
        {
            ShowNotesInViewport();
            return;
        }
        if (NoteVisibility == true)
        {
            HideNotesInViewport();
            return;
        }
    }
    //This method should be called to turn "on" the display of notes
    //Currently the notes are visible as they are downloaded from the database
    //This is a limiting feature
    public void ShowNotesInViewport()
    {
        Debug.Log("ShowNotesInViewport method called");
        //turn on all notes
        for (int i = 0; i < VisibleNoteObjects.Count; i++)
        {
            VisibleNoteObjects[i].SetActive(true);
        }
        NoteVisibility = true;
    }
    //This method should be called to turn "off" the display of notes
    public void HideNotesInViewport()
    {
        Debug.Log("HideNotesInViewport method called");
        //turn off all notes
        for (int i = 0; i < VisibleNoteObjects.Count; i++)
        {
            VisibleNoteObjects[i].SetActive(false);
        }
        NoteVisibility = false;
    }
    //This is used to remove the notes, through a less than perfect method based on their existing only as elements in the UI of the ViewNotes Panel
    void ClearNotes()
    {
        //clear the Notes Panel
        int children = canvas.transform.childCount;

        //clear the note array
        NoteList.Clear();
        
        for(int i = 0; i < children; i++)
        {

            Destroy(canvas.transform.GetChild(i).gameObject);
            try
            {
               // Destroy(canvas.transform.GetChild(i).gameObject);
            }
            catch(Exception e)
            {

            }
            //Debug.Log("Destroy " + children);
            //Debug.Log(canvas.transform.childCount);
        }
    }

    //making this method to tie button commands to push new updates of the notes
    public void CallUpdateNotes()
    {
        if(dbloader == null)
        {
            //Debug.Log("CallUpdateNotes");
            //Assign ViewController object
            manager = GameObject.Find("SpatioManager");
            dbloader = manager.GetComponent<DBLoader>();
        }
        dbloader.StartNoteDownload();
    }

    //called from DBLoader to update the contents of this panel
    public void UpdateNotes(JSONNode node, DateTime lastUpdate)
    {
        int NoteCount = 0;
        //runs the init command
        InitNote();
        
        //clear notes in the panel
        //ClearNotes();
        
        //Debug.Log("running UpdateNotes");
        int children = canvas.transform.childCount;
        //Debug.Log(children);
        foreach (JSONNode n in node.AsArray)
        {
            //Debug.Log("running UpdateNotes for each node in the node array "+n);
            DateTime postTime = DateTime.ParseExact(n["posttime"], "yyyy-MM-dd HH:mm:ss", null);
            //if (postTime <= lastUpdate)
            if (postTime >= lastUpdate)
            {
                //Debug.Log(postTime + " <= " + lastUpdate);
                continue;
            }
            GameObject blurb = Instantiate<GameObject>(blurbResource);
            NoteBlurb b = blurb.GetComponent<NoteBlurb>();
            b.Setup(view, n["first"], n["last"], n["brief"], n["full"], n["url"], new Vector3(n["x"], n["y"], n["z"]),
                    postTime,
                    DateTime.ParseExact(n["reftime"], "yyyy-MM-dd HH:mm:ss", null));

            //NoteList[children] = new NoteObject((new Vector3(n["x"], n["y"], n["z"])), (n["brief"]));


            //Add viewport note in the scene
            GameObject Note = Instantiate<GameObject>(viewportNote);
            Note.transform.position = ((new Vector3(n["x"], n["y"], n["z"])));
            //Populate vieport note text fields with the proper info from the DB
            Note.GetComponent<SpatioNote>().brief = (n["brief"]);
            Note.GetComponent<SpatioNote>().textFieldBrief.text = ("Brief Description: " + (n["brief"]));
            Note.GetComponent<SpatioNote>().name = (n["last"]);
            Note.GetComponent<SpatioNote>().textFieldName.text = ("Name: " + (n["last"]));
            Note.GetComponent<SpatioNote>().full = (n["full"]);
            Note.GetComponent<SpatioNote>().textFieldFull.text = ("Full Description: " + (n["full"]));
            Note.GetComponent<SpatioNote>().timestamp = postTime.ToString();
            Note.GetComponent<SpatioNote>().textFieldTimeStamp.text = ("Note Time: " + postTime.ToString());


            //Debug.Log("adding the latest visual note object to the VisibleNoteObjects List");
            VisibleNoteObjects.Add(Note);
            Note.SetActive(false);
            NoteCount++;
            //Debug.Log("added note" + Note.GetComponent<SpatioNote>().brief);

            blurb.transform.SetParent(canvas.transform);
            //Debug.Log("yes");
            RectTransform rt = blurb.GetComponent<RectTransform>();
            rt.localPosition = new Vector3(5, -5 - rt.sizeDelta.y * children, 0);
            //update child count number to track the number of notes in the panel
            children++;
        }
    }
}

