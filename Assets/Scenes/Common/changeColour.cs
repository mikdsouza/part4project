using UnityEngine;
using System.Collections;

public class changeColour : MonoBehaviour {
	private enum foundState { FOUND, NOTFOUND };
	private foundState state = foundState.FOUND;

	private Material foundMat, notFoundMat;

	void start(){

	}

	public bool found() 
	{
		return state == foundState.FOUND;
	}

	public void init()
	{
		foundMat = Resources.Load<Material> ("found");
		notFoundMat = Resources.Load<Material> ("notFound");
		OnMouseUp ();
	}

	void OnMouseUp(){
		if(state == foundState.NOTFOUND) {

			renderer.material = foundMat;
			state = foundState.FOUND;
		}
		else {
			renderer.material = notFoundMat;
			state = foundState.NOTFOUND;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
