using UnityEngine;
using System.Collections;

//SIMPLY FOR TESTING
public class ToggleTypeScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown (KeyCode.RightArrow))
		{
			if(gameObject.tag == "enemy")
			{
				gameObject.tag = "carnivore";
				Debug.Log ("CARNIVORE");
			}
			else if(gameObject.tag == "carnivore")
			{
				gameObject.tag = "enemy";
				Debug.Log ("ENEMY");
			}
		}
	}
}
