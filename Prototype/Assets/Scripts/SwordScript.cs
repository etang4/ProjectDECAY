using UnityEngine;
using System.Collections;

public class SwordScript : MonoBehaviour {


	// swing for a quarter of a second
	 public float targetTime = 0.25f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		timeDown();
		swing();
	}

	void swing(){
		transform.RotateAround(transform.parent.position, Vector3.up, -540 * Time.deltaTime);
	}

	void timeDown(){
		targetTime -= Time.deltaTime;
		if (targetTime <= 0.0f){
		    timerEnded();
		}
	}

	void timerEnded(){
		Destroy(this.gameObject);
	}
}




