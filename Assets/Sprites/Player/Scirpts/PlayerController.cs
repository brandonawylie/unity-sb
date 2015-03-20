﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	float walkSpeed = 1.0f;
	float jumpSpeed = 1.0f;
	protected Animator animator;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		// Update the x according to horizontal input
		float dx = Input.GetAxisRaw("Horizontal");
		Vector3 walkVector = new Vector3 (dx, 0, 0) * walkSpeed * Time.deltaTime;
		animator.SetBool ("isWalk", Mathf.Abs(walkVector.x) > 0);
		transform.position += walkVector;
		/*
		// Update the y accoridng to the vertical input
		float dy = Input.GetAxisRaw("Vertical");
		animator.SetBool ("isJump", rigidbody2D.velocity.y == 0);
		if (rigidbody2D.velocity.y == 0) {
			rigidbody2D.AddForce (new Vector2 (0, jumpSpeed), ForceMode2D.Impulse);
		}
		*/
	}
}