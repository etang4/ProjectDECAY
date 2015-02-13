using UnityEngine;
using System.Collections;

public class PlayerGetsHit : MonoBehaviour {

	bool hurt;
	float lifeRemaining;
	float hurtStateTime;
	float hurtStateTimeRemaining;
	public MeshRenderer MRtoMessWith;
	static float damageAmount;

	// Use this for initialization
	void Start () {
		hurt = false;
		lifeRemaining = 5f;
		hurtStateTime = 2;
		damageAmount = 1f;
		hurtStateTimeRemaining = hurtStateTime;
	}
	
	// Update is called once per frame
	void Update () {
		if(hurt){
			countdownHurtTime();
		}
	}

	public static void SetDamage(float newDamage)
	{
		damageAmount = newDamage;
	}

	void OnTriggerEnter(Collider collider) {
		if(collider.gameObject.tag =="enemy"&& !hurt){
			lifeRemaining -= damageAmount;
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
