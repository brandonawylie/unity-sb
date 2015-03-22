using UnityEngine;
using System.Collections;

public class BasicEnemyController : MonoBehaviour {
	string[] powerupList = {"Powerup_Health", "Powerup_Damage", "Powerup_Reload"};

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
			//collider.enabled = false;
			rigidbody2D.gravityScale = 0;
			animator.SetBool("isDying", true);
			transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + .4f,transform.localPosition.z);
			transform.localScale = new Vector3(1,1,1);

			// TODO roll for a chance of each powerup
			// 1. roll for whether a powerup spawns
			// 2. roll for which powerup spawns
			// HEALTH > RATE OF FIRE
			string resource = powerupList[Random.Range (0, powerupList.Length)];
			GameObject go = Instantiate(Resources.Load(resource, typeof(GameObject))) as GameObject;
			go.transform.position = transform.position;
			go.transform.localPosition = new Vector3(go.transform.localPosition.x, go.transform.localPosition.y - .1f,go.transform.localPosition.z);
		}

		if (animator.GetCurrentAnimatorStateInfo(0).IsName("Graveyard")) {
			Destroy(gameObject);
		}

		// AI
		// Move towards player
		// TODO have zones where teh enemies potrol
		// TODO if player is close, then  use this within zones
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
