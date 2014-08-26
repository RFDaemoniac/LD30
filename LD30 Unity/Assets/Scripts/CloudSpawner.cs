using UnityEngine;
using System.Collections;

public class CloudSpawner : MonoBehaviour {
	Object clouds;
	GameObject cloudClone;

	// Use this for initialization
	void Start () {
		clouds = Resources.Load("Prefabs/Clouds");
		InvokeRepeating("spawnCloud", 0f, 1.5f);
	}
	
	// Update is called once per frame
	void Update () {
	}

	void spawnCloud() {
		Vector2 v = WorldController.worldVelocity;

		//Finds the angle to build the bridge
		float xDiff = (v.x - GameConstants.camPos.x);
		float yDiff = (v.y - GameConstants.camPos.y);
		float heading = 0f;
		
		if(xDiff != 0) {
			heading = Mathf.Atan(yDiff / xDiff) * Mathf.Rad2Deg;
		}
		
		if(xDiff < 0) {
			heading += 180;
		}
		
		if(xDiff == 0) {
			if(yDiff > 0) {
				heading = 90f;
			}
			else if(yDiff < 0) {
				heading = 270f;
			}
		}

		heading = (heading + 180) + Random.Range(-30, 30);

		float z = Random.Range(0, 30);
		if(z < 10) {
			z = -5;
		}
		cloudClone = Instantiate(clouds, new Vector3(GameConstants.camPos.x + 20 * Mathf.Cos(heading * Mathf.Deg2Rad), GameConstants.camPos.y + 20 * Mathf.Sin(heading * Mathf.Deg2Rad), Random.Range(0, 30)), Quaternion.identity) as GameObject;

		v.x = v.x * Random.Range(2, 8) * -1;
		v.y = v.y * Random.Range(2, 8) * -1;

		cloudClone.rigidbody2D.velocity = v;
	}
}
