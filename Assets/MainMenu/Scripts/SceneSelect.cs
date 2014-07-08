using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneSelect : MonoBehaviour {
	public GUISkin MainMenuGUISkin;
	public GUISkin buttonSkin;
	
	Vector2 scrollPosition;
	Touch touch;
		
	void OnGUI () {
		MainMenuGUISkin.button.fontSize = (int) heightPercentage(4);
		MainMenuGUISkin.box.fontSize = (int) heightPercentage(6);
		MainMenuGUISkin.label.fontSize = (int) heightPercentage(2);
		GUI.skin = MainMenuGUISkin;
		createMenu();
	}
	
	//Crete the main menu
	void createMenu() {
		// Make a background box
		GUI.Box(new Rect(widthPercentage(5),heightPercentage(5),widthPercentage(90),heightPercentage(90)), "Scene Select");
		
		
		// Make the second button.
		if(GUI.Button(new Rect(widthPercentage(10),heightPercentage(24),widthPercentage(80),heightPercentage(10)), "Go Back")) {
			GoBack();
		}

		List<string> levels = new List<string>();

		//Add Levels here
		levels.Add("example-scene");
		levels.Add("axes-scene");
		levels.Add("ObjectCount-Scene");
		levels.Add("ShowOff-Scene");
		levels.Add ("HourGlass-Scene");
//		levels.Add("lol2");
//		levels.Add("lol3");
//		levels.Add("lol4");
//		levels.Add("lol5");
//		levels.Add("lol6");
//		levels.Add("lol7");
//		levels.Add("lol8");
//		levels.Add("lol9");
		
		float buttonHeight = heightPercentage(5);
		float buttonWidth = widthPercentage(75);
		float buttonSpacing = heightPercentage(2);
		float buttonX = widthPercentage(11);
		float buttonY = heightPercentage(36);
		float totalHeight = levels.Count * (buttonHeight + buttonSpacing) - buttonSpacing;
		GUIStyle buttonStyle = buttonSkin.button;
		buttonStyle.fontSize = (int) heightPercentage(3);
		buttonStyle.alignment = TextAnchor.MiddleLeft;
		
		//Create the scroll view
		scrollPosition = GUI.BeginScrollView(
				new Rect(widthPercentage(10),heightPercentage(36),widthPercentage(80), heightPercentage(55)),
				scrollPosition, 
				new Rect(widthPercentage(10),heightPercentage(36),widthPercentage(80) - 20, totalHeight),
				false, false);
			
			for(int i = 0; i < levels.Count; i++)
			{
				if (GUI.Button(
						new Rect(buttonX, buttonY, buttonWidth ,buttonHeight),
						"Scene: " + levels[i], buttonStyle)) {
					Application.LoadLevel(levels[i]);
				}
				
				buttonY += buttonHeight + buttonSpacing;
			}
		
		GUI.EndScrollView();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) 
			GoBack();
		
		if(Input.touchCount > 0)
		{
			touch = Input.touches[0];
			if (touch.phase == TouchPhase.Moved)
			{
				scrollPosition.y += touch.deltaPosition.y;
			}
		}
	}

	void GoBack() {
		Application.LoadLevel ("MainMenu-scene");
	}

	/// <summary>
	/// Return the number of pixels that corresponds to that percentage
	/// of the screen width
	/// </summary>
	/// <returns>Rectures a number of pixels</returns>
	/// <param name="percent">Number between 0 and 100</param>
	float widthPercentage(float percent) {
		return (percent / 100) * Screen.currentResolution.width;
	}

	/// <summary>
	/// Return the number of pixels that corresponds to that percentage
	/// of the screen height
	/// </summary>
	/// <returns>Rectures a number of pixels</returns>
	/// <param name="percent">Number between 0 and 100</param>
	float heightPercentage(float percent) {
		return (percent / 100) * Screen.currentResolution.height;
	}
}
