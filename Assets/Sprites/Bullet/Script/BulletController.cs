using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {

	protected Animator animator;
	public float damage = 10.0f;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		// TODO IsName might now work, if so hash graveyard, and check with dobule equals and nameHash
		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("Graveyard")) {
			Destroy(gameObject);
		}

		if (!renderer.isVisible) {
			Destroy(gameObject);
		}
	}

	void OnCollisionEnter2D(Collision2D collision){
		//print ("collision");
		if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Environment") {
			animator.SetBool("isImpact" ,true);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		//print ("collision");

		if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Environment") {
			animator.SetBool("isImpact" ,true);
			rigidbody2D.velocity = Vector2.zero;
		}

	}
}
