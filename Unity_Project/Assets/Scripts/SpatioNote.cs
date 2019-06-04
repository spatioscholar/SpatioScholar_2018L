using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpatioNote : MonoBehaviour
{
    public Text textField;
    public string brief;
    public Canvas ui;
    public bool uiVisible = false;
    public GameObject cam;
    public bool uiGlobalVisible = false;
    public ViewController vcontroller;
    public GameObject manager;

    // Start is called before the first frame update
    void Start()
    {
        //Assign ViewController object
        manager = GameObject.Find("SpatioManager");
        vcontroller = manager.GetComponent<ViewController>();
    }

    void CheckActiveCamera()
    {
        //if Overhead variable in the viewcontroller overhead value is set to true
        if(vcontroller.overhead == true)
        {
            cam = vcontroller.overheadObject;
        }
        else
        {
            cam = vcontroller.FPSObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //CheckActiveCamera method called to 
        CheckActiveCamera();
        //within this block call a method to constantly reorient each note to face the "active" camera
        //Check which camera is "active" meaning either overhead or fps
        transform.LookAt(cam.transform);
        this.transform.Rotate(0,180,0);

        if(uiGlobalVisible == true && uiVisible == false)
        {
            ui.gameObject.SetActive(true);
            uiVisible = true;
        }
        else
        {
            return;
        }
    }

    void OnMouseEnter()
    {
        //Debug.Log("OnMouseEnter");
        if (uiVisible == true)
        {
            return;
        }
        if(uiVisible == false)
        {
            ui.gameObject.SetActive(true);
            uiVisible = true;
            return;
        }
    }
    void OnMouseExit()
    {
        //Debug.Log("OnMouseExit");
        if (uiVisible == true && uiGlobalVisible == false)
        {
            ui.gameObject.SetActive(false);
            uiVisible = false;
            return;
        }
        if (uiGlobalVisible == true)
        {
            ui.gameObject.SetActive(true);
            uiVisible = true;
            return;
        }
        if (uiGlobalVisible == false)
        {
            ui.gameObject.SetActive(false);
            uiVisible = false;
            return;
        }
    }
    void SetOngoingVisible(bool input)
    {
        //Debug.Log("SetOngoingVisible = " + input);
        uiGlobalVisible = input;
    }
    void OnMouseDown()
    {
        Debug.Log("OnMouseDown");
        if (uiGlobalVisible == false)
        {
            SetOngoingVisible(true);

            uiVisible = true;
        }
        else
        {
            SetOngoingVisible(false);

            uiVisible = false;
        }
    }
}
