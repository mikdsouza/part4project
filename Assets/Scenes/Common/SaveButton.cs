using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SaveButton : MonoBehaviour {
	public Texture2D imageSave, imageLoad;
	public GUIStyle style;
	List<GameObject> markers;

	// Use this for initialization
	void Start () {
		markers = new List<GameObject> ();
		
		GameObject[] objs = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		foreach (GameObject obj in objs) {
			if(obj.GetComponent<ObjectCountEventHandler>() != null) {
				markers.Add(obj);
			}
		}
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
				PlayerPrefs.SetInt(obj.ID, obj.State);
			}
		}
	}

	public void load() {
		foreach (GameObject marker in markers) {
			foreach (changeColour obj in marker.GetComponentsInChildren<changeColour>()) {
				obj.State = PlayerPrefs.GetInt(obj.ID);
			}
		}
	}
}
