using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	float playerRaycastOffset = 0.1f; //Raycast offset from the player so it doesn't collide on top of the player
	float moveDistanceCheck = 0.2f;	//Checks forward to see if there is something in front of the direction the character is moving
	float playerSpeed = 2f;
	float bridgeInitialLength = 2; //Initial length of the raycast for building

	bool selected = true; //True if the main character is currently selected

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(selected) {

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

			//Clicks
			if(Input.GetMouseButtonDown(1)) {
				
			}

			//Resets build once the mouse is released
			if(Input.GetMouseButtonUp(1)) {

			}
		}
	}
}
