using UnityEngine;
using System.Collections;

public class jetScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(GetCurrentOffset());
	}

	Vector3 GetCurrentOffset () {
		return new Vector3(0, 0, (float) (0.002 * System.Math.Sin(Time.time * 2)));
	}
}
