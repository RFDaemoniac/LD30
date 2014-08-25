using UnityEngine;
using System.Collections;

public class PeopleController : PersonController {

	public static int numValues = 2;

	public float[] values;
	public float valuesRange = 5f;

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

	public int ability; //0 - shield

	GameObject shieldClone;

	// Use this for initialization
	protected override void Start () {
		base.Start();
		selected = false;
		connected = false;

		ability = Random.Range(0, 1);

		// initialize values and preferences
		values = new float[numValues];
		for (int i = 0; i < numValues; i++) {
			values[i] = Random.Range(-1 * valuesRange, valuesRange);
		}

		name = GameConstants.names[Random.Range(0, GameConstants.names.Length)];
		islandPreference = Random.Range (1, GameConstants.numIslandTypes + 1);
		islandPreferenceStrength = Random.Range (0, 2f);
		populationPreferenceStrength = Random.Range (0, 2f);
		populationMin = Random.Range (0,6);
		if (populationMin >= 3) {
			populationMin -= Random.Range(0,3);
		}
		populationMax = populationMin + Random.Range (3,10);
		if (populationMax >= 10) {
			populationMax -= Random.Range (0,4);
		}
		if (Random.Range (0f,1f) > 0.5f) {
			populationFavorMin = true;
		} else {
			populationFavorMin = false;
		}
		aloneHappiness = Random.Range(0.5f, 3f);

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
			if(Input.GetKeyDown(KeyCode.E)) {
				//Shield
				if(ability == 0) {
					//Activate shield
					if(!usingAbility) {						//Check if they are standing on an island
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

						usingAbility = false;
						updateAbilityIcon();
					}
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
		string carOne = "";
		string carTwo = "";
		hudText[4] = "I prefer to be with " + populationMin + " to " + populationMax + " people.";

		if(values[0] >= 0) {
			carOne = "I like < " + values[0].ToString() + " >.";
		}
		else {
			carOne = "I dislike < " + values[0].ToString() + " >.";
		}

		if(values[1] >= 0) {
			carTwo = "I like < " + values[1].ToString() + " >.";
		}
		else {
			carTwo = "I disike < " + values[1].ToString() + " >.";
		}

		hudText[2] = carOne;
		hudText[3] = carTwo;

		GameObject hud = GameObject.FindGameObjectWithTag("HUD");
		hud.GetComponent<HUDController>().updateText(hudText);
	}

	void updateAbilityIcon() {
		GameObject abilityIcon = GameObject.FindGameObjectWithTag("AbilityIcon");

		//Shield
		if(ability == 0) {
			if(!usingAbility) {
				abilityIcon.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/ShieldIcon");
			}
			else {
				abilityIcon.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/ShieldIcon_Selected");
			}
		}
	}
}
