using UnityEngine;
using System.Collections;

public class SpawnController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.S)) {
			GameObject go = Instantiate(Resources.Load("BasicEnemy", typeof(GameObject))) as GameObject;
			//BulletController goScript = go.GetComponent<BulletController>();
			//goScript.damage = bulletDamage;
			go.transform.position = transform.position;
		}
	}
}
