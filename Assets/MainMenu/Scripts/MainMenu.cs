﻿using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	public GUISkin MainMenuGUISkin;
		
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
		GUI.Box(new Rect(widthPercentage(5),heightPercentage(5),widthPercentage(90),heightPercentage(90)), "FrameCAD AR");
		GUI.Label(new Rect(widthPercentage(5), heightPercentage(13), widthPercentage(90), heightPercentage(4)), "Created by Mikhail and James");
		
		// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
		if(GUI.Button(new Rect(widthPercentage(10),heightPercentage(24),widthPercentage(80),heightPercentage(10)), "Scenes")) {
			Application.LoadLevel("SceneSelect");
		}
		
		// Make the second button.
		if(GUI.Button(new Rect(widthPercentage(10),heightPercentage(36),widthPercentage(80),heightPercentage(10)), "Exit")) {
			Quit();
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) 
			Quit();	
	}

	void Quit() {
		Application.Quit();
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
