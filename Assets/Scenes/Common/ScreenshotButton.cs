using UnityEngine;
using System.Collections;

public class ScreenshotButton : MonoBehaviour {

	private int screenshotCount = 0;
	public Texture2D image;
	public GUIStyle style;

	// Update is called once per frame
	void OnGUI () {
		if (GUI.Button (new Rect (Screen.width - 120, Screen.height - 120, 100, 100), image, style)) {
			takeScreenshot();
		}
	}
	
	void takeScreenshot() {
		// Android intent shiz taken out of here coz it's not ready yet
	}
}