using UnityEngine;
using System.Collections;

public class changeColour : MonoBehaviour {
	private enum foundState { NOTFOUND, FOUND };
	private foundState state = foundState.FOUND;

	private Material foundMat, notFoundMat;

	private string id = "";
	public string ID {
		get { return id;}
	}

	void start(){

	}

	public bool Found {
		get { return state == foundState.FOUND;}
	}

	public int State {
		get { return (int)state;}
		set { state = (foundState)value; setTexture(state);}
	}

	public string Scene {
		get { return Application.loadedLevelName; }
	}

	private void setTexture(foundState fState) {
		if(fState == foundState.NOTFOUND)
			renderer.material = notFoundMat;
		else 
			renderer.material = foundMat;			
	}

	public void init()
	{
		id = Application.loadedLevelName + "." + name + "." + transform.position.x + 
				"." + transform.position.y + "." + transform.position.z + "." + 
				transform.rotation.x + "." + transform.rotation.y + "." + 
				transform.rotation.z + "." +	transform.localScale.x + "." + 
				transform.localScale.y + "." + transform.localScale.z;

		foundMat = Resources.Load<Material> ("found");
		notFoundMat = Resources.Load<Material> ("notFound");
		OnMouseUp ();
	}

	void OnMouseUp(){
		if(state == foundState.NOTFOUND)
			state = foundState.FOUND;
		else 
			state = foundState.NOTFOUND;

		setTexture (state);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
