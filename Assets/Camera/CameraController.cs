using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraController : MonoBehaviour {
	public float damp_time = 0.15f;
	private Vector3 velocity = Vector3.zero;
	public Transform target;

	PlayerController player;
	GameObject winMenu;
	GameObject loseMenu;
	bool menuDisplayed = false;

	void Awake() {
		Application.targetFrameRate = 75;
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}

	// Use this for initialization
	void Start () {
		winMenu = GameObject.FindGameObjectWithTag("Win");
		winMenu.SetActive(false);
		loseMenu = GameObject.FindGameObjectWithTag("Lose");
		loseMenu.SetActive(false);
		target = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (player.hp <= 0 && !menuDisplayed) {
			menuDisplayed = true;
			//GameObject go = GameObject.FindGameObjectWithTag("Win");
			loseMenu.SetActive(true);
			loseMenu.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 1.0f, player.transform.position.z);
			loseMenu.transform.FindChild("RestartLevelButton").GetComponent<Button>().onClick.AddListener(() => { onRestartLevelClick(); });
			loseMenu.transform.FindChild("MainMenuButton").GetComponent<Button>().onClick.AddListener(() => { onMainMenuClick(); });

		}

		GameObject[] flags = GameObject.FindGameObjectsWithTag("CaptureZone");
		bool isDone = true;
		foreach (GameObject go in flags) {
			CaptureZoneController czc = go.GetComponent<CaptureZoneController>();
			if (!czc.isRaised){
				isDone = false;
			}
		}
		if (isDone && !menuDisplayed) {
			menuDisplayed = true;
			//GameObject go = GameObject.FindGameObjectWithTag("Win");
			winMenu.SetActive(true);
			winMenu.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 1.0f, player.transform.position.z);
			winMenu.transform.FindChild("NextLevelButton").GetComponent<Button>().onClick.AddListener(() => { onNextLevelClick(); });
			winMenu.transform.FindChild("RestartLevelButton").GetComponent<Button>().onClick.AddListener(() => { onRestartLevelClick(); });
			winMenu.transform.FindChild("MainMenuButton").GetComponent<Button>().onClick.AddListener(() => { onMainMenuClick(); });

		}

		if (target) {
			Vector3 point = GetComponent<Camera>().WorldToViewportPoint(target.position);
			Vector3 delta = target.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
			Vector3 destination = transform.position + delta;
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, damp_time);
		}
	}

	void onNextLevelClick() {
		string level = Application.loadedLevelName;
		char levelNum = level.ToCharArray()[level.Length - 1];
		level = level.Substring(level.Length - 1);
		level += (levelNum + 1);
		print ("trying to load level " + level);
		Application.LoadLevel (level);
	}

	void onRestartLevelClick() {
		Application.LoadLevel(Application.loadedLevelName);
	}

	void onMainMenuClick() {
		Application.LoadLevel("menu");
	}
}
