using UnityEngine;
using System.Collections;

public class ServerSettings : MonoBehaviour {
	public GUISkin MainMenuGUISkin, ButtonGUI;
	public static string serverAddress = "127.0.0.1:80";
	public static bool useServer = false;

	// Use this for initialization
	void Start () {
		serverAddress = PlayerPrefs.GetString("serverAddress", "127.0.0.1:80");
		useServer = PlayerPrefs.GetInt("useServer") == 1;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)) {
			GoBack();
		}
	}

	void OnGUI() {
		MainMenuGUISkin.button.fontSize = (int) MainMenu.heightPercentage(4);
		MainMenuGUISkin.textField.fontSize = (int) MainMenu.heightPercentage(4);
		MainMenuGUISkin.toggle.fontSize = (int) MainMenu.heightPercentage(3);
		MainMenuGUISkin.box.fontSize = (int) MainMenu.heightPercentage(6);
		MainMenuGUISkin.label.fontSize = (int) MainMenu.heightPercentage(2);
		ButtonGUI.label.fontSize = (int) MainMenu.heightPercentage(3);
		GUI.skin = MainMenuGUISkin;
		createServerSettingsGUI();
	}

	void createServerSettingsGUI() {
		// Make a background box
		GUI.Box(new Rect(MainMenu.widthPercentage(5),
				MainMenu.heightPercentage(5),
				MainMenu.widthPercentage(90),
				MainMenu.heightPercentage(90)), "Server Settings");
		
		
		// Make the second button.
		if(GUI.Button(new Rect(MainMenu.widthPercentage(10),
				MainMenu.heightPercentage(24),
		    	MainMenu.widthPercentage(80),
		    	MainMenu.heightPercentage(10)), "Go Back")) {
			GoBack();
		}

		serverAddress = GUI.TextField(new Rect(
				MainMenu.widthPercentage(10),
				MainMenu.heightPercentage(36),
				MainMenu.widthPercentage(80),
				MainMenu.heightPercentage(10)), serverAddress);

		useServer = GUI.Toggle(new Rect(
			MainMenu.widthPercentage(10),
			MainMenu.heightPercentage(48),
			MainMenu.heightPercentage(3),
			MainMenu.heightPercentage(3)), useServer, "Use server");

		GUI.Label(new Rect(
			MainMenu.widthPercentage(11) + MainMenu.heightPercentage(3),
			MainMenu.heightPercentage(48),
			MainMenu.widthPercentage(79) - MainMenu.heightPercentage(3),
			MainMenu.heightPercentage(3)), "Use Server", ButtonGUI.label);
	}

	void GoBack() {
		PlayerPrefs.SetString("serverAddress", serverAddress);

		if(useServer)
			PlayerPrefs.SetInt("useServer", 1);
		else
			PlayerPrefs.SetInt("useServer", 0);

		Application.LoadLevel ("MainMenu-scene");
	}
}
