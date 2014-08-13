using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenshotButton : MonoBehaviour {

	public Texture2D image;
	public GUIStyle style;

	private string marker_name = "";
	private string uploadURL = "http://" + ServerSettings.serverAddress + "/upload";

	List<ObjectCountEventHandler> markers;
	
	// Use this for initialization
	void Start () {
		markers = new List<ObjectCountEventHandler> ();
		
		GameObject[] objs = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		foreach (GameObject obj in objs) {
			if(obj.GetComponent<ObjectCountEventHandler>() != null) {
				markers.Add(obj.GetComponent<ObjectCountEventHandler>());
			}
		}
	}

	// Update is called once per frame
	void OnGUI () {
		if (GUI.Button (new Rect (Screen.width - 120, Screen.height - 120, 100, 100), image, style)) {

			// Get the first detected marker name (yucky)
			marker_name = "";
			foreach (ObjectCountEventHandler marker in markers) {
				if(marker.ShowLabel) {		
					marker_name = marker.name;
					break;
				}
			}

			// Check we've got a marker in view
			if (marker_name == "")
				return;

			// Take the screenshot and sent it to the server
			StartCoroutine(takeScreenshot());
		}
	}


	IEnumerator takeScreenshot() {
		yield return new WaitForEndOfFrame();

		// Create a texture the size of the screen, RGB24 format 
		Texture2D tex = new Texture2D(Screen.width, Screen.height);
		tex.ReadPixels(new Rect(0,0,Screen.width,Screen.height),0,0);
		tex.Apply();
		
		// Encode texture into PNG 
		var bytes = tex.EncodeToPNG(); 
		Destroy( tex );

		// Send off the request to the server
		WWWForm form = new WWWForm();
		form.AddField ("marker", marker_name);
		form.AddField ("scene", Application.loadedLevelName);
		form.AddBinaryData ( "file", bytes, marker_name + ".png" ,"image/png");
		WWW www = new WWW(uploadURL, form);
		StartCoroutine(WaitForRequest(www));
		Debug.Log ("Uploaded screenshot for " + marker_name + ", " + bytes.Length);

	}

	IEnumerator WaitForRequest(WWW www)
	{
		yield return www;
		
		// check for errors
		if (www.error == null) {
			Debug.Log("WWW Ok!: " + www.text);
		} 
		else {
			Debug.Log("WWW Error: "+ www.error);
		}    
	}
}