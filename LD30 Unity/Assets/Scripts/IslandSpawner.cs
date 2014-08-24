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
		clone = Instantiate(Resources.Load("Prefabs/Island_" + (islandSpawn + 1).ToString()), new Vector3(GameConstants.camPos.x + Random.Range(-1 * GameConstants.maxCamDistance, GameConstants.maxCamDistance), GameConstants.camPos.y + Random.Range(-1 * GameConstants.maxCamDistance, GameConstants.maxCamDistance), GameConstants.islandDepth), Quaternion.identity) as GameObject;
		clone.SendMessage("setVelocity", 1);
	}
}
