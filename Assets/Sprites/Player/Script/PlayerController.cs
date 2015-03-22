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
	float rollSpeed = 200.0f;
	
	// Upgradable stats
	public float hp = 100;
	public float maxHP = 100;
	public float bulletDamage = 10.0f;
	public float reloadTime = 1;
	public float shootKickback = 2.0f;
	
	protected Animator animator;
	protected List<GameObject> bullets;
	
	protected AudioSource jumpSound, shootSound, hurtSound, powerupSound, rollSound;
	
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
		if (Input.GetButton("Jump") && rigidbody2D.velocity.y == 0) {
			jumpSound.Play ();
			rigidbody2D.AddForce (new Vector2 (0, jumpSpeed), ForceMode2D.Impulse);
		}
		animator.SetBool ("isJump", Mathf.Abs(rigidbody2D.velocity.y) >= .5);
		
	}
	
	// update non-physics stuff like shooting
	void Update () {
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
			go.rigidbody2D.AddForce(bulletForce, ForceMode2D.Impulse);
			bullets.Add (go);
			
			Vector2 bulletKickbackForce = new Vector2(isFacingRight ? -shootKickback : shootKickback, 0);
			rigidbody2D.AddForce(bulletKickbackForce, ForceMode2D.Impulse);
			
		}
		
		// Rolling
		bool isRoll = animator.GetBool("isRoll");
		if (Input.GetButton ("Roll") && !isRoll) {
			rollSound.Play();
			animator.SetBool("isRoll", true);
			rollStartTime = Time.time;
			lastRollTickTime = rollStartTime;
			rigidbody2D.AddForce(new Vector2(isFacingRight ? rollSpeed : -rollSpeed, 0), ForceMode2D.Force);
			gameObject.layer = 10;
		}
		
		if (isRoll) {
			float delta = Time.time - rollStartTime;

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
	}

	void OnCollisionEnter2D(Collision2D collision){
		//print ("collision");
		if (collision.gameObject.tag == "BasicEnemy") {
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

	
	// flip the player's sprite to walk left & right
	void flip() {
		isFacingRight = !isFacingRight;
		
		Vector3 scalar = transform.localScale;
		scalar.x *= -1;
		transform.localScale = scalar;
	}
	
}
