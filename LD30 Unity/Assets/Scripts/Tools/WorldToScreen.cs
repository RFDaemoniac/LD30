using UnityEngine;
using System.Collections;

public class WorldToScreen : MonoBehaviour {
	public float x;
	public float y;
	public float z;

	// Use this for initialization
	void Start () {
		transform.position = Camera.main.WorldToViewportPoint(new Vector3(x, y, z));
	}
}
