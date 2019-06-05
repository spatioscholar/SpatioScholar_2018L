﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Reflection;
using System.Security.AccessControl;
//think this comment is needed because the project has yet to be run on .NET 4.0
//using System.Security.Policy;
using UnityEngine.UI;
public class SpatioAsset : MonoBehaviour {
	public Vector3 dragStart;
    public Vector3 mouseStart;
	public Sprite photo;
	public Image image;
	public SpatioAssetPanel imagePanel;
	public SpatioAssetPanel infoPanel;
    public Text title;
    public Text imageCreator;
    public Text dataCreator;
    public Text source;
    public Text date;
    public Text doc;
    public Text publisher;
    public Text language;
    public Text description;
    public Text rights;
    public Image infoPanelThumbnail;
    public string Block;

	// Use this for initialization
	void Start () {
		imagePanel.SetPhoto(photo);
		infoPanel.SetPhoto(photo);
        //infoPanel.transform.Find("Image").gameObject.GetComponent<RawImage>().texture = ;
        int imgWidth = image.sprite.texture.width;
		int imgHeight = image.sprite.texture.height;
		float ratio = (float)imgHeight / imgWidth;

		if (imgWidth > imgHeight) {
			imgWidth = Screen.width / 3 - 50;
			imgHeight = (int)(imgWidth * ratio);
		} else {
			imgWidth = (int)(Screen.height / 2 / ratio) - 20;
			imgHeight = (int)(imgWidth * ratio);;

		}
		RectTransform t = GetComponent<RectTransform>();
		image.rectTransform.sizeDelta = new Vector2 (imgWidth, imgHeight);
		t.sizeDelta = new Vector2(Screen.width / 3, imgHeight + 100);
		image.rectTransform.anchoredPosition = new Vector2 (0, -(imgHeight + 50));

	}
	public void BeginDrag()
	{
	    mouseStart = Input.mousePosition;
	    dragStart = transform.position;
	}

    //toggles between front and back panels, image and info
	public void SwitchPanel()
	{
		imagePanel.Switch ();
		infoPanel.Switch ();
	}

	public void Drag()
	{
		transform.position = dragStart + Input.mousePosition - mouseStart;
	}

    public void SetAssetFields(Dictionary<string,string> dict)
    {

            foreach (string s in dict.Keys)
        {
            if (s == "Title")
            {
                //Debug.Log(dict[s]);
                title.text = dict[s];
            }else if (s == "Creator/Author")
            {
                dataCreator.text = imageCreator.text = s + ": " + dict[s];
            }else if (s == "Date")
            {
                date.text = s + ": " + dict[s];
            }else if (s == "Source")
            {
                source.text = s + ": " + dict[s];
            }
            else if (s == "Publisher")
            {
                publisher.text = s + ": " + dict[s];
            }
            else if (s == "Document numbers (if official document)")
            {
                doc.text = s + ": " + dict[s];
            }
            else if (s ==  "Language")
            {
                language.text = dict[s];
            }
            else if (s == "Description")
            {
                //need to make the Description field that text goes into scrollable
                description.text = s + ": " + dict[s];
                //Debug.Log("Found Description identifier upon import of CSV file = " + dict[s]);
            }
            else if (s == "Rights")
            {
                rights.text = s + ": " + dict[s];
                //Debug.Log("Found Rights identifier upon import of CSV file = " + dict[s]);
            }
            else if (s == "Block")
            {
                //Debug.Log("Found Block identifier");
                Block = dict[s];
                //Debug.Log(dict[s]);
            }
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
