using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ApplyAll : MonoBehaviour {

	// Use this for initialization
	void Start () {		
		//Apply the back to main menu behaviour
		gameObject.AddComponent("BackToMainMenu");
		
		//Apply the Object counting scripts
		GameObject[] objs = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		foreach (GameObject obj in objs) {
			if(obj.GetComponent<DefaultTrackableEventHandler>() != null) {
				obj.AddComponent("ObjectCountEventHandler");
			}
		}
		gameObject.AddComponent("ObjectCountOffset");
		
		//Apply save button
		gameObject.AddComponent("SaveButton");
		SaveButton sb = gameObject.GetComponent(typeof(SaveButton)) as SaveButton;
		sb.style = new GUIStyle();
		sb.imageSave = Resources.Load<Texture2D>("save-icon");
		sb.imageLoad = Resources.Load<Texture2D>("open-icon");
		
		//Apply the screenshot feature
		gameObject.AddComponent("ScreenshotButton");
		ScreenshotButton sc = gameObject.GetComponent(typeof(ScreenshotButton)) as ScreenshotButton;
		sc.image = Resources.Load<Texture2D>("screenshot-icon");
		sc.style = new GUIStyle();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
