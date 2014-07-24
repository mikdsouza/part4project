using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SaveButton : MonoBehaviour {
	public Texture2D imageSave, imageLoad;
	public GUIStyle style;
	List<GameObject> markers;
	private string serverURLPost;
	private string serverURLGet;

	// Use this for initialization
	void Start () {
		markers = new List<GameObject> ();
		
		GameObject[] objs = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		foreach (GameObject obj in objs) {
			if(obj.GetComponent<ObjectCountEventHandler>() != null) {
				markers.Add(obj);
			}
		}

		serverURLPost = ServerSettings.serverAddress + "/insertToDB";
		serverURLGet = ServerSettings.serverAddress + "/getFromDB";
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
				PlayerPrefs.SetString(obj.ID, obj.State + "," + getTime().ToString());
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
				form.AddField("time", getTime().ToString());
				WWW www = new WWW(serverURLPost, form);

				StartCoroutine(WaitForRequest(www));
			}
		}
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
	IEnumerator setObjectState(WWW www, changeColour obj, double time)
	{
		yield return www;
		
		// check for errors
		if (www.error == null) {
			Debug.Log("WWW Ok!: " + www.text);

			string[] prefs = www.text.Split(',');
			double serverTime;
			int state;
			
			double.TryParse(prefs[1], out serverTime);
			int.TryParse(prefs[0], out state);
			
			if (serverTime > time)
				obj.State = state;
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

				if(ServerSettings.useServer) {
					WWWForm form = new WWWForm();
					form.AddField("str_id", obj.ID);
					WWW www = new WWW(serverURLGet, form);
					StartCoroutine(setObjectState(www, obj, time));
				}
			}
		}
	}
}
