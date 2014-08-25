using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour {
	bool canDoDamage;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//Destroys the laser if it's too far from the camera
		if(Mathf.Abs(transform.position.x - GameConstants.camPos.x) > GameConstants.maxCamDistance || Mathf.Abs(transform.position.y - GameConstants.camPos.y) > GameConstants.maxCamDistance) {
			Destroy(gameObject);
		}
	}
	void OnTriggerExit2D(Collider2D coll) {
		if(coll.gameObject.tag == "Island" && !canDoDamage) {
			canDoDamage = true;
		}
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if(coll.gameObject.tag == "Island" && canDoDamage) {
			coll.SendMessage("dealDamage", 1);
			Destroy(gameObject);
		}
	}
}
