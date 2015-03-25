using UnityEngine;
using System.Collections;

public class SpawnController : MonoBehaviour {

	public float spawnRate = 1;
	float lastSpawn = 0;
	public string enemyType;
	public bool isSpawning = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - lastSpawn >= spawnRate && isSpawning) {
			lastSpawn = Time.time;
			GameObject go = Instantiate(Resources.Load(enemyType, typeof(GameObject))) as GameObject;
			//BulletController goScript = go.GetComponent<BulletController>();
			//goScript.damage = bulletDamage;
			go.transform.position = transform.position;
		}
	}
}
