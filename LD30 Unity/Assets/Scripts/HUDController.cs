using UnityEngine;
using System.Collections;

public class HUDController : MonoBehaviour {
	public GUIText name;
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
		name.text = "Name: " + str[0];
		island.text = "Island: " + str[1];
		carOne = str[2];
		carTwo = str[3];
		people = str[4];
	}
}
