using UnityEngine;
using System.Collections;

public class PersonController : MonoBehaviour {

	protected float personRaycastOffset = 0.1f; //Raycast offset from the player so it doesn't collide on top of the player
	protected float moveDistanceCheck = 0.2f;	//Checks forward to see if there is something in front of the direction the character is moving
	protected float personSpeed = 2f;

	public bool selected = false;
	public bool connected = false;
	protected bool usingAbility = false;

	protected Animator anim;

	protected int happiness; //0 to 3, 0 being sad and 3 being really happy
	protected GameObject bubbleClone;
	protected float bubbleYOffset = 0.5f;

	// Use this for initialization
	protected virtual void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		checkGround();

		if (!selected) {
			anim.SetBool ("Idle", true);
		}
		if(connected && selected && !usingAbility) {
			//Move left
			if(Input.GetKey(KeyCode.A)) {
				anim.SetBool ("Idle", false);
				anim.SetInteger("XDir", -2);
				anim.SetInteger ("YDir", -2);
				
				Vector3 rayOrigin = transform.position;
				rayOrigin.x -= personRaycastOffset;
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, new Vector2(-1 * personRaycastOffset, 0f), moveDistanceCheck, GameConstants.islandLayerMask);
				RaycastHit2D hitBridge = Physics2D.Raycast(rayOrigin, new Vector2(-1 * personRaycastOffset, 0f), moveDistanceCheck, GameConstants.bridgeLayerMask);
				//Checks if there is still island on the left
				if((hit.collider != null && hit.collider.gameObject.rigidbody2D.velocity.x == 0 && hit.collider.gameObject.rigidbody2D.velocity.y == 0) || hitBridge.collider != null) {
					Vector3 tmp = transform.position;
					tmp.x -= personSpeed * Time.deltaTime;
					transform.position = tmp;
				}
			}
			
			//Move right
			if(Input.GetKey(KeyCode.D)) {
				anim.SetBool ("Idle", false);
				anim.SetInteger("XDir", 2);
				anim.SetInteger ("YDir", -2);
				
				Vector3 rayOrigin = transform.position;
				rayOrigin.x += personRaycastOffset;
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, new Vector2(personRaycastOffset, 0f), moveDistanceCheck, GameConstants.islandLayerMask);
				RaycastHit2D hitBridge = Physics2D.Raycast(rayOrigin, new Vector2(personRaycastOffset, 0f), moveDistanceCheck, GameConstants.bridgeLayerMask);
				
				//Checks if there is still island on the right
				if((hit.collider != null && hit.collider.gameObject.rigidbody2D.velocity.x == 0 && hit.collider.gameObject.rigidbody2D.velocity.y == 0) || hitBridge.collider != null) {
					Vector3 tmp = transform.position;
					tmp.x += personSpeed * Time.deltaTime;
					transform.position = tmp;
				}
			}
			
			//Move up
			if(Input.GetKey(KeyCode.W)) {
				anim.SetBool ("Idle", false);
				anim.SetInteger("YDir", 2);
				
				Vector3 rayOrigin = transform.position;
				rayOrigin.y += personRaycastOffset;
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, new Vector2(0f, personRaycastOffset), moveDistanceCheck, GameConstants.islandLayerMask);
				RaycastHit2D hitBridge = Physics2D.Raycast(rayOrigin, new Vector2(0f, personRaycastOffset), moveDistanceCheck, GameConstants.bridgeLayerMask);
				
				//Checks if there is still island up
				if((hit.collider != null && hit.collider.gameObject.rigidbody2D.velocity.x == 0 && hit.collider.gameObject.rigidbody2D.velocity.y == 0) || hitBridge.collider != null) {
					Vector3 tmp = transform.position;
					tmp.y += personSpeed * Time.deltaTime;
					transform.position = tmp;
				}
			}
			
			//Move down
			if(Input.GetKey(KeyCode.S)) {
				anim.SetBool ("Idle", false);
				anim.SetInteger("YDir", -2);
				
				Vector3 rayOrigin = transform.position;
				rayOrigin.y -= personRaycastOffset;
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, new Vector2(0f, -1 * personRaycastOffset), moveDistanceCheck, GameConstants.islandLayerMask);
				RaycastHit2D hitBridge = Physics2D.Raycast(rayOrigin, new Vector2(0f, -1 * personRaycastOffset), moveDistanceCheck, GameConstants.bridgeLayerMask);
				
				//Checks if there is still island down
				if((hit.collider != null && hit.collider.gameObject.rigidbody2D.velocity.x == 0 && hit.collider.gameObject.rigidbody2D.velocity.y == 0) || hitBridge.collider != null) {
					Vector3 tmp = transform.position;
					tmp.y -= personSpeed * Time.deltaTime;
					transform.position = tmp;
				}
			}
			
			//Sets the idle animation if the player is not moving
			if(!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D)) {
				anim.SetBool ("Idle", true);
			}
		}
	}

	protected virtual void OnMouseDown() {
		if (!selected && connected) {
			selected = true;
			WorldController w = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<WorldController>();
			w.SendMessage("changeActive", this);

			calculateHappiness(bubbleClone);
		}
	}

	//Calculates the person's happiness and changes the bubble's sprite
	public void calculateHappiness(GameObject bubble) {

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

	//Sets the person as a child of the ground its standing on and adjusts the depth
	void checkGround() {
		//Checks for bridges
		RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(0f, 0f), 0.1f, GameConstants.bridgeLayerMask);
		if(hit.collider != null) {
			if(hit.collider.tag == "Bridge") {
				transform.parent = hit.collider.gameObject.transform.parent;

				//Vector3 tmpScale = hit.collider.gameObject.transform.localScale;
				//transform.localScale = new Vector3(1 / tmpScale.x, 1 / tmpScale.y, 1 / tmpScale.z);
			}
		}

		//Checks for islands
		hit = Physics2D.Raycast(transform.position, new Vector2(0f, 0f), 0.1f, GameConstants.islandLayerMask);
		if(hit.collider != null) {
			if(hit.collider.tag == "Island") {
				transform.parent = hit.collider.gameObject.transform;
			}
		}

		//Adjusts the depth of the person
		Vector3 tmpPos = transform.position;
		tmpPos.z = Camera.main.WorldToViewportPoint(transform.position).y;
		transform.position = tmpPos;
	}
}
