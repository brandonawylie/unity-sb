using UnityEngine;
using System.Collections;

public class CaptureZoneController : MonoBehaviour {

	public float timeToCapture = 2.0f;
	public float zoneRegenRate = .5f;
	float timeLeft;

	bool isPlayerHere = false;

	Animator animator;

	public bool isRaised = false;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		timeLeft = timeToCapture;
	}
	
	// Update is called once per frame
	void Update () {
		if (!isRaised) {
			if ( timeLeft <= 0) {
				animator.SetBool("isRaised", true);
				isRaised = true;

				GameObject[] obs = GameObject.FindGameObjectsWithTag("EnemySpawn");
				print (obs.Length);
				foreach (GameObject ob in obs) {
					SpawnController con = ob.GetComponent<SpawnController>();

					if (GetComponent<BoxCollider2D>().OverlapPoint(new Vector2(con.transform.position.x, con.transform.position.y))) {
						con.isSpawning = false;
					}
				}
				//Destroy(gameObject);
			} else {
				if (isPlayerHere) {
					timeLeft -= Time.deltaTime;
				} else {
					timeLeft = Mathf.Clamp(timeLeft + zoneRegenRate, 0, timeToCapture);
				}
			}
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
