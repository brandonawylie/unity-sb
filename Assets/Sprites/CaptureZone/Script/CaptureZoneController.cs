using UnityEngine;
using System.Collections;

public class CaptureZoneController : MonoBehaviour {

	public float timeToCapture = 2.0f;
	public float zoneRegenRate = .5f;
	float timeLeft;

	bool isPlayerHere = false;

	// Use this for initialization
	void Start () {
		timeLeft = timeToCapture;
	}
	
	// Update is called once per frame
	void Update () {
		if ( timeLeft <= 0) {
			Destroy(gameObject);
		}

		if (isPlayerHere) {
			timeLeft -= Time.deltaTime;
		} else {
			timeLeft = Mathf.Clamp(timeLeft + zoneRegenRate, 0, timeToCapture);
		}
	}

	void OnTriggerEnter2D(Collider2D collision){
		if (collision.gameObject.tag == "Player") {
			isPlayerHere = true;
		}
	}

	void OnTriggerExit2D(Collider2D collision) {
		if (collision.gameObject.tag == "Player") {
			isPlayerHere = false;
		}
	}
}
