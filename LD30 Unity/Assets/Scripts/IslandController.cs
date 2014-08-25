using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IslandController : MonoBehaviour {
	public int health = 3;
	public int islandType; //1 - Grass, 2 - Stone

	Vector3 islandVelocity;

	public bool connected; //True if the island is connected to the player
	public bool invincible; //The island can't take damage if this is true

	Object explosion;
	public float explosionForce = 2f;
	GameObject islandCrack;

	public List<IslandController> childIslands;
	public IslandController parentIsland = null;

	// Use this for initialization
	void Start () {
		childIslands = new List<IslandController>();
	}
	
	// Update is called once per frame
	void Update () {
		adjustDepth();

		//Destroys the island if it's too far from the camera
		if(!connected && (Mathf.Abs(transform.position.x - GameConstants.camPos.x) > GameConstants.maxCamDistance || Mathf.Abs(transform.position.y - GameConstants.camPos.y) > GameConstants.maxCamDistance)) {
			destroy();
		}

	}

	void OnTriggerEnter2D(Collider2D coll) {
		if(coll.gameObject.tag == "Island") {
			if(!connected && coll.gameObject != null) {
				coll.SendMessage("dealDamage", 1);
				destroy();
			}
		}
	}

	//Damage dealt to itself
	public void dealDamage(int dmg) {
		if(!invincible) {
			if(health == 3) {
				addCrack();
			}
			health -= dmg;

			resizeCrack();

			if(health <= 0) {
				destroy();
			}
		}
		else {
			//Find the shield child
			foreach(Transform ppl in transform) {
				if(ppl.gameObject.tag == "Person") {
					foreach(Transform child in ppl.gameObject.transform) {
						if(child.gameObject.tag == "Shield") {
							child.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("Sprites/Shield")[1];
							Invoke("changeShieldSprite", 1);
						}
					}
				}
			}
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
		} 
		else if(parentIsland == null && !this.containsPlayer()) {
			rigidbody2D.velocity += new Vector2(additionalVelocity.x, additionalVelocity.y);
			foreach (IslandController island in childIslands) {
				if (island != null) {
					island.changeVelocity(true, additionalVelocity);
				}
			}
		}
		else if(push) {
			rigidbody2D.velocity += new Vector2(additionalVelocity.x, additionalVelocity.y);
			foreach (IslandController island in childIslands) {
				if (island != null) {
					island.changeVelocity(true, additionalVelocity);
				}
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

		//Disconnect from children
		if (childIslands != null) {
			foreach (IslandController island in childIslands) {
				island.disconnectIsland(this);
				if(gameObject != null && island != null) {
					Vector3 newVelocity = (island.transform.position - transform.position).normalized * explosionForce;
					island.changeVelocity(false, newVelocity);
				}
			}
		}

		//Disconnect from parent
		if(parentIsland != null) {
			parentIsland.disconnectIsland(this);
			Vector3 newVelocity = (parentIsland.transform.position - transform.position).normalized * explosionForce;
			parentIsland.changeVelocity(false, newVelocity);
		}

		//Deselects people on the island
		foreach(Transform child in transform) {
			if(child.gameObject.tag == "Person") {
				child.gameObject.GetComponent<PeopleController>().deselect();
			}
			if (child.gameObject.tag == "Player") {
				child.gameObject.GetComponent<PlayerController>().lose();
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

	//Checks if this subtree contains the player
	public bool containsPlayer() {
		GameObject playerIsland = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().previousIsland;
		if(this.gameObject == playerIsland) {
			return true;
		}
		for(int i = 0; i < childIslands.Count; i++) {
			if(childIslands[i].gameObject == playerIsland) {
				return true;
			}
		}
		return false;
	}

	void adjustDepth() {
		//Adjusts the depth of the island
		Vector3 tmpPos = transform.position;
		tmpPos.z = GameConstants.islandDepth + Camera.main.WorldToViewportPoint(transform.position).y;
		transform.position = tmpPos;
	}

	void addCrack() {
		float randLength = Random.Range(0, 100) / 100f;
		float randAngle = Random.Range(0, 360);
		Vector3 randPos = transform.position;
		randPos.x = transform.position.x + randLength * Mathf.Cos(randAngle * Mathf.Deg2Rad);
		randPos.y = transform.position.y + randLength * Mathf.Sin(randAngle * Mathf.Deg2Rad);
		randPos.z = transform.position.z - 0.001f;

		islandCrack = Instantiate(Resources.Load("Prefabs/IslandCrack"), randPos, Quaternion.identity) as GameObject;
		islandCrack.transform.parent = transform;
	}

	void resizeCrack() {
		if(islandCrack != null) {
			if(health == 2) {
				islandCrack.transform.localScale = new Vector3(2f, 2f, 1f);
			}
			else if(health == 1) {
				islandCrack.transform.localScale = new Vector3(3f, 3f, 1f);
			}
		}
	}

	public void setType(int type) {
		islandType = type;
	}

	void changeShieldSprite() {
		//Find the shield child
		foreach(Transform ppl in transform) {
			if(ppl.gameObject.tag == "Person") {
				foreach(Transform child in ppl.gameObject.transform) {
					if(child.gameObject.tag == "Shield") {
						child.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("Sprites/Shield")[0];
						Invoke("changeShieldSprite", 1);
					}
				}
			}
		}
	}
}
