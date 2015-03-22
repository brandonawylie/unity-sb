using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BasicEnemyAI : MonoBehaviour {

	public float chaseSpeed = 2f;
	public float patrolSpeed = 3f;
	public float chaseDistance = 1.0f;

	Vector3 direction;

	GameObject playerGameObject;
	// Use this for initialization
	void Start () {
		playerGameObject = GameObject.Find("Player");
		Vector3 toPlayer = playerGameObject.transform.position - transform.position;
		direction = new Vector3(toPlayer.x < 0 ? -1.0f : 1.0f, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		// AI
		// Move towards player
		// TODO have zones where teh enemies potrol
		// TODO if player is close, then  use this within zones
		float distance = Vector3.Distance(transform.position, playerGameObject.transform.position);
		//print (distance);
		Vector3 toPlayer = playerGameObject.transform.position - transform.position;
		toPlayer.Normalize();
		if (distance <= chaseDistance) {
			print ("chasing player");
			Vector3 chaseVector = new Vector3 (toPlayer.x, 0, 0) * chaseSpeed * Time.deltaTime;
			transform.position += chaseVector;
		} else {
			Vector3 patrolVector = direction * patrolSpeed * Time.deltaTime;
			//print (patrolVector);
			transform.position += patrolVector;
		}
	}

	
	void OnTriggerEnter2D(Collider2D other) {	
		//print ("collision");
		if (other.gameObject.tag == "PatrolFlag") {
			print ("patrol flag hit");
			print (direction);
			direction = new Vector3(-direction.x, 0, 0);
			print (direction);
		}
	}


}
