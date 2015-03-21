using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
	// TODO add reload bar
	// TODO make controls more responsive
	// TODO give hp
	// TODO make enemy attacks decrement hp
	// TODO adjust these variables with power ups
	// TODO fix jumping sfx when stuck underneath platform

	float walkSpeed = 3.0f;
	float jumpSpeed = 5.0f;
	float shootSpeed = 3.0f;

	public float hp = 100;
	public float maxHP = 100;

	float bulletDamage = 10.0f;

	bool isFacingRight = true;

	float lastShootTime = 0;
	float reloadTime = 1;
	float shootKickback = 2.0f;

	protected Animator animator;
	protected List<GameObject> bullets;

	protected AudioSource jumpSound, shootSound;
	// Use this for initialization
	void Start () {
		bullets = new List<GameObject> ();
		animator = GetComponent<Animator> ();
		AudioSource[] audioSources = GetComponents<AudioSource>();
		jumpSound = audioSources[0];
		shootSound = audioSources[1];
	}
	
	// Control physics based stuff like velocity/position
	void FixedUpdate () {
		// Update the x according to horizontal input
		float dx = Input.GetAxisRaw("Horizontal");
		if (Mathf.Abs(dx) > 0) {
			Vector3 walkVector = new Vector3 (dx, 0, 0) * walkSpeed * Time.deltaTime;
			if (walkVector.x < 0 && isFacingRight) {
				flip ();
			} else if (walkVector.x > 0 && !isFacingRight) {
				flip ();
			}
			animator.SetBool ("isWalk", Mathf.Abs(walkVector.x) > 0);
			transform.position += walkVector;
		}

		// Update the y accoridng to the vertical input
		if (Input.GetKey(KeyCode.UpArrow) && rigidbody2D.velocity.y == 0) {
			jumpSound.Play ();
			rigidbody2D.AddForce (new Vector2 (0, jumpSpeed), ForceMode2D.Impulse);
		}
		animator.SetBool ("isJump", Mathf.Abs(rigidbody2D.velocity.y) >= .5);

	}

	// update non-physics stuff like shooting
	void Update () {
		bool isShoot = animator.GetBool("isShoot");
		if (isShoot) animator.SetBool("isShoot", false);

		
		if (Input.GetKey (KeyCode.Space) && !isShoot && Time.time - lastShootTime >= reloadTime) {
			shootSound.Play ();
			animator.SetBool("isShoot", true);
			lastShootTime = Time.time;

			// TODO set the correct position for the bullet to spawn
			GameObject go = Instantiate(Resources.Load("Bullet", typeof(GameObject))) as GameObject;
			BulletController goScript = go.GetComponent<BulletController>();
			goScript.damage = bulletDamage;
			go.transform.position = transform.position;
			Vector2 bulletForce = isFacingRight ? Vector2.right * shootSpeed : -Vector2.right * shootSpeed;
			go.rigidbody2D.AddForce(bulletForce, ForceMode2D.Impulse);
			bullets.Add (go);

			Vector2 bulletKickbackForce = new Vector2(isFacingRight ? -shootKickback : shootKickback, 0);
			rigidbody2D.AddForce(bulletKickbackForce, ForceMode2D.Impulse);

		}
	}

	void OnCollisionEnter2D(Collision2D collision){
		//print ("collision");
		if (collision.gameObject.tag == "BasicEnemy") {
			hp -= 10;
			print ("hit");
		}
	}
	


	// flip the player's sprite to walk left & right
	void flip() {
		isFacingRight = !isFacingRight;
		
		Vector3 scalar = transform.localScale;
		scalar.x *= -1;
		transform.localScale = scalar;
	}

}
