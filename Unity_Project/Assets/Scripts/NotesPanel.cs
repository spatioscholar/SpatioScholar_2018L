using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotesPanel : MonoBehaviour {
    public Vector3 dragStart;
    public Vector3 mouseStart;
    public JumpToPoint point;
    public ViewController view;
    public Slider timeSlider;
    public GameObject fpsController;
    public InputField firstField;
    public InputField lastField;
    public InputField briefField;
    public InputField fullField;
    public InputField URLField;
    public InputField monthField;
    public InputField dayField;
    public InputField yearField;
    public InputField xField;
    public InputField yField;
    public InputField zField;
    public Text posttimeText;
    SpatioManager manager;
    public Vector3 PickLocation;
    GameObject viewportLocation;
    int eraseCounter = 0;
    GameObject LocationMarker;

    // Use this for initialization
    void Start () {
        //assign the SpatioManager automatically for system level method calls
        manager = FindObjectOfType<SpatioManager>();
	}
	
	// Update is called once per frame
	void Update () {
   
        if (eraseCounter == 10)
        {
            eraseCounter = 0;
            try {
                LocationMarker = null;
            }
            catch
            {

            }
        }
        eraseCounter++;
   	}

    public void BeginDrag()
    {
        mouseStart = Input.mousePosition;
        dragStart = transform.position;
    }

    public void Drag()
    {
        transform.position = dragStart + Input.mousePosition - mouseStart;
    }

    //this calls the routine that adds a new note to the database through the SpatioManager
    public void AddNote()
    {
        
        int Day, Month, Year;
        if ((Day = int.Parse(dayField.text)) < 1 || Day > 31)
            return;
        if ((Month = int.Parse(monthField.text)) < 1 || Month > 12)
            return;
        Year = int.Parse(yearField.text);
            
        System.DateTime date = new System.DateTime(Year, Month, Day);
        Vector3 location = new Vector3(float.Parse(xField.text), float.Parse(yField.text), float.Parse(zField.text));
        manager.AddNote(firstField.text, lastField.text, briefField.text, 
                        fullField.text, URLField.text, date, location);
    }

    public void NoteLocation()
    {
        //raycast a location
        //wait for a mouse click
        //raycast coordinates from that location
        //put that location back into the Add Notes Fields
    }
    private bool isRunning = false;
    public void Wrapper()
    {
        if (isRunning == false)
        {
            StartCoroutine(WaitForLocationPick());
        }
    }

    IEnumerator WaitForLocationPick()
    {
        isRunning = true;
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit = new RaycastHit();

                if (view.overhead)
                {
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                    {
                        //PickLocation = hit.transform.position;
                        //Debug.Log("I hit you");
                        //xField.text = "" + hit.transform.position.x;
                        //yField.text = "" + hit.transform.position.y;
                        //zField.text = "" + hit.transform.position.z;
                        xField.text = "" + hit.point.x;
                        yField.text = "" + hit.point.y;
                        zField.text = "" + hit.point.z;
                        //Debug.Log("yield break");
                        isRunning = false;
                        viewportLocation = Resources.Load<GameObject>("Note_In_Scene");
                        GameObject LocationMarker = Instantiate<GameObject>(viewportLocation);
                        LocationMarker.transform.position = hit.point;
                        yield break;
                    }
                }
                else
                {
                    if (Physics.Raycast(fpsController.GetComponentInChildren<Camera>().ScreenPointToRay(Input.mousePosition), out hit))
                    {
                        //PickLocation = hit.transform.position;
                        //Debug.Log("I hit you");
                        xField.text = "" + hit.point.x;
                        yField.text = "" + hit.point.y;
                        zField.text = "" + hit.point.z;
                        //Debug.Log("yield break");
                        isRunning = false;
                        viewportLocation = Resources.Load<GameObject>("Note_In_Scene");
                        LocationMarker = Instantiate<GameObject>(viewportLocation);
                        LocationMarker.transform.position = hit.point;
                        yield break;
                    }
                }
                
            }
            Debug.Log("yield return null");
            yield return null;
        }
        //not here yield return null;
    }

    public void UpdateLocation()
    {
        if(view.overhead)
        {
            xField.text = "" + point.lastClick.x;
            yField.text = "" + point.lastClick.y;
            zField.text = "" + point.lastClick.z;
        }
        else
        {
            xField.text = "" + fpsController.transform.position.x;
            yField.text = "" + fpsController.transform.position.y;
            zField.text = "" + fpsController.transform.position.z;
        }
    }

    public void Set(string first, string last, string brief,
               string full, string url, Vector3 loc,
               System.DateTime post, System.DateTime refDate)
    {
        //Debug.Log("Test!!!2");

        firstField.interactable = true;
        firstField.text = first;
        
        firstField.ForceLabelUpdate();
        lastField.text = last;
        briefField.text = brief;
        fullField.text = full;
        URLField.text = url;
        monthField.text = "" + refDate.Month;
        dayField.text = "" + refDate.Date;
        yearField.text = "" + refDate.Year;
        xField.text = "" + loc.x;
        yField.text = "" + loc.y;
        zField.text = "" + loc.z;
        //Debug.Log("Test!!!3");
        if (posttimeText)
            posttimeText.text = post.ToString("HH:mm:ss.fff dd/MM/yyyy");
    }

    public void UpdateTime()
    {
        dayField.text = "1";
        monthField.text = "1";
        yearField.text = "" + timeSlider.value;

    }


    public void Clear()
    {
        firstField.text = lastField.text = briefField.text = fullField.text =
        URLField.text = dayField.text = monthField.text = yearField.text =
        xField.text = yField.text = zField.text = "";
    }
}
