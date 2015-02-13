using UnityEngine;
using System.Collections;

/*if player presses C, the player can move faster
 *and takes less damage*/

public class EpipenScript : MonoBehaviour {
	
	private bool canUse; //whether player can use Epipen or not

	// Use this for initialization
	void Start () {
		canUse = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.C) && canUse)
		{
			canUse = false;
			//Adjust speed of player and period of immunity
			TopDownCharacterController.SetMaxSpeed(50f);
			PlayerGetsHit.SetDamage(0.5f);
		}
	
	}
}
