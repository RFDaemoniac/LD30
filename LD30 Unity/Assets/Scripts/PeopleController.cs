using UnityEngine;
using System.Collections;

public class PeopleController : PersonController {

	public static int numValues;

	public int[] values;

	// Use this for initialization
	protected override void Start () {
		base.Start();
		selected = false;
		connected = false;
		values = new int[numValues];
		foreach (int value in values) {
			value = Random.Range(-3, 3);
		}

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
}
