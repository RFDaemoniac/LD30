using UnityEngine;
using System.Collections;

public class ScreenShake : MonoBehaviour {
	public static float shakeTime;
	public static float shakeIntensity;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(shakeTime > 0) {
			Vector3 tmp = Random.insideUnitSphere * shakeIntensity;
			tmp.z = -20f;
			transform.position = tmp;
			shakeTime -= Time.deltaTime;
		}
		else {
			transform.position = new Vector3(0f, 0f, -20f);
		}
	}

	public static void startShake(float timer, float intensity) {
		shakeTime = timer;
		shakeIntensity = intensity;
	}
}
