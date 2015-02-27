using UnityEngine;
using System.Collections;

public class PlayerGetsHit : MonoBehaviour {

	bool hurt;
	float lifeRemaining;
	float hurtStateTime;
	float hurtStateTimeRemaining;
	public MeshRenderer MRtoMessWith;

	// Use this for initialization
	void Start () {
		hurt = false;
		lifeRemaining = 5;
		hurtStateTime = 2;
		hurtStateTimeRemaining = hurtStateTime;
	}
	
	// Update is called once per frame
	void Update () {
		if(hurt){
			countdownHurtTime();
		}
	}
	void OnTriggerEnter(Collider collider) {
		if(!hurt && (collider.gameObject.tag =="enemy" || collider.gameObject.tag =="enemyCrow" )){
			if(collider.gameObject.tag =="enemy") lifeRemaining-=1;
			if(collider.gameObject.tag =="enemyCrow") lifeRemaining-=0.5f;
			if(lifeRemaining<=0){
				Debug.Log("YOU DIED");
				Destroy(this.gameObject);
			}
			startHurtState();
		}
    }
    void startHurtState(){
    	hurt = true;
    	hurtStateTimeRemaining = hurtStateTime;
    	Debug.Log("IT IS NOW HURT" + " Life Remaining is " + lifeRemaining);

    }
    void countdownHurtTime(){
    	hurtStateTimeRemaining-= Time.deltaTime;
    	MRtoMessWith.enabled = ((Time.frameCount%3)!=0) ? false : true;
    	if(hurtStateTimeRemaining<=0){
    		 hurt = false;
    		 Debug.Log("IT IS NO LONGER HURT");
    		 MRtoMessWith.enabled = true;
    	}
    }
}
