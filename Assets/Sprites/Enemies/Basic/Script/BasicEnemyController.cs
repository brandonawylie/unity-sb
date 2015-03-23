using UnityEngine;
using System.Collections;

public class BasicEnemyController : MonoBehaviour {
	string[] powerupList = {"Powerup_Health", "Powerup_Damage", "Powerup_Reload", "Powerup_Speed"};

	public float hp = 10.0f;

	public float damage = 1.0f;
	public float dyingScaleX = 1;
	public float dyingScaleY = 1;
	public float dyingOffsetX = 0;
	public float dyingOffsetY = 0.4f;

	public float upgradeOffsetX = 0;
	public float upgradeOffsetY = -0.1f;

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
			//collider2D.isTrigger = true;
			//collider.enabled = false;
			GetComponent<Rigidbody2D>().isKinematic = true;
			GetComponent<Collider2D>().enabled = false;
			GetComponent<Rigidbody2D>().gravityScale = 0;
			animator.SetBool("isDying", true);
			transform.localPosition = new Vector3(transform.localPosition.x + dyingOffsetX, transform.localPosition.y + dyingOffsetY,transform.localPosition.z);
			transform.localScale = new Vector3(dyingScaleX,dyingScaleY,1);

			// TODO roll for a chance of each powerup
			// 1. roll for whether a powerup spawns
			// 2. roll for which powerup spawns
			// HEALTH > RATE OF FIRE
			string resource = powerupList[Random.Range (0, powerupList.Length)];
			GameObject go = Instantiate(Resources.Load(resource, typeof(GameObject))) as GameObject;
			go.transform.position = transform.position;
			go.transform.localPosition = new Vector3(go.transform.localPosition.x + upgradeOffsetX, go.transform.localPosition.y + upgradeOffsetY,go.transform.localPosition.z);
		}

		if (animator.GetCurrentAnimatorStateInfo(0).IsName("Graveyard")) {
			Destroy(gameObject);
		}

	}


	void OnTriggerEnter2D(Collider2D other) {		
		if (other.gameObject.tag == "PlayerBullet") {
			BulletController otherScript = other.GetComponent<BulletController>();
			hp -= otherScript.damage;
			Vector3 distance = other.gameObject.transform.position - transform.position;
			distance.Normalize();

			GetComponent<Rigidbody2D>().AddForce(distance, ForceMode2D.Impulse);
		}
		
	}
}
