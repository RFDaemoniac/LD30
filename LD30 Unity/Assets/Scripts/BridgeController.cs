using UnityEngine;
using System.Collections;

public class BridgeController : MonoBehaviour {
	public Vector3 startingPoint; //Starting coordinates of the bridge
	public Vector3 endPoint; //End coordinates of the bridge
	public GameObject startingIsland; //One of the island the bridge connects to
	public GameObject endIsland; //Other island the bridge connects to
	public float bridgeHeading; //Bridge's heading, used to rotate sprites when creating the bridge

	Object bridgeEndSprite;
	Object bridgePieceSprite;
	GameObject clone;

	// Use this for initialization
	void Start () {
		bridgeEndSprite = Resources.Load("Prefabs/BridgeEnd");
		bridgePieceSprite = Resources.Load("Prefabs/BridgePiece");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setStartingPoint(Vector3 start) {
		Vector3 tmp = start;
		tmp.z = GameConstants.bridgeDepth;
		startingPoint = tmp;
	}

	public void setEndPoint(Vector3 end) {
		Vector3 tmp = end;
		tmp.z = GameConstants.bridgeDepth;
		endPoint = tmp;
	}

	public void setStartingIsland(GameObject island) {
		startingIsland = island;
	}

	public void setEndIsland(GameObject island) {
		endIsland = island;
	}

	public void setBridgeHeading(float heading) {
		bridgeHeading = heading;
	}

	//Build the bridge
	public void buildBridge() {
		bridgeEndSprite = Resources.Load("Prefabs/BridgeEnd");
		bridgePieceSprite = Resources.Load("Prefabs/BridgePiece");

		//Sets the bridge's position
		Vector3 tmpPos;
		tmpPos.x = (startingPoint.x + endPoint.x) / 2;
		tmpPos.y = (startingPoint.y + endPoint.y) / 2;
		tmpPos.z = (startingPoint.z + endPoint.z) / 2;
		transform.position = tmpPos;

		float bridgeLength = Mathf.Sqrt(Mathf.Pow(startingPoint.x - endPoint.x, 2) + Mathf.Pow(startingPoint.y - endPoint.y, 2));

		//Builds the bridge's ends and sets them as this Bridge GameObject's children
		clone = Instantiate(bridgeEndSprite, startingPoint, Quaternion.identity) as GameObject;
		clone.transform.Rotate(0f, 0f, bridgeHeading);
		clone.transform.parent = transform;
		clone = Instantiate(bridgeEndSprite, endPoint, Quaternion.identity) as GameObject;
		clone.transform.Rotate(0f, 0f, bridgeHeading);
		clone.transform.parent = transform;

		//Builds the bridge's main piece and sets it as this Bridge GameObject's child
		clone = Instantiate(bridgePieceSprite, transform.position, Quaternion.identity) as GameObject;
		clone.transform.localScale = new Vector3(bridgeLength / 0.4f, 1f, 1f);
		clone.transform.Rotate(0f, 0f, bridgeHeading);
	}
}
