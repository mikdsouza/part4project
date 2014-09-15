using UnityEngine;
using System.Collections;

public class jetScript : MonoBehaviour {
	public Rigidbody missile1, missile2;
	private bool missile2Fire = false;

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

	void OnMouseUp() {
		Rigidbody clone, missile;

		if (missile2Fire)
			missile = missile1;
		else
			missile = missile2;

		missile2Fire = !missile2Fire;

		clone = Instantiate(missile, missile.GetComponent<Transform>().position + (new Vector3(0,(float)-0.2,0))
			, missile.GetComponent<Transform>().rotation) as Rigidbody;
		clone.AddRelativeForce(new Vector3(500,0,0));
		clone.GetComponentInChildren<ParticleEmitter>().emit = true;
		clone.GetComponent<Transform>().localScale = new Vector3((float)0.1168631,(float)0.1168631,(float)0.1168631);
		Destroy(clone.gameObject, 1);
	}
}
