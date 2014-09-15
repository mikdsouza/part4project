using UnityEngine;
using System.Collections;

public class shadowScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.localScale += GetCurrentOffset();
	}

	Vector3 GetCurrentOffset () {
		return new Vector3((float) (-0.0004 * System.Math.Sin(Time.time * 2)), 
		                   (float) (-0.0004 * System.Math.Sin(Time.time * 2)), 
		                   (float) (-0.0004 * System.Math.Sin(Time.time * 2)));
	}
}
