using UnityEngine;
using System.Collections;

public class DestroyTimer : MonoBehaviour {
	public float destroyTimer;

	// Use this for initialization
	void Start () {
		Invoke("destroy", destroyTimer);
	}
	
	void destroy() {
		Destroy(gameObject);
	}
}
