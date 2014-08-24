using UnityEngine;
using System.Collections;

public class PeopleController : PersonController {

	// Use this for initialization
	void Start () {
		RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(0f, 0f), 0.1f, GameConstants.islandLayerMask);

		if(hit.collider != null) {
			if(hit.collider.tag == "Island") {
				transform.parent = hit.collider.gameObject.transform;
			}
		}
		else {
			Destroy(gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
