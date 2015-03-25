using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Spawner : MonoBehaviour {

	public string[] enemyPriorities;
	private Dictionary<int, string> dict;
	private int prioritySum;
	public bool keepSpawning;
	// seconds / spawn
	public float spawnRate = 1;
	private float nextSpawn = 0;

	// Use this for initialization
	void Start ()
	{
		prioritySum = 0;
		int currentPriorityNumber = 0;
		this.dict = new Dictionary<int, string>();
		foreach (string enemyPriority in enemyPriorities)
		{
			string[] tokens = enemyPriority.Split(new string[] { "," }, StringSplitOptions.None);
			string enemy = tokens[0];
			int priority = System.Int32.Parse(tokens[1]);
			dict.Add(currentPriorityNumber, enemy);
			currentPriorityNumber += priority;
		} 
		prioritySum = currentPriorityNumber;
		nextSpawn = Time.time + spawnRate;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Time.time > nextSpawn && keepSpawning)
		{
			SpawnEnemy();
			nextSpawn = Time.time + spawnRate;
			print ("next spawn: " + nextSpawn);
		}
	}

	void SpawnEnemy()
	{
		System.Random rnd = new System.Random ();
		print ("priority sum: " + prioritySum);
		int selectedEnemy = rnd.Next (0, prioritySum + 1);
		print ("selectedEnemy: " + selectedEnemy);
		while (!dict.ContainsKey(selectedEnemy) && dict.Count > 0)
		{
			selectedEnemy--;
			print("selected enemy: " + selectedEnemy);
		}
		string enemyToSpawn = dict [selectedEnemy];
		print ("spawing enemey: " + enemyToSpawn);
		GameObject spawnedEnemy = Instantiate(Resources.Load(enemyToSpawn, typeof(GameObject))) as GameObject;
		spawnedEnemy.transform.position = transform.position;

	}
}
