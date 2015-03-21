using UnityEngine;
using System.Collections;

public class BasicEnemyController : MonoBehaviour {
	// TODO make the enemy try to kill the player
	// TODO correct position after scaling for the death explosion

	float hp = 10.0f;

	Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		bool isDying = animator.GetBool("isDying");
		if (hp <= 0 && !isDying) {
			print ("dying");
			animator.SetBool("isDying", true);
			transform.localScale = new Vector3(1,1,1);

		}

		if (animator.GetCurrentAnimatorStateInfo(0).IsName("Graveyard")) {
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {		
		if (other.gameObject.tag == "PlayerBullet") {
			BulletController otherScript = other.GetComponent<BulletController>();
			hp -= otherScript.damage;
		}
		
	}
}
