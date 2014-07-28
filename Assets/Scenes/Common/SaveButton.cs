using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SaveButton : MonoBehaviour {
	public Texture2D imageSave, imageLoad;
	public GUIStyle style;
	List<GameObject> markers;
	private string serverURLPost;
	private string serverURLGet;
	private Dictionary<string, changeColour> sceneObjects;

	// Use this for initialization
	void Start () {
		markers = new List<GameObject> ();
		
		GameObject[] objs = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		foreach (GameObject obj in objs) {
			if(obj.GetComponent<ObjectCountEventHandler>() != null) {
				markers.Add(obj);
			}
		}

		sceneObjects = new Dictionary<string, changeColour>();
		foreach(GameObject marker in markers) {
			foreach(changeColour obj in marker.GetComponentsInChildren<changeColour>()) {
				sceneObjects.Add(obj.ID, obj);
			}
		}

		serverURLPost = "http://" + ServerSettings.serverAddress + "/insertToDB";
		serverURLGet = "http://" + ServerSettings.serverAddress + "/getFromDB";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		if (GUI.Button (new Rect (20, Screen.height - 120, 100, 100), imageSave, style)) {
			save ();
		}

		if (GUI.Button (new Rect (140, Screen.height - 120, 100, 100), imageLoad, style)) {
			load ();
		}
	}

	void save(){
		Debug.Log ("Performing save action");
		foreach(GameObject marker in markers) {
			foreach(changeColour obj in marker.GetComponentsInChildren<changeColour>()) {
				//PlayerPrefs.SetInt(obj.ID, obj.State);
				PlayerPrefs.SetString(obj.ID, obj.State + "," + getTime().ToString());
			}
		}

		if (ServerSettings.useServer) {
			saveToServer();
		}
	}

	void saveToServer() {
		Debug.Log ("Performing save to server action");
		WWWForm form = new WWWForm();
		string serialized = "";

		foreach(GameObject marker in markers) {
			foreach(changeColour obj in marker.GetComponentsInChildren<changeColour>()) {
				serialized += obj.Scene + "," + obj.ID + "," + obj.State + "," + getTime() + ";";
			}
		}

		form.AddField("data", serialized);
		WWW www = new WWW(serverURLPost, form);
		StartCoroutine(WaitForRequest(www));
	}

	double getTime() {
		System.DateTime epochStart = new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc);
		double timestamp = (System.DateTime.UtcNow - epochStart).TotalSeconds;
		return timestamp;
	}

	//Taken from http://answers.unity3d.com/questions/11021/how-can-i-send-and-receive-data-to-and-from-a-url.html
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

	//Taken from http://answers.unity3d.com/questions/11021/how-can-i-send-and-receive-data-to-and-from-a-url.html
	IEnumerator setObjectState(WWW www)
	{
		yield return www;
		
		// check for errors
		if (www.error == null) {
			Debug.Log("WWW Ok!: " + www.text);

			foreach(string line in www.text.Split(';')) {
				string[] paramServer = line.Split(',');
				changeColour objekt;
				sceneObjects.TryGetValue(paramServer[0], out objekt);

				//Get the data from the reg
				string[] paramReg = PlayerPrefs.GetString(paramServer[0]).Split(',');

				double currentTime = 0;
				if (paramReg.Length >= 2) {
					double.TryParse(paramReg[1], out currentTime);
				}

				double serverTime = 0;
				double.TryParse(paramServer[2], out serverTime);

				//Comapre times
				if(currentTime < serverTime) {
					int state = 0;
					int.TryParse(paramServer[1], out state);
					objekt.State = state;
					PlayerPrefs.SetString(objekt.ID, objekt.State + "," + serverTime);
				}
			}
		} 
		else {
			Debug.Log("WWW Error: "+ www.error);
		}    
	}   

	public void load() {
		Debug.Log("Performing load action");
		foreach (GameObject marker in markers) {
			foreach (changeColour obj in marker.GetComponentsInChildren<changeColour>()) {
				//obj.State = PlayerPrefs.GetInt(obj.ID);
				string pref = PlayerPrefs.GetString(obj.ID, "0");
				int state;
				double time;

				//Incase there is no save data
				if(!pref.Contains(",")) {
					int.TryParse(pref, out state);
					time = 0;
				}
				else {
					//[0] is the value
					//[1] is the timestamp
					string[] prefs = pref.Split(',');

					double.TryParse(prefs[1], out time);
					int.TryParse(prefs[0], out state);
				}

				obj.State = state;

			}
		}
		if(ServerSettings.useServer) {
			WWWForm form = new WWWForm();
			form.AddField("scene_name", Application.loadedLevelName);
			WWW www = new WWW(serverURLGet, form);
			StartCoroutine(setObjectState(www));
		}
	}
}
