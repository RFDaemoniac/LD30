using UnityEngine;
using System.Collections;

public class OneOffSound : MonoBehaviour {

	float startTime;
	AudioSource audioSound;


	// Use this for initialization
	void Start () {
		audioSound = GetComponent<AudioSource>();
		audioSound.PlayOneShot(audioSound.clip);
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - (startTime + 0.2f) > audioSound.clip.length) {
			Destroy (gameObject);
		}
	}
}
