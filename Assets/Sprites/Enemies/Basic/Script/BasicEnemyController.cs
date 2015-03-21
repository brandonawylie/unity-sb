using UnityEngine;
using System.Collections;

public class BasicEnemyController : MonoBehaviour {
	// TODO make the enemy try to kill the player
	// TODO correct position after scaling for the death explosion

	float hp = 10.0f;
	float walkSpeed = 1.0f;
	public float damage = 1.0f;

	Animator animator;

	GameObject playerGameObject;

	// Use this for initialization
	void Start () {
		playerGameObject = GameObject.Find("Player");
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		bool isDying = animator.GetBool("isDying");
		if (hp <= 0 && !isDying) {
			collider2D.isTrigger = true;
			animator.SetBool("isDying", true);
			transform.localScale = new Vector3(1,1,1);
		}

		if (animator.GetCurrentAnimatorStateInfo(0).IsName("Graveyard")) {
			Destroy(gameObject);
		}

		// Move towards player
		Vector3 toPlayer = playerGameObject.transform.position - transform.position;
		toPlayer.Normalize();
		Vector3 walkVector = new Vector3 (toPlayer.x, 0, 0) * walkSpeed * Time.deltaTime;
		transform.position += walkVector;

	}

	void OnTriggerEnter2D(Collider2D other) {		
		if (other.gameObject.tag == "PlayerBullet") {
			BulletController otherScript = other.GetComponent<BulletController>();
			hp -= otherScript.damage;
		}
		
	}
}
