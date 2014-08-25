using UnityEngine;
using System.Collections;

public class HTPController : MonoBehaviour {
	public GameObject htpSprite;
	public GameObject abilitySprite;

	public GUIText text1;
	public GUIText text2;
	public GUIText text3;
	public GUIText htpText;

	int phase;

	// Use this for initialization
	void Start () {
		phase = 0;
		htpSprite.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/HTP1");
		abilitySprite.transform.position = new Vector3(100f, 100f, 0f);
		text1.transform.position = Camera.main.WorldToViewportPoint(new Vector3(100f, 100f, 0f));
		text2.transform.position = Camera.main.WorldToViewportPoint(new Vector3(100f, 100f, 0f));
		text3.transform.position = Camera.main.WorldToViewportPoint(new Vector3(100f, 100f, 0f));
		htpText.transform.position = Camera.main.WorldToViewportPoint(new Vector3(0f, 6, 0f));
		htpText.text = "WASD to move around your selected character.";
	}
	
	// Update is called once per frame
	void Update () {
		if(phase == 0 && Input.GetKeyDown(KeyCode.Space)) {
			htpSprite.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/HTP2");
			htpText.text = "With the main character selected, right click other islands to connect them.\nEach person has a description that can be read below. Try to match them up!";
			phase++;
		}
		else if(phase == 1 && Input.GetKeyDown(KeyCode.Space)) {
			htpSprite.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/HTP3");
			htpText.text = "Make many groups of happy people by connecting your islands and managing them.";
			phase++;
		}
		else if(phase == 2 && Input.GetKeyDown(KeyCode.Space)) {
			htpSprite.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/HTP4");
			htpText.text = "Kill off some islands and if people are on islands that are drifting away, you\nwill be rewarded if their happiness has increased since they joined your crew!";
			phase++;
		}
		else if(phase == 3 && Input.GetKeyDown(KeyCode.Space)) {
			htpText.text = "";
			htpSprite.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("");
			abilitySprite.transform.position = new Vector3(0f, 0f, 0f);

			text1.transform.position = Camera.main.WorldToViewportPoint(new Vector3(-6f, 3f, 0f));
			text2.transform.position = Camera.main.WorldToViewportPoint(new Vector3(-6f, 0.5f, 0f));
			text3.transform.position = Camera.main.WorldToViewportPoint(new Vector3(-6f, -2f, 0f));


			phase++;
		}
		else if(phase == 4 && Input.GetKeyDown(KeyCode.Space)) {
			Application.LoadLevel("MainMenu");
		}
	}
}
