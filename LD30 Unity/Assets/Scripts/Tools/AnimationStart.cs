using UnityEngine;
using System.Collections;

public class AnimationStart : MonoBehaviour {
	Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		float speedAdd = (float)(Random.Range(0, 21) - 10) / 100f;

		anim.speed = 1 + speedAdd;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
