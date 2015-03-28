using UnityEngine;
using System.Collections;

public class LadderScript : MonoBehaviour {

	// true if this is the bottom ladder in a chain of ladders (or if it is the only ladder in the chain).
	// false otherwise. Used to keep player from falling through floors.
	public bool isBottom;

	// This ladder's bottom.
	public GameObject LadderBottom;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
