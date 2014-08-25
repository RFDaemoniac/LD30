using UnityEngine;
using System.Collections;

public class IslandSpawner : MonoBehaviour {
	Object people;

	public static float screenSize = 8f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static void spawnIsland() {
		GameObject clone;
		int islandSpawn = Random.Range(0, GameConstants.numIslands);
		Vector3 tmpPos = new Vector3(0f, 0f, GameConstants.islandDepth);

		//Finds a random position to spawn the island at a certain distance away from the camera
		if(Random.Range(0, 2) == 0) {
			tmpPos.x = GameConstants.camPos.x + Random.Range(screenSize, GameConstants.maxCamDistance);
		}
		else {
			tmpPos.x = GameConstants.camPos.x - Random.Range(screenSize, GameConstants.maxCamDistance);
		}

		if(Random.Range(0, 2) == 0) {
			tmpPos.y = GameConstants.camPos.y + Random.Range(screenSize, GameConstants.maxCamDistance);
		}
		else {
			tmpPos.y = GameConstants.camPos.y - Random.Range(screenSize, GameConstants.maxCamDistance);
		}

		clone = Instantiate(Resources.Load("Prefabs/Island_" + 	(islandSpawn + 1).ToString()), tmpPos, Quaternion.identity) as GameObject;
		clone.SendMessage("setVelocity", 1);
		if (islandSpawn <= 1) {
			clone.SendMessage("setType",1);
		} else if (islandSpawn == 3) {
			clone.SendMessage ("setType",2);
		}

		//Spawn from 0 to 2 people on the island
		int numPeople = Random.Range(0, 3);
		bool spawned;
		Vector3 randPos;
		float randLength;
		float randAngle;

		for(int i = 0; i < numPeople; i++) {
			spawned = false;
			while(!spawned) {
				randLength = Random.Range(0, 200) / 100f;
				randAngle = Random.Range(0, 360);
				randPos = clone.transform.position;
				randPos.x = clone.transform.position.x + randLength * Mathf.Cos(randAngle * Mathf.Deg2Rad);
				randPos.y = clone.transform.position.y + randLength * Mathf.Sin(randAngle * Mathf.Deg2Rad);
				randPos.z = 0f;
				RaycastHit2D hit = Physics2D.Raycast(randPos, new Vector2(0f, 0f), 0.1f, GameConstants.peopleLayerMask);

				if(hit.collider == null) {
					Instantiate(Resources.Load("Prefabs/People"), randPos, Quaternion.identity);
					spawned = true;
				}
			}
		}
	}
}
