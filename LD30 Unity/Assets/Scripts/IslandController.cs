using UnityEngine;
using System.Collections;

public class IslandController : MonoBehaviour {
	public int health = 3;
	public int islandType; //1 - Grass, 2 - Stone

	float maxCamDistance = 30f;

	Vector3 islandVelocity;

	public bool connected; //True if the island is connected to the player

	// Use this for initialization
	void Start () {
		//Sets a random speed
		islandVelocity = new Vector3(Random.Range(-1 * GameConstants.islandMaxSpeed, GameConstants.islandMaxSpeed), Random.Range(-1 * GameConstants.islandMaxSpeed, GameConstants.islandMaxSpeed), 0f);
	}
	
	// Update is called once per frame
	void Update () {
		if(!connected && (Mathf.Abs(transform.position.x - GameConstants.camPos.x) > maxCamDistance || Mathf.Abs(transform.position.y - GameConstants.camPos.y) > maxCamDistance)) {
			Destroy(gameObject);
		}
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
