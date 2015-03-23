using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public float damp_time = 0.15f;
	private Vector3 velocity = Vector3.zero;
	public Transform target;

	PlayerController player;

	void Awake() {
		Application.targetFrameRate = 75;
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}

	// Use this for initialization
	void Start () {
		target = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (player.hp <= 0) {
			PlayerPrefs.SetString("LastLevel", Application.loadedLevelName);
			Application.LoadLevel("GameOver");
		}

		if (target) {
			Vector3 point = GetComponent<Camera>().WorldToViewportPoint(target.position);
			Vector3 delta = target.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
			Vector3 destination = transform.position + delta;
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, damp_time);
		}
	}
}
