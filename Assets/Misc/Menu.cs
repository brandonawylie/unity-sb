using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;


public class Menu : MonoBehaviour {

	// Main Menu
	GameObject mainMenu;
	GameObject startButton;
	GameObject continueButton;
	GameObject levelSelectButton;
	GameObject tutorialButton;

	// Level Select
	GameObject levelSelectMenu;
	GameObject levelSelection;
	GameObject backButtonLevel;
	

	// Use this for initialization
	void Start () {
		// Main Menu
		mainMenu = GameObject.Find("Main_Menu");

		// Add Listeners for Main Menu
		startButton = GameObject.Find("Start_Button");
		startButton.GetComponent<Button>().onClick.AddListener(() => { OnStartClick(); });
		continueButton = GameObject.Find("Continue_Button");
		continueButton.GetComponent<Button>().onClick.AddListener(() => { OnContinueClick(); });
		levelSelectButton = GameObject.Find("Level_Select_Button");
		levelSelectButton.GetComponent<Button>().onClick.AddListener(() => { OnLevelSelectClick(); });
		tutorialButton = GameObject.Find("Tutorial_Button");
		tutorialButton.GetComponent<Button>().onClick.AddListener(() => { OnTutorialClick(); });

		// Level Select Menu
		levelSelectMenu = GameObject.Find("Level_Select_Menu");
		levelSelection = GameObject.Find ("Level_Selection");
		backButtonLevel = GameObject.Find ("Back_Button_Level");
		backButtonLevel.GetComponent<Button>().onClick.AddListener(() => { OnBackButtonLevelClick(); });

		string myPath = Application.dataPath;
		//string levelsPath = assetsPath.Substring(0,myPath.LastIndexOf('/')) + "/levels";
		string[] files;
		files = Directory.GetFiles(myPath);
		int i = 0;
		foreach (string file in files)
		{
			if(file.EndsWith(".unity"))
			{
				GameObject button = Instantiate(Resources.Load("Menu_Button", typeof(GameObject))) as GameObject;
				button.transform.SetParent(levelSelection.transform, false);
				RectTransform t = button.GetComponent<RectTransform>();
				t.anchorMax = new Vector2(t.anchorMax.x, t.anchorMax.y -0.15f * i);
				t.anchorMin = new Vector2(t.anchorMin.x, t.anchorMin.y -0.15f * i);
				button.GetComponentInChildren<Text>().text = file;
				button.GetComponent<Button>().onClick.AddListener(() => { Application.LoadLevel(file.Replace(".unity","")); });
				i++;
			}
		}
		levelSelectMenu.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	void OnStartClick()
	{
		print ("Start Button Clicked");
	}
	void OnContinueClick()
	{
		print ("Continue Button Clicked");
	}
	void OnLevelSelectClick()
	{
		print ("Level Select Button Clicked");
		mainMenu.SetActive(false);
		levelSelectMenu.SetActive(true);
	}
	void OnTutorialClick()
	{
		print ("Tutorial Button Clicked");
	}
	void OnBackButtonLevelClick()
	{
        levelSelectMenu.SetActive(false);
        mainMenu.SetActive(true);
	}
}
