using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {

	protected Animator animator;

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
		if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Environment") {
			animator.SetBool("isImpact" ,true);
		}
	}
}
