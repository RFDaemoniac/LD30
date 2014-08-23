using UnityEngine;
using System.Collections;

//Keeps track of game constants that should be changed
public class GameConstants : MonoBehaviour {

	//Layers
	public static int islandLayerMask = 1 << 8;

	//Island
	public static float islandMaxSpeed = 5f;
}
