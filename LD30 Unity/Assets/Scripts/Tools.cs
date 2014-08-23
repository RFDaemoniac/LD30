using UnityEngine;
using System.Collections;

public class Tools : MonoBehaviour {

	//Input a world coordinate and outputs a screen position
	public static Vector3 worldToScreenPosition(Vector3 pos) {
		return Camera.main.WorldToViewportPoint(pos);
	}
}
