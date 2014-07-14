using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectCountOffset : MonoBehaviour {
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
	void Update () {
		int runningCount = 0;

		foreach (ObjectCountEventHandler marker in markers) {
			if(marker.ShowLabel)		
				marker.HeightCount = runningCount++;
		}
	}
}
