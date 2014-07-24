using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SaveButton : MonoBehaviour {
	public Texture2D imageSave, imageLoad;
	public GUIStyle style;
	List<GameObject> markers;
	private string serverURL;

	// Use this for initialization
	void Start () {
		markers = new List<GameObject> ();
		
		GameObject[] objs = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		foreach (GameObject obj in objs) {
			if(obj.GetComponent<ObjectCountEventHandler>() != null) {
				markers.Add(obj);
			}
		}

		serverURL = ServerSettings.serverAddress + "/insertToDB";
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
		foreach(GameObject marker in markers) {
			foreach(changeColour obj in marker.GetComponentsInChildren<changeColour>()) {
				//PlayerPrefs.SetInt(obj.ID, obj.State);
				PlayerPrefs.SetString(obj.ID, obj.State + "," + Time.time);
			}
		}

		if (ServerSettings.useServer) {
			saveToServer();
		}
	}

	void saveToServer() {
		foreach(GameObject marker in markers) {
			foreach(changeColour obj in marker.GetComponentsInChildren<changeColour>()) {
				WWWForm form = new WWWForm();
				form.AddField("str_id", obj.ID);
				form.AddField("state", obj.State);
				form.AddField("time", Time.time.ToString());
				WWW www = new WWW(serverURL, form);

				StartCoroutine(WaitForRequest(www));
			}
		}
	}

	//Taken from http://answers.unity3d.com/questions/11021/how-can-i-send-and-receive-data-to-and-from-a-url.html
	IEnumerator WaitForRequest(WWW www)
	{
		yield return www;

		// check for errors
		if (www.error == null) {
			Debug.Log("WWW Ok!: " + www.data);
		} 
		else {
			Debug.Log("WWW Error: "+ www.error);
		}    
	}   

	public void load() {
		foreach (GameObject marker in markers) {
			foreach (changeColour obj in marker.GetComponentsInChildren<changeColour>()) {
				//obj.State = PlayerPrefs.GetInt(obj.ID);
				string pref = PlayerPrefs.GetString(obj.ID, "0");
				int state;
				float time;

				//Incase there is no save data
				if(!pref.Contains(",")) {
					int.TryParse(pref, out state);

				}
				else {
					//[0] is the value
					//[1] is the timestamp
					string[] prefs = pref.Split(',');
					float.TryParse(prefs[1], out time);
					int.TryParse(prefs[0], out state);
				}

				obj.State = state;
			}
		}
	}
}
