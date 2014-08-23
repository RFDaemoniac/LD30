using UnityEngine;
using System.Collections;

//Changes the world
public class WorldController : MonoBehaviour {
	public static Vector3 worldVelocity;
	public static int numConnectedIslands = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static void setWorldVelocity(Vector3 v) {
		worldVelocity = v;
	}

	//Adds a connected island and changes the world's velocity
	public static void addConnectedIsland(Vector3 speed) {
		numConnectedIslands++;
		worldVelocity += new Vector3(speed[0] / numConnectedIslands, speed[1] / numConnectedIslands, speed[2] / numConnectedIslands);
	}

	//Removes a connected island and changes the world's velocity
	public static void removeConnectedIsland(Vector3 speed) {
		worldVelocity -= new Vector3(speed[0] / numConnectedIslands, speed[1] / numConnectedIslands, speed[2] / numConnectedIslands);
		numConnectedIslands--;
	}
}
