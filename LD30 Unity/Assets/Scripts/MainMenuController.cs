using UnityEngine;
using System.Collections;

public class MainMenuController : MonoBehaviour {

	int option;
	int numOptions = 2;

	// Use this for initialization
	void Start () {
		option = 1;
		moveArrow ();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
			if(option != numOptions) {
				option++;
				moveArrow();
			}
		}
		else if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
			if(option != 1) {
				option--;
				moveArrow();
			}
		}

		//Check for a selection
		if(Input.GetKeyDown (KeyCode.Return)) {
			if(option == 1) {
				Application.LoadLevel("Game");
			}
			else {
				Application.LoadLevel("HowToPlay");
			}
		}
	}

	public void moveArrow() {
		if(option == 1) {
			transform.position = new Vector3(-3.5f, -1.66f, 0f);
		}
		else if(option == 2) {
			transform.position = new Vector3(-3.5f, -4.8f, 0f);
		}
	}
}
