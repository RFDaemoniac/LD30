using UnityEngine;
using System.Collections;

public class Shake : MonoBehaviour {
	public float speed;
	public float shakeTimer;
	public float timesToShake;
	public bool shake;

	int numShake; //Number of times to shake
	int direction;
	float timer;

	// Use this for initialization
	void Start () {
		numShake = 0;
		shake = false;
		direction = 0;
		timer = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(shake && (numShake < timesToShake || timesToShake == 0)) {
			timer += Time.deltaTime;
			if(direction == 0) {
				transform.Rotate(new Vector3(0f, 0f, -1 * Time.deltaTime * speed));
				if(timer >= shakeTimer) {
					direction = 1;
					timer = 0f;
				}
			}
			if(direction == 1) {
				transform.Rotate(new Vector3(0f, 0f, Time.deltaTime * speed));
				if(timer >= 2 * shakeTimer) {
					direction = 2;
					timer = 0f;
				}
			}
			if(direction == 2) {
				transform.Rotate(new Vector3(0f, 0f, -1 * Time.deltaTime * speed));
				if(timer >= shakeTimer) {
					if(timesToShake != 0) {
						numShake++;
					}
					direction = 0;
					timer = 0f;
					transform.rotation = Quaternion.identity;
					if(numShake == timesToShake && timesToShake != 0) {
						shake = false;
						numShake = 0;
					}
				}
			}
		}
	}

	public void startShake() {
		if(!shake) {
			shake = true;
		}
	}
}
