using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
	// TODO add reload bar
	// TODO make controls more responsive
	// TODO fix jumping sfx when stuck underneath platform

	static float MAX_SPEED = 5.0f;
	float walkSpeed = 3.0f;
	float jumpSpeed = 5.0f;
	float shootSpeed = 3.0f;
	
	bool isFacingRight = true;
	
	float lastShootTime = 0;
	float rollStartTime = 0;
	float lastRollTickTime = 0;
	float rollTime = .5f;
	float rollSpeed = 5.0f;
	
	// Upgradable stats
	public float hp = 100;
	public float maxHP = 100;
	public float bulletDamage = 10.0f;
	public float reloadTime = 1;
	public float shootKickback = 2.0f;
	
	protected Animator animator;
	protected List<GameObject> bullets;
	
	protected AudioSource jumpSound, shootSound, hurtSound, powerupSound, rollSound;
	private bool onLadder;
	private bool isGrounded = false;
	bool isDead = false;
	bool rotatedWhenDead = false;
	
	// Use this for initialization
	void Start () {
		bullets = new List<GameObject> ();
		animator = GetComponent<Animator> ();
		AudioSource[] audioSources = GetComponents<AudioSource>();
		jumpSound = audioSources[0];
		shootSound = audioSources[1];
		hurtSound = audioSources[2];
		powerupSound = audioSources[3];
		rollSound = audioSources[4];
		onLadder = false;
	}
	
	// Control physics based stuff like velocity/position
	void FixedUpdate () {
		
		// if we are rolling, then there should be no movement input
		bool isRoll = animator.GetBool("isRoll");
		if (isRoll) return;
		
		// Update the x according to horizontal input
		float dx = Input.GetAxisRaw("Horizontal");
		Vector3 walkVector = new Vector3 (dx, 0, 0) * walkSpeed * Time.deltaTime;
		if (Mathf.Abs(dx) > 0) {
			if (walkVector.x < 0 && isFacingRight) {
				flip ();
			} else if (walkVector.x > 0 && !isFacingRight) {
				flip ();
			}
			transform.position += walkVector;
		}
		animator.SetBool ("isWalk", Mathf.Abs(walkVector.x) > 0);
		
		// Update the y accoridng to the vertical input
		if (!onLadder && Input.GetButton("Jump") && GetComponent<Rigidbody2D>().velocity.y == 0 && isGrounded) {
			jumpSound.Play ();
			GetComponent<Rigidbody2D>().AddForce (new Vector2 (0, jumpSpeed), ForceMode2D.Impulse);
		}
		animator.SetBool ("isJump", Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y) >= .5);
		
	}
	
	// update non-physics stuff like shooting
	void Update () {
		if (hp <= 0 && !isDead) {
			isDead = true;
			animator.SetBool("isDying", true);
		}

		if (animator.GetCurrentAnimatorStateInfo(0).IsName("Graveyard") && !rotatedWhenDead) {
			rotatedWhenDead = true;
			transform.Rotate(0, 0, 90);
		}

		RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), -Vector2.up, GetComponent<Collider2D>().bounds.extents.y + 0.1f, 1);
		isGrounded = hit.collider != null;
		bool isShoot = animator.GetBool("isShoot");
		if (isShoot) animator.SetBool("isShoot", false);
		
		// Shooting
		if (Input.GetButton("Shoot") && !isShoot && Time.time - lastShootTime >= reloadTime) {
			shootSound.Play ();
			animator.SetBool("isShoot", true);
			lastShootTime = Time.time;
			
			// TODO set the correct position for the bullet to spawn
			GameObject go = Instantiate(Resources.Load("Bullet", typeof(GameObject))) as GameObject;
			BulletController goScript = go.GetComponent<BulletController>();
			goScript.damage = bulletDamage;
			go.transform.position = transform.position;
			Vector2 bulletForce = isFacingRight ? Vector2.right * shootSpeed : -Vector2.right * shootSpeed;
			go.GetComponent<Rigidbody2D>().AddForce(bulletForce, ForceMode2D.Impulse);
			bullets.Add (go);
			
			Vector2 bulletKickbackForce = new Vector2(isFacingRight ? -shootKickback : shootKickback, 0);
			GetComponent<Rigidbody2D>().AddForce(bulletKickbackForce, ForceMode2D.Impulse);
			
		}
		
		// Rolling
		bool isRoll = animator.GetBool("isRoll");
		if (Input.GetButton ("Roll") && !isRoll) {
			rollSound.Play();
			animator.SetBool("isRoll", true);
			rollStartTime = Time.time;
			lastRollTickTime = rollStartTime;
			//rigidbody2D.AddForce(new Vector2(isFacingRight ? rollSpeed : -rollSpeed, 0), ForceMode2D.Force);
			gameObject.layer = 10;
		}
		
		if (isRoll) {
			float delta = Time.time - rollStartTime;
			transform.position += new Vector3(isFacingRight ? Time.deltaTime * rollSpeed : -Time.deltaTime * rollSpeed, 0, 0);
			if (delta >= rollTime) {
				gameObject.layer = 9;				
				animator.SetBool("isRoll", false);
				transform.rotation = Quaternion.identity;
			} else {
				float percentage = (Time.time - lastRollTickTime) / rollTime;
				transform.Rotate(0, 0, isFacingRight ? -percentage * 360.0f : percentage * 360.0f);
				lastRollTickTime = Time.time;
			}
		}

		if (onLadder) {
			float dy = 0;
			if (Input.GetKey ("up")) {
				dy = 1;
			} else if (Input.GetKey ("down")) {
				dy = -1;
			}
			Vector3 climbVector = new Vector3 (0, dy, 0) * walkSpeed * Time.deltaTime;
			transform.position += climbVector;
		}
	}

	void OnCollisionEnter2D(Collision2D collision){
		//print ("collision");
		if (collision.gameObject.tag == "BasicEnemy" || collision.gameObject.tag == "Enemy") {
			BasicEnemyController script = collision.gameObject.GetComponent<BasicEnemyController>();
			hp -= script.damage;
			hurtSound.Play ();
		}

		if (collision.gameObject.tag == "Powerup") {
			powerupSound.Play ();
			PowerupController otherScript = collision.gameObject.GetComponent<PowerupController>();
			string type = otherScript.type;
			if (type == "Health") {
				hp = Mathf.Clamp(hp + otherScript.healAmount, hp, maxHP);
			} else if(type == "Damage") {
				bulletDamage += otherScript.damageIncrease;
			} else if(type == "Reload") {
				reloadTime -= otherScript.reloadReduction;
			} else if(type == "Speed") {
				walkSpeed = Mathf.Clamp(walkSpeed + otherScript.speedIncrease, 0, MAX_SPEED);
			}
			
			Destroy(collision.gameObject);
			
		}
	}

	void OnCollisionStay2D(Collision2D collision) {
		string tag = collision.gameObject.tag;
		BoxCollider2D playerCollider = this.gameObject.GetComponent<BoxCollider2D> ();
		if (tag == "LadderPlatform" && onLadder) {
			playerCollider.isTrigger = true;
		}
	}
	
	void OnTriggerEnter2D(Collider2D trigger) {
		string tag = trigger.gameObject.tag;
		if (tag == "Ladder") {
			onLadder = true;
			this.GetComponent<Rigidbody2D>().isKinematic = true;
		}
		
		if (tag == "Environment" || tag == "LadderPlatform") {
			this.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
		}
	}
	
	void OnTriggerExit2D(Collider2D trigger) {
		string tag = trigger.gameObject.tag;
		if (tag == "Ladder") {
			onLadder = false;
			this.GetComponent<Rigidbody2D>().isKinematic = false;
			this.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
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
