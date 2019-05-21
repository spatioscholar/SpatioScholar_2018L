using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpatioButton : MonoBehaviour {
	public GameObject asset;
    public Image image;
	// Use this for initialization
	void Start () {
	}
	public void OnClick()
	{
        //each document in the documents panel is a button, this is the onclick behaviour for each. I think this sets the associated image panel active.
		asset.SetActive (!asset.activeSelf);
	}
	// Update is called once per frame
	void Update () {
	
	}
}
