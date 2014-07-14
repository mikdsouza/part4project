using UnityEngine;
using System.Collections;

public class ObjectCountEventHandler : MonoBehaviour, ITrackableEventHandler {
	
	private TrackableBehaviour mTrackableBehaviour;
	
	private bool showLabel = false;
	public bool ShowLabel {
		get { return showLabel;}
	}

	private Rect labelBox = new Rect(20,20,1920,1080);
	private int numberOfObjects = 0;
	private int trackerID = -1;

	private readonly int margin = 20;
	private int heightOffset = 0;

	private int heightCount = 0;
	public int HeightCount {
		get { return heightCount;}
		set { heightCount = value;}
	}

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
		heightOffset = Screen.currentResolution.height / 18;
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
			GUI.skin.label.fontSize = Screen.currentResolution.height / 18;
			countObjects();

			if (numberOfObjects == 0)
				GUI.skin.label.normal.textColor = Color.blue;
			else
				GUI.skin.label.normal.textColor = Color.red;

			labelBox.y = (1 + heightCount) * margin + (heightCount * heightOffset);

			trackerID = (mTrackableBehaviour as MarkerBehaviour).Marker.ID;
			GUI.Label (labelBox, trackerID + ":" + numberOfObjects /*+ " " + heightOffset + " " + heightCount*/);
		}
	}

	void countObjects()
	{
		numberOfObjects = 0;

		foreach (MeshFilter mesh in gameObject.GetComponentsInChildren<MeshFilter>()) {
			if (mesh.tag != "GameController")
				continue;

			numberOfObjects++;

			if ((mesh.GetComponent(typeof(changeColour)) as changeColour).Found) 
				numberOfObjects--;
		}
	}
}
