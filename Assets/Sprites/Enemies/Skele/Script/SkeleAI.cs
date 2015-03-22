using UnityEngine;
using System.Collections;

public class SkeleAI : MonoBehaviour {

	GameObject playerGameObject;
	Vector3 direction;
	
	public float speed = 3.0f;
	public float timeToTurnAround = 2.0f;
	public float detectPlayerDistance = 10.0f;
	float lastTurnTime;
	bool isFacingRight = true;

	float attackSpeed  = 6.0f;
	float attackDistance = 1;

	Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		playerGameObject = GameObject.FindGameObjectWithTag("Player");
		direction = new Vector3(1,0,0);
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x + (isFacingRight ? .6f : -.7f), transform.position.y), 
		                                   isFacingRight ? Vector2.right : -Vector2.right, detectPlayerDistance, 1 << 9);
		print (hit.collider);
		if (hit.collider != null && hit.collider.gameObject.tag == "Player") {
			lastTurnTime = Time.time;
			transform.position += direction * speed * Time.deltaTime;
			animator.SetBool("isWalking", true);
		} else {
			animator.SetBool("isWalking", false);
			if (Time.time - lastTurnTime >= timeToTurnAround) {
				direction = -direction;
				lastTurnTime = Time.time;
			}
		}



		// TODO if player is close, then  use this within zones
		float distance = Vector3.Distance(transform.position, playerGameObject.transform.position);
		//print (distance);
		if (distance <= attackDistance) {
			Vector3 toPlayer = playerGameObject.transform.position - transform.position;
			toPlayer.Normalize();
			direction = new Vector3(toPlayer.x > 0 ? 1: -1, 0, 0);
			Vector2 force = new Vector2(toPlayer.x * attackSpeed * Time.deltaTime, toPlayer.y * attackSpeed * Time.deltaTime);
			GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
			animator.SetBool("isAttacking", true);
		} else {
			animator.SetBool("isAttacking", false);
		}

		if (isFacingRight && direction.x < 0) {
			flip();
		} else if (!isFacingRight && direction.x > 0) {
			flip();
		}


		//transform.position += direction * speed * Time.deltaTime;
	
	}

	// flip the player's sprite to walk left & right
	void flip() {
		isFacingRight = !isFacingRight;
		
		Vector3 scalar = transform.localScale;
		scalar.x *= -1;
		transform.localScale = scalar;
	}
}
