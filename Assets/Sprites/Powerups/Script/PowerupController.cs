using UnityEngine;
using System.Collections;

public class PowerupController : MonoBehaviour {

	public string type;
	public int healAmount = 10;
	public float reloadReduction = .5f;
	public float damageIncrease = 1;

	float rotationCoeff = .05f;

	AudioSource bounceSound;

	// Use this for initialization
	void Start () {
		bounceSound = gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(0, 10.0f * Time.deltaTime / rotationCoeff, 0);
	}

	void OnCollisionEnter2D(Collision2D collision){
		//print ("collision");
		// TODO get velocity on impact
		// TODO scale sound occording to the velocity on impact
		if (collision.gameObject.layer == 0) {
			bounceSound.Play();
		}
	}
}
