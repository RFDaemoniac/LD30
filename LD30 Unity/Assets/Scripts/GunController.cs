using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour {
	bool canDoDamage;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
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
