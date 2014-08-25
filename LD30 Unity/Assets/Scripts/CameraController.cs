using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public GameObject player;

	public static float camDepth = -10;

	GameObject selectedUnit; //current selected unit
	float mouseCamMovementMultiplier = 0.25f;

	// Use this for initialization
	void Start () {
		selectedUnit = player;
	}
	
	// Update is called once per frame
	void Update () {
		//Finds the mouse position relative to the current camera position
		if(selectedUnit != null) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			Vector3 target = ray.origin;
			float xDiff = (target.x - selectedUnit.transform.position.x);
			float yDiff = (target.y - selectedUnit.transform.position.y);
			float mouseDist = Mathf.Sqrt(Mathf.Pow(xDiff, 2f) + Mathf.Pow(yDiff, 2f));
			float mouseHeading = 0f;

			//Finds the mouse heading with respect to the camera's origin
			if(xDiff != 0) {
				mouseHeading = Mathf.Atan(yDiff / xDiff) * Mathf.Rad2Deg;
			}

			if(xDiff < 0) {
				mouseHeading += 180;
			}

			if(xDiff == 0) {
				if(yDiff > 0) {
					mouseHeading = 90f;
				}
				else if(yDiff < 0) {
					mouseHeading = 270f;
				}
			}

			//Change camera position depending on mouse and selected unit position
			Vector3 tmpPos = selectedUnit.transform.position;
			tmpPos.z = camDepth;

			//Finds the camera offset due to the mouse
			if(mouseDist <= 4) {
				tmpPos.x += xDiff * mouseCamMovementMultiplier;
				tmpPos.y += yDiff * mouseCamMovementMultiplier;
			}
			//Caps the camera distance if the mouse is too far
			else {
				tmpPos.x += Mathf.Cos(mouseHeading * Mathf.Deg2Rad);
				tmpPos.y += Mathf.Sin(mouseHeading * Mathf.Deg2Rad);
			}

			// shows a little more of the direction that you're heading
			tmpPos.x += WorldController.worldVelocity.x * 4f;
			tmpPos.y += WorldController.worldVelocity.y * 2f;

			transform.position = tmpPos;
			GameConstants.camPos = transform.position;
		}
	}
}
