using UnityEngine;
using System.Collections;

public class ObjectCountEventHandler : MonoBehaviour, ITrackableEventHandler {
	
	private TrackableBehaviour mTrackableBehaviour;
	
	private bool showLabel = false;
	private Rect labelBox = new Rect(0,0,1920,1080);
	private int numberOfObjects = 3;

	void Start () {

		mTrackableBehaviour = GetComponent<TrackableBehaviour>();
		if (mTrackableBehaviour)
		{
			mTrackableBehaviour.RegisterTrackableEventHandler(this);
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

			if (numberOfObjects == 0)
				GUI.skin.label.normal.textColor = Color.blue;
			else
				GUI.skin.label.normal.textColor = Color.red;

			GUI.Label (labelBox, numberOfObjects.ToString());
		}
	}
}
