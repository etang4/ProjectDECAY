using UnityEngine;
using System.Collections;

public class CannonballScript : MonoBehaviour {
	// swing for a quarter of a second
	 public float targetTime = 1.0f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		timeDown();
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

