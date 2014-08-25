using UnityEngine;
using System.Collections;

public class HUDController : MonoBehaviour {
	public GUIText nameText;
	public GUIText island;
	public GUIText carOne;
	public GUIText carTwo;
	public GUIText people;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//Name, island, caracteristic 1-2, how many people around
	public void updateText(string[] str) {
		nameText.text = "Name: " + str[0];
		island.text = "Island: " + str[1];
		carOne.text = str[2];
		carTwo.text = str[3];
		people.text = str[4];
	}
}
