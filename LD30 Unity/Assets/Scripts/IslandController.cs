using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IslandController : MonoBehaviour {
	public int health = 3;
	public int islandType; //1 - Grass, 2 - Stone

	Vector3 islandVelocity;

	public bool connected; //True if the island is connected to the player

	Object explosion;
	public float explosionForce = 2f;

	List<IslandController> childIslands;
	IslandController parentIsland = null;

	// Use this for initialization
	void Start () {
		childIslands = new List<IslandController>();
	}
	
	// Update is called once per frame
	void Update () {
		//Destroys the island if it's too far from the camera
		if(!connected && (Mathf.Abs(transform.position.x - GameConstants.camPos.x) > GameConstants.maxCamDistance || Mathf.Abs(transform.position.y - GameConstants.camPos.y) > GameConstants.maxCamDistance)) {
			destroy();
		}

	}

	void OnTriggerEnter2D(Collider2D coll) {
		if(coll.gameObject.tag == "Island") {
			if(!connected) {
				coll.SendMessage("dealDamage", 2);
				destroy();
			}
		}
	}

	//Damage dealt to itself
	public void dealDamage(int dmg) {
		health -= dmg;

		if(health <= 0) {
			destroy();
		}
	}

	//A bridge is built if build is not 0
	public void connect(int build) {
		if(!connected) {
			if(build != 0) {
				PlayerController.islandFound = true;
			}

			connected = true;
			IslandSpawner.spawnIsland();
			WorldController.addConnectedIsland(islandVelocity);
			rigidbody2D.velocity = new Vector3(0f, 0f, 0f);
		}
	}

	public void setVelocity(int v) {
		//Sets a random island speed
		islandVelocity = new Vector3(Random.Range(-1 * GameConstants.islandMaxSpeed, GameConstants.islandMaxSpeed), Random.Range(-1 * GameConstants.islandMaxSpeed, GameConstants.islandMaxSpeed), 0f);

		if(v == 0) {
			rigidbody2D.velocity = new Vector3(0f, 0f, 0f);
		}
		else {
			rigidbody2D.velocity = islandVelocity + WorldController.worldVelocity;
		}
	}

	// push boolean forces the change, otherwise it only happens if you are root
	public void changeVelocity(bool push, Vector3 additionalVelocity) {
		if (parentIsland != null && !push) {
			parentIsland.changeVelocity(false, additionalVelocity);
		} else {
			rigidbody2D.velocity += new Vector2(additionalVelocity.x, additionalVelocity.y);
			foreach (IslandController island in childIslands) {
				island.changeVelocity(true, additionalVelocity);
			}
		}
	}

	void destroy() {
		explosion = Resources.Load("Prefabs/Explosion");

		//Starts an explosion animation
		Instantiate(explosion, transform.position, Quaternion.identity);

		//If close to the camera, do a small screen shake
		if(Mathf.Abs(transform.position.x - GameConstants.camPos.x) < 5 && Mathf.Abs(transform.position.y - GameConstants.camPos.y) < 5) {
			ScreenShake.startShake(0.1f, 0.2f);
		}

		if (childIslands != null) {
			foreach (IslandController island in childIslands) {
				island.disconnectIsland(this);
				if(gameObject != null) {
					Vector3 newVelocity = (island.transform.position - transform.position).normalized * explosionForce;
					island.changeVelocity(false, newVelocity);
				}
			}
		}

		//Deselects people on the island
		foreach(Transform child in transform) {
			if(child.gameObject.tag == "Person") {
				Debug.Log("HI2");
				child.gameObject.GetComponent<PeopleController>().deselect();
			}
		}

		IslandSpawner.spawnIsland();
		Destroy(gameObject);
	}

	public void connectIsland(IslandController island) {
		childIslands.Add(island);
	}

	public void setParent(IslandController island) {
		parentIsland = island;
	}

	public void disconnectIsland(IslandController island) {
		if (parentIsland == island) {
			parentIsland = null;
		} else {
			childIslands.Remove(island);
		}
	}
}
