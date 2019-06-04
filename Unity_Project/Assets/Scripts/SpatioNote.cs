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

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Camera");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseEnter()
    {
        if(uiVisible == true)
        {
            return;
        }
        if(uiVisible == false)
        {
            ui.transform.LookAt(cam.transform);
            ui.gameObject.SetActive(true);
            return;
        }
    }
    void OnMouseClick()
    {
        ui.gameObject.SetActive(false);
        uiVisible = false;
    }
}
