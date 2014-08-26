using UnityEngine;
using System.Collections;

public class CloudController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Vector2 v = WorldController.worldVelocity;
		v.x *= Random.Range(0.2f, 2f);
		v.y *= Random.Range (0.2f, 2f);
		rigidbody2D.velocity = v;
	}
	
	// Update is called once per frame
	void Update () {
	}
}
