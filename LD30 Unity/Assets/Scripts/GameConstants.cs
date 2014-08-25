using UnityEngine;
using System.Collections;

//Keeps track of game constants that should be changed
public class GameConstants : MonoBehaviour {

	//Layers
	public static int islandLayerMask = 1 << 8;
	public static int bridgeLayerMask = 1 << 9;
	public static int bubbleLayerMask = 1 << 10;
	public static int peopleLayerMask = 1 << 11;

	//Island
	public static float islandMaxSpeed = 0.2f;
	public static float explosionForce = 0.3f;
	public static float islandDepth = 10f;
	public static int numIslands = 3; //Number of island sprites in the game
	public static int numIslandTypes = 2; // number of island types in the game

	//Bridge
	public static float bridgeDepth = 9f;
	public static float bridgeLaserDepth = 15f;

	//Camera
	public static Vector3 camPos = new Vector3(0f, 0f, 0f);
	public static float maxCamDistance = 30f; //Maximum distance an object can be from the camera before it gets destroyed

	//People
	public static string[] names = {"Frank", "Robin", "Sonny", "Stephane", "Alec", "Matt", "Noel", "Chevy", "Rami", "JW", "Pedro", "Kyle", "Brandon", "Zach", "Joakim", "Cole", "Scott", "Jonas", "Monica", "Arden", "Jade", "Tara", "Amora", "Jenna", "Hann", "Adriel", "Myriame", "Heather", "Chelsea", "Zoe", "Karen", "Humaira", "Ashly", "Lucy", "Becca", "Heather", "Optar"};
	public static string[] islandTypes = {"Grass", "Stone"};
}
