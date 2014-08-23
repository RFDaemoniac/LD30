using UnityEngine;
using System.Collections;

public class IslandController : MonoBehaviour {
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

	public void connect() {
		if(!connected) {
			connected = true;
			PlayerController.islandFound = true;
			WorldController.addConnectedIsland(islandVelocity);
		}
	}
}
