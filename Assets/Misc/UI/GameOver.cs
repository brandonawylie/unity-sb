using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class GameOver : MonoBehaviour {
	GameObject continueButton, mainMenuButton;
	// Use this for initialization
	void Start () {
		continueButton = GameObject.Find("ContinueButton");
		continueButton.GetComponent<Button>().onClick.AddListener(() => { OnContinueClick(); });
		mainMenuButton = GameObject.Find("MainMenuButton");
		mainMenuButton.GetComponent<Button>().onClick.AddListener(() => { OnMainMenuClick(); });
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnContinueClick() {
		string level = PlayerPrefs.GetString ("LastLevel");
		Application.LoadLevel(level);
	}

	void OnMainMenuClick() {
		Application.LoadLevel("menu");
	}
}
