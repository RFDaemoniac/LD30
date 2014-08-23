﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	float playerRaycastOffset = 0.1f; //Raycast offset from the player so it doesn't collide on top of the player
	float moveDistanceCheck = 0.2f;	//Checks forward to see if there is something in front of the direction the character is moving
	float playerSpeed = 2f;
	float bridgeAngle = 0f; //Building bridge's angle
	float bridgeCurrentLength = 2f; //Current raycast for building the bridge
	float bridgeBuildSpeed = 1f; //Speed at which the bridge is building
	float bridgeBuildFastSpeed = 10f; //Speed at which the bridge is building when on an island
	float bridgeHeading; //Current building bridge heading
	Vector3 bridgeStart; //Bridge's starting position
	Vector3 bridgeEnd; //Bridge's end position
	GameObject bridgeStartingIsland; //Island you are standing on
	GameObject bridgeEndIsland; //Island you are trying to connect
	GameObject bridgeClone;
	bool moving = false;

	bool building = false; //True if currently building a bridge;
	bool selected = true; //True if the main character is currently selected

	Object bridge;
	Animator anim;

	public static bool islandFound = false;

	// Use this for initialization
	void Start () {
		bridge = Resources.Load("Prefabs/Bridge");

		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		//An island was found while building the bridge, connect the islands
		if(islandFound) {
			Debug.Log("Bridge starts at " + bridgeStart + " and ends at " + bridgeEnd);

			//Create the bridge
			bridgeClone = Instantiate(bridge, new Vector3(0f, 0f, GameConstants.bridgeDepth), Quaternion.identity) as GameObject;
			bridgeClone.SendMessage("setStartingPoint", bridgeStart);
			bridgeClone.SendMessage("setEndPoint", bridgeEnd);
			bridgeClone.SendMessage("setStartingIsland", bridgeStartingIsland);
			bridgeClone.SendMessage("setEndIsland", bridgeEndIsland);
			bridgeClone.SendMessage("setBridgeHeading", bridgeHeading);
			bridgeClone.SendMessage("buildBridge");


			islandFound = false;
			building = false;
		}

		if(selected) {
			if(!building) {
				//Move up
				if(Input.GetKey(KeyCode.W)) {
					Vector3 rayOrigin = transform.position;
					rayOrigin.y += playerRaycastOffset;
					RaycastHit2D hit = Physics2D.Raycast(rayOrigin, new Vector2(0f, playerRaycastOffset), moveDistanceCheck, GameConstants.islandLayerMask);
					RaycastHit2D hitBridge = Physics2D.Raycast(rayOrigin, new Vector2(0f, playerRaycastOffset), moveDistanceCheck, GameConstants.bridgeLayerMask);

					//Checks if there is still island up
					if(hit.collider != null || hitBridge.collider != null) {
						Vector3 tmp = transform.position;
						tmp.y += playerSpeed * Time.deltaTime;
						transform.position = tmp;

						moving = true;
						anim.SetInteger("Walk", 2);
					}
				}

				//Move left
				if(Input.GetKey(KeyCode.A)) {
					Vector3 rayOrigin = transform.position;
					rayOrigin.x -= playerRaycastOffset;
					RaycastHit2D hit = Physics2D.Raycast(rayOrigin, new Vector2(-1 * playerRaycastOffset, 0f), moveDistanceCheck, GameConstants.islandLayerMask);
					RaycastHit2D hitBridge = Physics2D.Raycast(rayOrigin, new Vector2(-1 * playerRaycastOffset, 0f), moveDistanceCheck, GameConstants.bridgeLayerMask);
					//Checks if there is still island on the left
					if(hit.collider != null || hitBridge.collider != null) {
						Vector3 tmp = transform.position;
						tmp.x -= playerSpeed * Time.deltaTime;
						transform.position = tmp;

						moving = true;
						anim.SetInteger("Walk", 1);
					}
				}

				//Move down
				if(Input.GetKey(KeyCode.S)) {
					Vector3 rayOrigin = transform.position;
					rayOrigin.y -= playerRaycastOffset;
					RaycastHit2D hit = Physics2D.Raycast(rayOrigin, new Vector2(0f, -1 * playerRaycastOffset), moveDistanceCheck, GameConstants.islandLayerMask);
					RaycastHit2D hitBridge = Physics2D.Raycast(rayOrigin, new Vector2(0f, -1 * playerRaycastOffset), moveDistanceCheck, GameConstants.bridgeLayerMask);

					//Checks if there is still island down
					if(hit.collider != null || hitBridge.collider != null) {
						Vector3 tmp = transform.position;
						tmp.y -= playerSpeed * Time.deltaTime;
						transform.position = tmp;

						moving = true;
						anim.SetInteger("Walk", 1);
					}
				}

				//Move right
				if(Input.GetKey(KeyCode.D)) {
					Vector3 rayOrigin = transform.position;
					rayOrigin.x += playerRaycastOffset;
					RaycastHit2D hit = Physics2D.Raycast(rayOrigin, new Vector2(playerRaycastOffset, 0f), moveDistanceCheck, GameConstants.islandLayerMask);
					RaycastHit2D hitBridge = Physics2D.Raycast(rayOrigin, new Vector2(playerRaycastOffset, 0f), moveDistanceCheck, GameConstants.bridgeLayerMask);

					//Checks if there is still island on the right
					if(hit.collider != null || hitBridge.collider != null) {
						Vector3 tmp = transform.position;
						tmp.x += playerSpeed * Time.deltaTime;
						transform.position = tmp;

						moving = true;
						anim.SetInteger("Walk", 2);
					}
				}

				//Sets the idle animation if the player is not moving
				if(!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D)) {
					if(anim.GetInteger("Walk") == 2)
						anim.SetInteger("Walk", -1);
					else if(anim.GetInteger("Walk") == 1)
						anim.SetInteger("Walk", 0);
				}
			}

			//Clicks
			if(Input.GetMouseButtonDown(1)) {
				building = true;

				//Finds the current island the player is standing on
				RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(0f, 0f), 0.1f, GameConstants.islandLayerMask);
				if(hit.collider != null) {
					if(hit.collider.tag == "Island") {
						bridgeStartingIsland = hit.collider.gameObject;
					}
				}

				//Finds the angle to build the bridge
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				Vector3 target = ray.origin;
				float xDiff = (target.x - transform.position.x);
				float yDiff = (target.y - transform.position.y);
				bridgeHeading = 0f;

				if(xDiff != 0) {
					bridgeHeading = Mathf.Atan(yDiff / xDiff) * Mathf.Rad2Deg;
				}

				if(xDiff < 0) {
					bridgeHeading += 180;
				}

				if(xDiff == 0) {
					if(yDiff > 0) {
						bridgeHeading = 90f;
					}
					else if(yDiff < 0) {
						bridgeHeading = 270f;
					}
				}
				bridgeAngle = bridgeHeading;
				bridgeCurrentLength = 0f;
				bridgeBuildSpeed = bridgeBuildFastSpeed;
				bridgeStart = transform.position;
				bridgeEnd = transform.position;

			}

			//Resets build once the mouse is released
			else if(Input.GetMouseButton(1)) {
				if(building) {
					Vector3 rayOrigin = transform.position + new Vector3(bridgeCurrentLength * Mathf.Cos(bridgeAngle * Mathf.Deg2Rad), bridgeCurrentLength * Mathf.Sin(bridgeAngle * Mathf.Deg2Rad), 0f);
					RaycastHit2D hit = Physics2D.Raycast(rayOrigin, new Vector2(0f, 0f), 0.1f, GameConstants.islandLayerMask);

					//Keeps track of the start point of a bridge
					if(bridgeBuildSpeed == bridgeBuildFastSpeed) {
						bridgeStart = rayOrigin;
					}

					//Checks at the end of the building bridge if there is a connection
					bridgeCurrentLength += Time.deltaTime * bridgeBuildSpeed;

					bridgeBuildSpeed = 1f;

					//Checks if there is still island on the left
					if(hit.collider != null) {
						if(hit.collider.tag == "Island") {
							bridgeBuildSpeed = bridgeBuildFastSpeed;

							//Attempts to connect with that island if it isn't connected already
							hit.collider.SendMessage("connect", 1, SendMessageOptions.RequireReceiver);
							bridgeEnd = rayOrigin;

							bridgeEndIsland = hit.collider.gameObject;
						}
					}
				}
			}

			if(Input.GetMouseButtonUp(1)) {
				building = false;
			}
		}
	}
}
