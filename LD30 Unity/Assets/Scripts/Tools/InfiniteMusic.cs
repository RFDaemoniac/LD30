using UnityEngine;
using System.Collections;

public class InfiniteMusic : MonoBehaviour {
	static float musicTime;
	public AudioSource audio;
	public int level;

	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource>();

		if(level != 1) {
			audio.time = musicTime;
		}
		else {
			audio.time = 0f;
		}
		audio.Play();
	}

	void Update() {
		musicTime = audio.time;
	}
}
