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
		#if UNITY_ANDROID
		// Take the screenshot
		string filename = "screenshot" + screenshotCount + ".png";
		Application.CaptureScreenshot(filename);
		string origin = System.IO.Path.Combine(Application.persistentDataPath, filename);

		//Grab the current activity
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject activity = jc.GetStatic<AndroidJavaObject>("currentActivity");

		// Call the Android intent
		activity.Call("sendEmail", "james.s.mcarthur@gmail.com", origin);

		screenshotCount++;
		#endif
	}
}