using UnityEngine;
using System.Collections;

public class CloudController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Vector2 v = WorldController.worldVelocity;
		v.x *= 10;
		v.y *= 10;
		rigidbody2D.velocity = v;
	}
	
	// Update is called once per frame
	void Update () {
	}
}
