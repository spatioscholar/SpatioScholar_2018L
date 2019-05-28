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

    // Use this for initialization
    void Start()
    {
        Debug.Log("ViewNotesPanel Start Called");
        //loads an instance of the prefab for use
        blurbResource = Resources.Load<GameObject>("Note Blurb");
        Debug.Log(blurbResource.ToString());
    }

    void InitNote()
    {

        //loads an instance of the prefab for use
        blurbResource = Resources.Load<GameObject>("Note Blurb");
        Debug.Log(blurbResource.ToString());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateNotes(JSONNode node, DateTime lastUpdate)
    {
        if(blurbResource == null)
        {
            InitNote();
        }
        //Debug.Log("running UpdateNotes");
        int children = canvas.transform.childCount;
        foreach (JSONNode n in node.AsArray)
        {
            //Debug.Log("running UpdateNotes for each node in the node array "+n);
            DateTime postTime = DateTime.ParseExact(n["posttime"], "yyyy-MM-dd HH:mm:ss", null);
            //Debug.Log("postTime: " + postTime + " lastUpdate: " + lastUpdate);
            if (postTime <= lastUpdate)
                continue;
            GameObject blurb = Instantiate<GameObject>(blurbResource);
            NoteBlurb b = blurb.GetComponent<NoteBlurb>();
            b.Setup(view, n["first"], n["last"], n["brief"],
                    n["full"], n["url"],
                    new Vector3(n["x"], n["y"], n["z"]),
                    postTime,
                    DateTime.ParseExact(n["reftime"], "yyyy-MM-dd HH:mm:ss", null));
            blurb.transform.SetParent(canvas.transform);
            RectTransform rt = blurb.GetComponent<RectTransform>();
            rt.localPosition = new Vector3(5, -5 - rt.sizeDelta.y * children, 0);
            children++;
        }
    }
}

