using UnityEngine;
using System.Collections;

public class FlyingMonsterAI : MonoBehaviour {

	// The diameter of the area the monster patrols.
	public float patrolDiameter;
	public float patrolSpeed;
	public float chaseSpeed;

	// The distance between the enemy and the player before the enemy starts chasing.
	public float chaseDistance;

	// When this is >= patrolDiameter, start moving left.
	// If it's <= 0, start moving right.
	private float distanceFromStart;
	private bool isFacingRight;
	private Animator animator;
	private GameObject playerGameObject;

	// Use this for initialization
	void Start () {
		distanceFromStart = 0f;
		isFacingRight = true;
		animator = GetComponent<Animator>();
		playerGameObject = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
		bool isDying = animator.GetBool("isDying");
		if (!isDying) {
			float dx;
			if (getPlayerDistance() <= chaseDistance) {
				distanceFromStart = patrolDiameter / 2;  // So that the enemey patrols the area immediately surrounding where it "loses sight" of the player.
				if (playerGameObject.transform.position.x > transform.position.x) {
					isFacingRight = true;
				} else {
					isFacingRight = false;
				}
				// TODO: Actually get monster to chase. Also do attack animation on contact (obviously in OnCollisionEnter2D() 
			} else {
				if ((isFacingRight && distanceFromStart < patrolDiameter) || 
					(!isFacingRight && distanceFromStart <= 0)) {
					dx = 1;
				} else {
					dx = -1;
				}
				Vector3 patrolVector = new Vector3 (dx, 0, 0) * patrolSpeed * Time.deltaTime;
				if (Mathf.Abs (dx) > 0) {
					if (distanceFromStart >= patrolDiameter && isFacingRight) {
						flip ();
					} else if (distanceFromStart <= 0 && !isFacingRight) {
						flip ();
					}
					transform.position += patrolVector;
				}
				distanceFromStart += patrolVector.x;
			}
		} else if (!isFacingRight) {
			flip ();
		}
	}

	float getPlayerDistance() {
		float playerX = playerGameObject.transform.position.x;
		float playerY = playerGameObject.transform.position.y;
		float enemyX = transform.position.x;
		float enemyY = transform.position.y;
		return Mathf.Sqrt ((playerX - enemyX) * (playerX - enemyX) + (playerY - enemyY) * (playerY - enemyY));
	}

	// Flip the sprite when turning around.
	void flip() {
		isFacingRight = !isFacingRight;
		Vector3 scalar = transform.localScale;
		scalar.x *= -1;
		transform.localScale = scalar;
	}
}
