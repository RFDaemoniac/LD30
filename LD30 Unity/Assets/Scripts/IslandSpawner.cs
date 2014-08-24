using UnityEngine;
using System.Collections;

public class IslandSpawner : MonoBehaviour {

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
			tmpPos.x = GameConstants.camPos.x + Random.Range(5, GameConstants.maxCamDistance);
		}
		else {
			tmpPos.x = GameConstants.camPos.x - Random.Range(5, GameConstants.maxCamDistance);
		}

		if(Random.Range(0, 2) == 0) {
			tmpPos.y = GameConstants.camPos.y + Random.Range(5, GameConstants.maxCamDistance);
		}
		else {
			tmpPos.y = GameConstants.camPos.y - Random.Range(5, GameConstants.maxCamDistance);
		}

		clone = Instantiate(Resources.Load("Prefabs/Island_" + (islandSpawn + 1).ToString()), tmpPos, Quaternion.identity) as GameObject;
		clone.SendMessage("setVelocity", 1);
	}
}
