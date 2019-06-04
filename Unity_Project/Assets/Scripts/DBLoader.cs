using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
//think this comment is needed because the project has yet to be run on .NET 4.0
//using System.Security.Policy;
using SimpleJSON;
using UnityEngine.Networking;

public class DBLoader : MonoBehaviour {
	static public List<string> fieldList;
    public string dbSource;
    DateTime lastUpdate;
    DateTime mostRecent;
    float updateTimeRemaining;
    public SpatioAsset assetPrefab;
    public SpatioButton buttonPrefab;
    public SpatioDocuments DocumentsPanel;
    public NotesPanel notesPanel;
    public GameObject addConfirmation;
    public ViewNotesPanel viewNotes;
    public Canvas UI;
    // Use this for initialization
    void Start () {
        //temporarily commented to debug database coordination
        //StartCoroutine(RetrieveNotes());
    }

    void LoadData (int line_number, List<string> line)
	{
		if (line_number == 0)
		{
            fieldList = new List<string>(line);
            foreach(string str in fieldList)
                Debug.Log("FST-" + str);
		
		}else
		{
		    Dictionary<string, string> csvData = new Dictionary<string, string>();
		    for (int i = 0; i < fieldList.Count; i++)
		    {
		        if (fieldList[i] == "")
		            continue;
		        if (i >= line.Count)
		        {
                    //Debug.Log("Adding <" + fieldList[i] + ", (Empty)>");
                    csvData.Add(fieldList[i], "");
		        }else{
		            //Debug.Log("Adding <" + fieldList[i] + ", " + line[i] + ">");
		            csvData.Add(fieldList[i], line[i]);
		        }
		    }
                
            //Create a spatioasset and spatiobutton
		    SpatioAsset asset = (SpatioAsset)Instantiate(assetPrefab, Vector3.zero, Quaternion.identity);
		    SpatioButton button = (SpatioButton) Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity);
		    button.asset = asset.gameObject;
            RectTransform btnRect = button.GetComponent<RectTransform>();
            btnRect.anchorMin = new Vector2(0.5f, 1.0f);
            btnRect.anchorMax = new Vector2(0.5f, 1.0f);
            RectTransform assetRect = asset.GetComponent<RectTransform>();
            assetRect.offsetMax = new Vector2(Screen.height / 2, Screen.width / 2);


            DocumentsPanel.AddButton(button);
            Debug.Log(btnRect.offsetMin);
            Debug.Log(btnRect.offsetMax);
            asset.transform.SetParent(UI.transform, false);
            assetRect.offsetMin = new Vector2((Screen.width - 381) / 2, -(Screen.height + 275) / 2);
            assetRect.offsetMax = new Vector2((Screen.width + 381) / 2, -(Screen.height - 275) / 2);
            button.transform.SetParent(DocumentsPanel.transform, false);
            //Associate them with the UI
            StartCoroutine(fetchSource(asset, button, csvData["Host"]));
		    asset.SetAssetFields(csvData);
            //asset.gameObject.SetActive(false);
            
		}
	}

    IEnumerator fetchSource(SpatioAsset asset, SpatioButton button, string source)
    {
        string prestring = "https://drive.google.com/open?id=";
        Texture2D t = new Texture2D(1, 1);
        //Convert address
        if (source.StartsWith(prestring))
            source = "https://drive.google.com/uc?export=download&id=" + source.Substring(prestring.Length);
        Debug.Log(source);
        //source = "http://www.cartoonthrills.com/images/headerlogo.jpg";
        //WWW site = new WWW(source);
        t = Resources.Load<Texture2D>(source);
        yield return t;
       
        //site.LoadImageIntoTexture(t);
        Sprite s = Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0.5f, 0.5f));
        //Debug.Log("Texture data: " + site.text);
        
        asset.image.sprite = s;
        button.image.sprite = s;
        asset.gameObject.SetActive(false);

    }
    public IEnumerator UpdateTime()
    {
        WWWForm form = new WWWForm();
        form.AddField("operation", "status");
        WWW site = new WWW(dbSource, form);
        yield return site;
        JSONNode node = JSON.Parse(site.text);
        //Debug.Log("update node = " + node.ToString());
        //why is this null?
        //Debug.Log("outputting update node ");
        //Debug.Log(node[0]["posttime"]);
        //Debug.Log(node["data"]["MAX(posttime)"]);
        //Debug.Log(node["MAX(posttime)"]);
        //DateTime mostRecent = DateTime.ParseExact(node["MAX(posttime)"], "yyyy-MM-dd HH:mm:ss", null);

        //Debug.Log("node.Count = " + node.Count);
        for (int i = 0; i < node.Count; i++)
        {
            mostRecent = DateTime.ParseExact(node[i]["posttime"], "yyyy-MM-dd HH:mm:ss", null);
            //gets the most recent post time in the database
        }
        //Debug.Log(mostRecent.ToString());
        //if the latest post is newer than the last update post time.....then RetrieveNotes
        if (mostRecent > lastUpdate)
        //if (mostRecent != lastUpdate)
        {
            Debug.Log(mostRecent+ " is larger than " + lastUpdate);
            yield return RetrieveNotes();
            //lastUpdate = mostRecent;
        }
        

        //simplifying this to simply RetrieveNotes()
        //Debug.Log("DBLoader UpdateTime() called");
        //yield return RetrieveNotes();
    }
    
    public IEnumerator AddNote(string first, string last, string brief, string full, string URL, System.DateTime date, Vector3 location)
    {
        Debug.Log("Calling the AddNote WWW FORM Method");
        //build a form with the data
        WWWForm form = new WWWForm();
        form.AddField("operation", "insert");
        form.AddField("first", first);
        form.AddField("last", last);
        form.AddField("brief", brief);
        form.AddField("full", full);
        form.AddField("url", URL);
        form.AddField("reftime", date.ToString("yyyy-MM-dd HH:mm:ss.fff"));
        form.AddField("posttime", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
        form.AddField("x", location.x.ToString());
        form.AddField("y", location.y.ToString());
        form.AddField("z", location.z.ToString());
        //WWW site = new WWW(dbSource, form);
        //yield return site;
        //Debug.Log(site.text);

        //updating away from WWW form towards UnityWebRequest
        using (UnityWebRequest www = UnityWebRequest.Post(dbSource, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }

        //Debug.Log(date.ToString("yyyy-MM-dd HH:mm:ss.fff"));
        //Debug.Log(location.x.ToString());

        addConfirmation.SetActive(true);
        notesPanel.Clear();
    }

    //hits the php and database for a full set of notes, the contents of the database
    public IEnumerator RetrieveNotes()
    {
        //Debug.Log("RetrieveNotes() called");
        WWWForm form = new WWWForm();
        form.AddField("operation", "select");
        WWW site = new WWW(dbSource, form);
        yield return site;
        //Debug.Log(site.text);
        JSONNode node = JSON.Parse(site.text);
        //Debug.Log("retrieve node = "+node.ToString());

        //Debug.Log(node.ToString());
        //commenting out the "data" structure. This came from Bo's original PHP script that I lost access to.
        //viewNotes.UpdateNotes(node["data"], lastUpdate);
        for (int i = 0; i < node.Count; i++)
        {
            //the latest note retrieved has it's post time saved into lastUpdate
            lastUpdate = DateTime.ParseExact(node[i]["posttime"], "yyyy-MM-dd HH:mm:ss", null);
        }
        //Debug.Log("LastUpdate = " + lastUpdate);
        viewNotes.UpdateNotes(node, lastUpdate);
    }

    /*
    void DrawNotesInModel()
    {
        //Define the NoteMarker GameObject for use
        GameObject NoteMarker = new GameObject();
        //clear ViewNotesPanel
        //refresh notes in ViewNotesPanel
        //for each note - instantiate a Notes Node in the X Y Z location
        
        //loads an instance of the prefab for use
        NoteMarker = Resources.Load<GameObject>("Note_In_Scene");
        GameObject viewNote = Instantiate<GameObject>(NoteMarker);
        viewNote.transform.position = new Vector3(0, 0, 0);
        //viewNote.full = "Test text for visible markers";
    }
    */

    // Update is called once per frame
    void Update () {
	    if(updateTimeRemaining > 0)
        {
            updateTimeRemaining -= Time.deltaTime;
        }
        else
        {
            updateTimeRemaining = 20;
            //StartCoroutine(UpdateTime());
            StartNoteDownload();
        }
	}

    public void StartNoteDownload()
    {
        StartCoroutine(UpdateTime());
    }
}



//This object class is new as of May/June 2019
//Each note should be derived from this object and stored in a single central location
//This will allow for more robust
public class NoteObject : MonoBehaviour
{
    public string full;
    public string brief;
    public DateTime posttime;
    public DateTime reftime;
    public string last;
    public string first;
    public Vector3 location;

    public NoteObject(Vector3 A, string B)
    {
        location = A;
        brief = B;
    }
}
