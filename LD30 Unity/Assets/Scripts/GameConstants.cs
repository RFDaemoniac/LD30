using UnityEngine;
using System.Collections;

//Keeps track of game constants that should be changed
public class GameConstants : MonoBehaviour {

	//Layers
	public static int islandLayerMask = 1 << 8;
	public static int bridgeLayerMask = 1 << 9;
	public static int peopleLayerMask = 1 << 10;

	//Island
	public static float islandMaxSpeed = 0.5f;
	public static float islandDepth = 10f;
	public static int numIslands = 3; //Number of island sprites in the game

	//Bridge
	public static float bridgeDepth = 9f;
	public static float bridgeLaserDepth = 15f;

	//Camera
	public static Vector3 camPos = new Vector3(0f, 0f, 0f);
	public static float maxCamDistance = 30f; //Maximum distance an object can be from the camera before it gets destroyed
}
