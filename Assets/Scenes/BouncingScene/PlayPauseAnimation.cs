using UnityEngine;
using System.Collections;

public class PlayPauseAnimation : MonoBehaviour, ITrackableEventHandler {

	private TrackableBehaviour mTrackableBehaviour;

	// Use this for initialization
	void Start () {
		mTrackableBehaviour = GetComponent<TrackableBehaviour>();
		if (mTrackableBehaviour)
		{
			mTrackableBehaviour.RegisterTrackableEventHandler(this);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// Implementation of the ITrackableEventHandler function called when the
	/// tracking state changes.
	/// </summary>
	public void OnTrackableStateChanged(
		TrackableBehaviour.Status previousStatus,
		TrackableBehaviour.Status newStatus)
	{
		if (newStatus == TrackableBehaviour.Status.DETECTED ||
		    newStatus == TrackableBehaviour.Status.TRACKED ||
		    newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
		{
			Time.timeScale = 1;
		}
		else
		{
			Time.timeScale = 0;
		}

		Debug.Log ("Set time scale to " + Time.timeScale);
	}
}
