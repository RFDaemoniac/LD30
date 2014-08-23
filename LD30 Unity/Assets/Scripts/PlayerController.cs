using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	float playerRaycastOffset = 0.1f; //Raycast offset from the player so it doesn't collide on top of the player
	float moveDistanceCheck = 0.2f;	//Checks forward to see if there is something in front of the direction the character is moving
	float playerSpeed = 2f;
	float bridgeAngle = 0f; //Building bridge's angle
	float bridgeCurrentLength = 2f; //Current raycast for building the bridge
	float bridgeBuildSpeed = 1f; //Speed at which the bridge is building
	float bridgeHeading; //Current building bridge heading

	bool building = false; //True if currently building a bridge;
	bool selected = true; //True if the main character is currently selected

	public static bool islandFound;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		//An island was found while building the bridge, connect the islands
		if(islandFound) {
			Debug.Log("Woo");
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

					//Checks if there is still island on the left
					if(hit.collider != null) {
						Vector3 tmp = transform.position;
						tmp.y += playerSpeed * Time.deltaTime;
						transform.position = tmp;
					}
				}

				//Move left
				if(Input.GetKey(KeyCode.A)) {
					Vector3 rayOrigin = transform.position;
					rayOrigin.x -= playerRaycastOffset;
					RaycastHit2D hit = Physics2D.Raycast(rayOrigin, new Vector2(-1 * playerRaycastOffset, 0f), moveDistanceCheck, GameConstants.islandLayerMask);
					//Checks if there is still island on the left
					if(hit.collider != null) {
						Vector3 tmp = transform.position;
						tmp.x -= playerSpeed * Time.deltaTime;
						transform.position = tmp;
					}
				}

				//Move down
				if(Input.GetKey(KeyCode.S)) {
					Vector3 rayOrigin = transform.position;
					rayOrigin.y -= playerRaycastOffset;
					RaycastHit2D hit = Physics2D.Raycast(rayOrigin, new Vector2(0f, -1 * playerRaycastOffset), moveDistanceCheck, GameConstants.islandLayerMask);

					//Checks if there is still island on the left
					if(hit.collider != null) {
						Vector3 tmp = transform.position;
						tmp.y -= playerSpeed * Time.deltaTime;
						transform.position = tmp;
					}
				}

				//Move right
				if(Input.GetKey(KeyCode.D)) {
					Vector3 rayOrigin = transform.position;
					rayOrigin.x += playerRaycastOffset;
					RaycastHit2D hit = Physics2D.Raycast(rayOrigin, new Vector2(playerRaycastOffset, 0f), moveDistanceCheck, GameConstants.islandLayerMask);

					//Checks if there is still island on the left
					if(hit.collider != null) {
						Vector3 tmp = transform.position;
						tmp.x += playerSpeed * Time.deltaTime;
						transform.position = tmp;
					}
				}
			}

			//Clicks
			if(Input.GetMouseButtonDown(1)) {
				building = true;

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
				Debug.Log(bridgeAngle);
			}

			//Resets build once the mouse is released
			else if(Input.GetMouseButton(1)) {
				//Checks at the end of the building bridge if there is a connection
				bridgeCurrentLength += Time.deltaTime * bridgeBuildSpeed;

				Vector3 rayOrigin = transform.position + new Vector3(bridgeCurrentLength * Mathf.Cos(bridgeAngle * Mathf.Deg2Rad), bridgeCurrentLength * Mathf.Sin(bridgeAngle * Mathf.Deg2Rad), 0f);
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, new Vector2(0f, 0f), 0.1f, GameConstants.islandLayerMask);

				bridgeBuildSpeed = 1f;

				//Checks if there is still island on the left
				if(hit.collider != null) {
					if(hit.collider.tag == "Island") {
						bridgeBuildSpeed = 10f;
						hit.collider.SendMessage("connect");
					}
				}
			}

			if(Input.GetMouseButtonUp(1)) {
				building = false;
			}
		}
	}
}
