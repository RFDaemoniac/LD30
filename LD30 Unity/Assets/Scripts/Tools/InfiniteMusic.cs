using UnityEngine;
using System.Collections;

public class InfiniteMusic : MonoBehaviour {
	static float musicTime;
	public AudioSource audioSound;
	public int level;

	// Use this for initialization
	void Start () {
		audioSound = GetComponent<AudioSource>();

		if(level != 1) {
			audioSound.time = musicTime;
		}
		else {
			audioSound.time = 0f;
		}
		audioSound.Play();
	}

	void Update() {
		musicTime = audioSound.time;
	}
}
