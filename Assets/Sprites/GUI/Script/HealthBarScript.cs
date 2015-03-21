using UnityEngine;
using System.Collections;

public class HealthBarScript : MonoBehaviour {
	

	public Texture backgroundTexture;
	public Texture foregroundTexture;
	public Texture frameTexture;
	
	public int healthWidth = 199;
	public int healthHeight = 30;
	
	public int healthMarginLeft = 41;
	public int healthMarginTop = 38;

	public int frameWidth = 266;
	public int frameHeight = 65;
	
	public int frameMarginLeft = 10;
	public int frameMarginTop  =10;
	
	GameObject playerGameObject;
	PlayerController playerScript;

	// Use this for initialization
	void Start () {
		playerGameObject = GameObject.FindGameObjectWithTag("Player");
		playerScript = playerGameObject.GetComponent<PlayerController>();
	}

	void OnGUI () {
		
		float percentHP = playerScript.hp / playerScript.maxHP;
		
		GUI.DrawTexture( new Rect(frameMarginLeft,frameMarginTop, frameMarginLeft + frameWidth, frameMarginTop + frameHeight), backgroundTexture, ScaleMode.ScaleToFit, true, 0 );
		
		GUI.DrawTexture( new Rect(healthMarginLeft,healthMarginTop,healthWidth * percentHP + healthMarginLeft, healthHeight), foregroundTexture, ScaleMode.ScaleAndCrop, true, 0 );
		
		GUI.DrawTexture( new Rect(frameMarginLeft,frameMarginTop, frameMarginLeft + frameWidth,frameMarginTop + frameHeight), frameTexture, ScaleMode.ScaleToFit, true, 0 );
		
	}
	



}
