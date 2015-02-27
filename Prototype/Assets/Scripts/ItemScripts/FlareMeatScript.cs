using UnityEngine;
using System.Collections;

/*Defines how long a flare will exist for*/
public class FlareMeatScript : MonoBehaviour {

	private float timeToStop = 10f;
	private float timeToDisappear = 80f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		//Stopping the flare
		if (timeToStop > 0f)
		{
			timeToStop -= Time.deltaTime*5f;
		}
		else if (gameObject.rigidbody.velocity.magnitude != 0f)
		{
			gameObject.rigidbody.velocity = new Vector3(0f, 0f, 0f);
		}

		//Destroying the Flare
		timeToDisappear -= Time.deltaTime*5f;
		if (timeToDisappear <= 0f)
		{
			Destroy (gameObject);
		}
	}
}
