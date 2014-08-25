using UnityEngine;
using System.Collections;

public class PeopleController : PersonController {

	public static int numValues = 2;

	public float[] values;
	public float valuesRange = 5f;

	public int islandPreference;

	protected int happiness; //0 to 3, 0 being sad and 3 being really happy
	protected GameObject bubbleClone;
	protected float bubbleYOffset = 1.25f;

	// Use this for initialization
	protected override void Start () {
		base.Start();
		selected = false;
		connected = false;
		values = new float[numValues];
		for (int i = 0; i < numValues; i++) {
			values[i] = Random.Range(-1 * valuesRange, valuesRange);
		}
		islandPreference = Random.Range (0, GameConstants.numIslands);

		RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(0f, 0f), 0.1f, GameConstants.islandLayerMask);

		if(hit.collider != null) {
			if(hit.collider.tag == "Island") {
				transform.parent = hit.collider.gameObject.transform;
			}
		}
		else {
			deselect ();
			Destroy(gameObject);
		}
	}

	protected override void Update() {
		base.Update();

		//New update
		if(!connected) {
			RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(0f, 0f), 0.1f, GameConstants.islandLayerMask);
			if(hit.collider != null) {
				if(hit.collider.tag == "Island") {
					connected = hit.collider.GetComponent<IslandController>().connected;
				}
			}
		}
	}


	//Calculates the person's happiness and changes the bubble's sprite
	public void calculateHappiness(GameObject bubble) {
		float calc;
		float[] currentValues;
		float result = 0;
		float numPeople = 0;

		//Does a standard deviation of people's characteristics around this person
		GameObject[] p = GameObject.FindGameObjectsWithTag("Person");
		foreach(GameObject unit in p) {
			if(findDistance(transform.position, unit.transform.position) < 2) {
				numPeople++;

				currentValues = unit.gameObject.GetComponent<PeopleController>().values;

				calc = 0f;
				for(int i = 0; i < numValues; i++) {
					calc += Mathf.Pow(values[i] - currentValues[i], 2);
				}

				result += Mathf.Sqrt(calc / numValues);
			}
		}

		if(numPeople > 1) {
			result /= (numPeople - 1);
			Debug.Log(result);

			if(result <= valuesRange / 3) {
				happiness = 3;
			}
			else if(result <= 2 * valuesRange / 3) {
				happiness = 2;
			}
			else if(result <= valuesRange) {
				happiness = 1;
			}
			else {
				happiness = 0;
			}
		}
		
		//Draws the bubble
		if(happiness == 0) {
			bubble = Instantiate(Resources.Load("Prefabs/BubbleSad"), transform.position + new Vector3(0f, bubbleYOffset, 0f), Quaternion.identity) as GameObject;
		}
		else if(happiness == 1) {
			bubble = Instantiate(Resources.Load("Prefabs/BubbleHappyOne"), transform.position + new Vector3(0f, bubbleYOffset, 0f), Quaternion.identity) as GameObject;
		}
		else if(happiness == 2) {
			bubble = Instantiate(Resources.Load("Prefabs/BubbleHappyTwo"), transform.position + new Vector3(0f, bubbleYOffset, 0f), Quaternion.identity) as GameObject;
		}
		else if(happiness == 3) {
			bubble = Instantiate(Resources.Load("Prefabs/BubbleHappyThree"), transform.position + new Vector3(0f, bubbleYOffset, 0f), Quaternion.identity) as GameObject;
		}
		
		//Make the bubble the character the parent
		bubble.transform.parent = transform;
	}

	protected override void OnMouseDown() {
		base.OnMouseDown();
		if (selected) {
			calculateHappiness(bubbleClone);
		}
	}


	public void deselect() {
		// this avoids destroying the selection ring when a selected person dies
		if (selected) {
			GameObject selectionRing = GameObject.FindGameObjectWithTag("SelectionRing");
			if (selectionRing.transform.parent == transform) {
				selectionRing.transform.parent = null;
				selectionRing.renderer.enabled = false;
			}
		}
	}

	float findDistance(Vector3 dist1, Vector3 dist2) {
		return Mathf.Sqrt(Mathf.Pow(dist1.x - dist2.x, 2) + Mathf.Pow(dist1.y - dist2.y, 2));
	}
}
