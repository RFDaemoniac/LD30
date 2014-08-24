using UnityEngine;
using System.Collections;

public class Pulse : MonoBehaviour {
	public float speed;
	public float maxSize; //Needs to be above one

	bool growing;

	// Use this for initialization
	void Start () {
		growing = true;
	}
	
	// Update is called once per frame
	void Update () {
		//Grow
		if(growing) {
			transform.localScale += new Vector3(Time.deltaTime * speed, Time.deltaTime * speed, 0f);
			if(transform.localScale.x >= maxSize) {
				growing = false;
			}
		}

		//Shrink
		else {
			transform.localScale -= new Vector3(Time.deltaTime * speed, Time.deltaTime * speed, 0f);
			if(transform.localScale.x <= 1) {
				growing = true;
			}
		}
	}
}
