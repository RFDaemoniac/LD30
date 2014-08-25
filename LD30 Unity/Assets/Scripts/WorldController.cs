﻿using UnityEngine;
using System.Collections;

//Setups the world and keeps track of certain game states
public class WorldController : MonoBehaviour {
	public GameObject player;
	public static GUIText scoreText;

	public static Vector2 worldVelocity;
	public static int numConnectedIslands = 0;
	public static int score = 0;

	public PersonController activePerson;

	public GameObject selectionRing;

	GameObject clone;

	int numTotalIslands = 12;

	// Use this for initialization
	void Start () {
		//Places the player at the origin
		player.transform.position = new Vector3(0f, 0f, 0f);

		//Spawns an initial island at the origin
		int islandSpawn = Random.Range(0, GameConstants.numIslands);
		clone = Instantiate(Resources.Load("Prefabs/Island_" + (islandSpawn + 1).ToString()), new Vector3(0f, 0f, GameConstants.islandDepth), Quaternion.identity) as GameObject;
		clone.SendMessage("connect", 0, SendMessageOptions.RequireReceiver);
		clone.SendMessage("setVelocity", 0, SendMessageOptions.RequireReceiver);
		activePerson = player.GetComponent<PersonController>();
		changeActive(activePerson);
		setWorldVelocity(new Vector3(Random.Range (-1f, 1f), Random.Range (0f, 1f), 0f));
		if (worldVelocity.magnitude < 0.4f) {
			worldVelocity = worldVelocity.normalized * 0.4f;
		}

		//Spawn other islands randomly
		for(int i = 0; i < numTotalIslands; i++) {
			IslandSpawner.spawnIsland();
		}

		StartCoroutine(CheckLoss());

		scoreText = GameObject.FindGameObjectWithTag("Score").GetComponent<GUIText>();
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void changeActive(PersonController newPerson) {
		if (activePerson != null) {
			activePerson.selected = false;
		} 
		else {
			if (selectionRing != null) {
				selectionRing.renderer.enabled = true;
			}
		}
		activePerson = newPerson;
		activePerson.selected = true;

		Vector3 tmpPos = activePerson.transform.position;
		tmpPos.z += 0.01f;
		selectionRing.transform.position = tmpPos;
		selectionRing.transform.parent = activePerson.transform;
	}

	public static void setWorldVelocity(Vector3 v) {
		worldVelocity = v;
	}

	//Adds a connected island and changes the world's velocity
	public static void addConnectedIsland(Vector2 speed) {
		numConnectedIslands++;
		worldVelocity += new Vector2(speed[0] / numConnectedIslands, speed[1] / numConnectedIslands);
	}

	//Removes a connected island and changes the world's velocity
	public static void removeConnectedIsland(Vector2 speed) {
		worldVelocity -= new Vector2(speed[0] / numConnectedIslands, speed[1] / numConnectedIslands);
		numConnectedIslands--;
	}

	public IEnumerator CheckLoss() {
		while (true) {
			if (player == null) {
				// do endgame
			}
			yield return new WaitForSeconds(0.3f);
		}
	}

	//Adds/Removes points from the score
	public static void addScore(int x) {
		score += x;
		scoreText.text = "Score: " + score.ToString();
	}
}
