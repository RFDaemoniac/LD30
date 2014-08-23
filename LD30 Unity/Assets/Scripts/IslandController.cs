using UnityEngine;
using System.Collections;

public class IslandController : MonoBehaviour {
	public int health = 3;
	public int islandType; //1 - Grass, 2 - Stone

	Vector3 islandVelocity;

	public bool connected; //True if the island is connected to the player

	// Use this for initialization
	void Start () {
		//Sets a random speed
		islandVelocity = new Vector3(Random.Range(-1 * GameConstants.islandMaxSpeed, GameConstants.islandMaxSpeed), Random.Range(-1 * GameConstants.islandMaxSpeed, GameConstants.islandMaxSpeed), 0f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//A bridge is built if build is not 0
	public void connect(int build) {
		if(!connected) {
			if(build != 0) {
				PlayerController.islandFound = true;
			}

			connected = true;
			WorldController.addConnectedIsland(islandVelocity);
		}
	}
}
