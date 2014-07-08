using UnityEngine;
using System.Collections;

public class ObjectCountEventHandler : MonoBehaviour, ITrackableEventHandler {
	
	private TrackableBehaviour mTrackableBehaviour;
	
	private bool showLabel = false;
	private Rect labelBox = new Rect(20,20,1920,1080);
	private int numberOfObjects = 0;

	void Start () {

		mTrackableBehaviour = GetComponent<TrackableBehaviour>();
		if (mTrackableBehaviour)
		{
			mTrackableBehaviour.RegisterTrackableEventHandler(this);
		}

		foreach( MeshFilter mesh in gameObject.GetComponentsInChildren<MeshFilter>())
		{
			if (mesh.tag != "GameController")
				continue;

			mesh.gameObject.AddComponent("changeColour");
			(mesh.GetComponent(typeof(changeColour)) as changeColour).init();
		}
	}
	
	public void OnTrackableStateChanged(
		TrackableBehaviour.Status previousStatus,
		TrackableBehaviour.Status newStatus)
	{
		if (newStatus == TrackableBehaviour.Status.DETECTED ||
		    newStatus == TrackableBehaviour.Status.TRACKED)
		{
			showLabel = true;
		}
		else
		{
			showLabel = false;
		}
	}
	
	void OnGUI() {
		if (showLabel) {
			GUI.skin.label.fontSize = Screen.currentResolution.height / 15;
			countObjects();

			if (numberOfObjects == 0)
				GUI.skin.label.normal.textColor = Color.blue;
			else
				GUI.skin.label.normal.textColor = Color.red;

			GUI.Label (labelBox, numberOfObjects.ToString());
		}
	}

	void countObjects()
	{
		numberOfObjects = 0;

		foreach (MeshFilter mesh in gameObject.GetComponentsInChildren<MeshFilter>()) {
			if (mesh.tag != "GameController")
				continue;

			numberOfObjects++;

			if ((mesh.GetComponent(typeof(changeColour)) as changeColour).found()) 
				numberOfObjects--;
		}
	}
}
