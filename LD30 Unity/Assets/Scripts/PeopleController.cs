using UnityEngine;
using System.Collections;

public class PeopleController : PersonController {

	public static int numValues = 2;

	public float[] values;
	public float valuesRange = 5f;

	float cleanFeelingType;
	float talkFeelingType;

	public string name;
	public int islandPreference;
	public float islandPreferenceStrength;
	public float aloneHappiness;
	public int populationMin;
	public int populationMax;
	public float populationPreferenceStrength;
	public bool populationFavorMin;

	protected int happiness; //0 to 3, 0 being sad and 3 being really happy
	protected GameObject bubbleClone;
	protected float bubbleYOffset = 1.25f;

	public int ability; //0 - 4 nothing, 5 shield, 6 & 7 shoot, 8 engine

	GameObject shieldClone;
	float gunCooldown = 5f;
	bool canShoot = true;
	float gunSpeed = 10f;
	float engineStrength = 0.1f;
	float maxEngineSpeed = 2f;

	// Use this for initialization
	protected override void Start () {
		base.Start();
		selected = false;
		connected = false;

		ability = Random.Range(0, 9);
		if (ability <= 4) {
			ability = 0; // nothing
		} else if (ability <= 6) {
			ability = 1; // shield
		} else if (ability <= 8) {
			ability = 2; // gun
		} else if (ability <= 9) {
			ability = 3; // engine
		}

		// initialize values and preferences
		values = new float[numValues];
		for (int i = 0; i < numValues; i++) {
			values[i] = Random.Range(-1 * valuesRange, valuesRange);
		}

		name = GameConstants.names[Random.Range(0, GameConstants.names.Length)];
		islandPreference = Random.Range (1, 6);
		islandPreference = (islandPreference < 5) ? 1 : 2;
		islandPreferenceStrength = Random.Range (-0.5f, 0.5f);
		populationPreferenceStrength = Random.Range (0, 0.5f);
		populationMin = Random.Range (1,6);
		if (populationMin >= 3) {
			populationMin -= Random.Range(0,3);
		}
		populationMax = populationMin + Random.Range (3,10);
		if (populationMax >= 7) {
			populationMax -= Random.Range (0,4);
		}
		while (populationMax <= populationMin) {
			populationMax++;
		}
		if (Random.Range (0f,1f) > 0.5f) {
			populationFavorMin = true;
		} else {
			populationFavorMin = false;
		}
		aloneHappiness = Random.Range(0.5f, 2f);
		gunCooldown = Random.Range (3f, 6f);
		gunSpeed = Random.Range (6f, 12f);
		engineStrength = Random.Range (0.1f, 0.3f);
		maxEngineSpeed = Random.Range (1f, 2f);

		talkFeelingType = Random.Range (0f, 1f);
		cleanFeelingType = Random.Range (0f, 1f);

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

		if(selected && connected) {
			if(Input.GetMouseButtonDown(1)) {
				//Shield
				if(ability == 1) {
					//Activate shield
					if(!usingAbility) {						
						//Check if they are standing on an island
						RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(0f, 0f), 0.1f, GameConstants.islandLayerMask);
						if(hit.collider != null) {
							if(hit.collider.tag == "Island") {
								shieldClone = Instantiate(Resources.Load("Prefabs/Shield"), hit.collider.gameObject.transform.position + new Vector3(0f, 0f, -0.1f), Quaternion.identity) as GameObject;
								shieldClone.transform.parent = transform;
								if(hit.collider != null)
									hit.collider.gameObject.GetComponent<IslandController>().invincible = true;

								usingAbility = true;
								updateAbilityIcon();
							}
						}
					}

					//Deactivate shield
					else {
						//Check if they are standing on an island
						if(shieldClone != null) {
							Destroy(shieldClone.gameObject);
						}

						RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(0f, 0f), 0.1f, GameConstants.islandLayerMask);
						if(hit.collider != null) {
							if(hit.collider.tag == "Island") {
								if(hit.collider != null) {
									hit.collider.gameObject.GetComponent<IslandController>().invincible = false;
								}
							}
						}

						usingAbility = false;
						updateAbilityIcon();
					}
				}

				//Gun
				else if(ability == 2) {
					if(canShoot) {
						//Finds the angle to build the bridge
						GameObject shotClone;
						Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
						Vector3 target = ray.origin;
						float xDiff = (target.x - transform.position.x);
						float yDiff = (target.y - transform.position.y);
						float angleToShoot = 0f;
						
						if(xDiff != 0) {
							angleToShoot = Mathf.Atan(yDiff / xDiff) * Mathf.Rad2Deg;
						}
						
						if(xDiff < 0) {
							angleToShoot += 180;
						}
						
						if(xDiff == 0) {
							if(yDiff > 0) {
								angleToShoot = 90f;
							}
							else if(yDiff < 0) {
								angleToShoot = 270f;
							}
						}

						shotClone = Instantiate(Resources.Load("Prefabs/Gun"), transform.position, Quaternion.identity) as GameObject;
						shotClone.transform.Rotate(new Vector3(0f, 0f, angleToShoot));
						shotClone.rigidbody2D.velocity = new Vector3(gunSpeed * Mathf.Cos(angleToShoot * Mathf.Deg2Rad), gunSpeed * Mathf.Sin(angleToShoot * Mathf.Deg2Rad), 0f);
						canShoot = false;
						Invoke("allowShooting", gunCooldown);
						updateAbilityIcon();
					}
				}
			}
			//Engine
			if (ability == 3 && Input.GetMouseButton(1)) {
				// you hold RMB for engine, not just press it
				usingAbility = true;
				Vector2 newWorldVelocity = WorldController.worldVelocity;
				if (Input.GetKey(KeyCode.D)) {
					newWorldVelocity += new Vector2(engineStrength * Time.deltaTime, 0f);
				}
				if (Input.GetKey (KeyCode.A)) {
					newWorldVelocity += new Vector2(-engineStrength * Time.deltaTime, 0f);
				}
				if (Input.GetKey (KeyCode.W)) {
					newWorldVelocity += new Vector2(0f, engineStrength * Time.deltaTime);
				}
				if (Input.GetKey (KeyCode.S)) {
					newWorldVelocity += new Vector2(0f, -1f * engineStrength * Time.deltaTime);
				}
				if (newWorldVelocity.magnitude > WorldController.worldVelocity.magnitude && newWorldVelocity.magnitude > maxEngineSpeed) {
					newWorldVelocity = newWorldVelocity.normalized * maxEngineSpeed;
				}
				WorldController.worldVelocity = newWorldVelocity;
			} else if (ability == 3) {
				usingAbility = false;
			}
		}
	}


	//Calculates the person's happiness and changes the bubble's sprite
	public void calculateHappiness(GameObject bubble) {
		float calc;
		float[] currentValues;
		float result = 0;
		float numPeople = 0;
		float newHappiness = 0;

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
			if(result <= valuesRange / 3) {
				newHappiness = 3;
			}
			else if(result <= 2 * valuesRange / 3) {
				newHappiness = 2;
			}
			else if(result <= valuesRange) {
				newHappiness = 1;
			}
			else {
				newHappiness = 0;
			}
		} else {
			newHappiness = aloneHappiness;
		}
		// Happiness is altered by population
		if (numPeople >= populationMin && numPeople <= populationMax) {
			newHappiness += populationPreferenceStrength;
		} else if (numPeople >= populationMax && populationFavorMin) {
			newHappiness -= populationPreferenceStrength/2f;
		} else if (numPeople <= populationMin && !populationFavorMin) {
			newHappiness -= populationPreferenceStrength/2f;
		}

		// and by terrain
		IslandController island = transform.GetComponentInParent<IslandController>();
		if (island != null) {
			if (island.islandType == islandPreference) {
				newHappiness += islandPreferenceStrength;
			} else {
				newHappiness -= islandPreferenceStrength/2f;
			}
		}
		happiness = (int) newHappiness;
		
		//Draws the bubble
		if(happiness <= 0) {
			bubble = Instantiate(Resources.Load("Prefabs/BubbleSad"), transform.position + new Vector3(0f, bubbleYOffset, 0f), Quaternion.identity) as GameObject;
		}
		else if(happiness == 1) {
			bubble = Instantiate(Resources.Load("Prefabs/BubbleHappyOne"), transform.position + new Vector3(0f, bubbleYOffset, 0f), Quaternion.identity) as GameObject;
		}
		else if(happiness == 2) {
			bubble = Instantiate(Resources.Load("Prefabs/BubbleHappyTwo"), transform.position + new Vector3(0f, bubbleYOffset, 0f), Quaternion.identity) as GameObject;
		}
		else if(happiness >= 3) {
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
		updateHUD();
		updateAbilityIcon();
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

	void updateHUD() {
		string[] hudText = new string[5];
		hudText[0] = name;
		hudText[1] = GameConstants.islandTypes[islandPreference - 1];
		hudText[4] = "I enjoy groups of " + populationMin + " to " + populationMax + " people.";
		
		if (values[0]/valuesRange >= 3/5) {
			if (talkFeelingType < 0.5f) {
				hudText[2] = "I demand constant conversation.";
			} else {
				hudText[2] = "You better be willing to gossip.";
			}
		} else if (values[0]/valuesRange >= 1/5) {
			if (talkFeelingType < 0.5f) {
				hudText[2] = "I appreciate sharing occasionally.";
			} else {
				hudText[2] = "Small talk means small ideas.";
			}
		} else if (values[0]/valuesRange >= -1/5) {
			if (talkFeelingType < 0.5f) {
				hudText[2] = "I don't mind listening.";
			} else {
				hudText[2] = "I value concise speech.";
			}
		} else if (values[0]/valuesRange >= -3/5) {
			if (talkFeelingType < 0.5f) {
				hudText[2] = "I prefer quiet.";
			} else {
				hudText[2] = "If you need to speak, please whisper.";
			}
		} else {
			if (talkFeelingType < 0.5f) {
				hudText[2] = "I need a standard of silence.";
			} else {
				hudText[2] = "I can't stand people gabbing.";
			}
		}
		if (values[1]/valuesRange >= 3/5) {
			if (cleanFeelingType < 0.333f) {
				hudText[3] = "I purge my surroundings regularly";
			} else if (cleanFeelingType < 0.6667f) {
				hudText[3] = "Everything must be spotless.";
			} else {
				hudText[3] = "Cleanliness is next to godliness.";
			}
		} else if (values[1]/valuesRange >= 1/5) {
			if (cleanFeelingType < 0.5f) {
				hudText[3] = "I actively maintain an organized environment.";
			} else {
				hudText[3] = "I won't clean up after your mess.";
			}
		} else if (values[1]/valuesRange >= -1/5) {
			if (talkFeelingType < 0.5f) {
				hudText[3] = "I limit my detritus.";
			} else {
				hudText[3] = "I'm flexible about my standards.";
			}
		} else if (values[1]/valuesRange >= -3/5) {
			if (cleanFeelingType < 0.5f) {
				hudText[3] = "I can't be bothered to clean.";
			} else {
				hudText[3] = "A little dirt never killed anybody.";
			}
		} else {
			if (cleanFeelingType < 0.5f) {
				hudText[3] = "I thrive in a chaotic environment.";
			} else {
				hudText[3] = "A clean house is a sign of a wasted life.";
		
			}
		}

		GameObject hud = GameObject.FindGameObjectWithTag("HUD");
		hud.GetComponent<HUDController>().updateText(hudText);
	}

	//Used as the cooldown for the shooting ability
	void allowShooting() {
		canShoot = true;
		updateAbilityIcon();
	}

	void updateAbilityIcon() {
		GameObject abilityIcon = GameObject.FindGameObjectWithTag("AbilityIcon");

		//No ability
		if (ability <= 0) {
			abilityIcon.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("");
		}
		//Shield
		else if(ability == 1) {
			if(!usingAbility) {
				abilityIcon.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/ShieldIcon");
			}
			else {
				abilityIcon.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/ShieldIcon_Selected");
			}
		}
		//Gun
		else if(ability == 2) {
			if(canShoot) {
				abilityIcon.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/GunIcon");
			}
			else {
				abilityIcon.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/GunIcon_Selected");
			}
		}
		// Engine
		else if (ability == 3) {
			if (!usingAbility) {
				abilityIcon.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/EngineIcon");
			} else {
				abilityIcon.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/EngineIcon_Selected");
			}
		}
	}
}
