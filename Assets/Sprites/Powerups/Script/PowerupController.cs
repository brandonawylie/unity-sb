using UnityEngine;
using System.Collections;

public class PowerupController : MonoBehaviour {

	public string type;
	public int healAmount = 10;
	public float reloadReduction = .5f;
	public float damageIncrease = 1;

	float rotationCoeff = .05f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(0, 10.0f * Time.deltaTime / rotationCoeff, 0);
	}
}
