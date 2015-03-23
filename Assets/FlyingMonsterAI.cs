using UnityEngine;
using System.Collections;

public class FlyingMonsterAI : MonoBehaviour {

	// The diameter of the area the monster patrols.
	public float patrolDiameter;
	public float patrolSpeed;
	public float chaseSpeed;
	public float attackSpeed;

	// When this is >= patrolDiameter, start moving left.
	// If it's <= 0, start moving right.
	private float distanceFromStart;
	private bool isFacingRight;
	private Animator animator;

	// Use this for initialization
	void Start () {
		distanceFromStart = 0f;
		isFacingRight = true;
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		bool isDying = animator.GetBool("isDying");
		if (!isDying) {
			float dx;
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
		} else if (!isFacingRight) {
			flip ();
		}
	}

	// Flip the sprite when turning around.
	void flip() {
		isFacingRight = !isFacingRight;
		Vector3 scalar = transform.localScale;
		scalar.x *= -1;
		transform.localScale = scalar;
	}
}
