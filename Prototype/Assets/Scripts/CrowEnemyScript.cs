using UnityEngine;
using System.Collections;

public class CrowEnemyScript : MonoBehaviour {

	enum eBehaviourState {pursue, wait, attack, reproduce};
	static int numberOfCrows = 0;

	eBehaviourState currentState;
	// Use this for initialization
	void Start () {
		currentState = eBehaviourState.wait;
		reportStateChange();
		numberOfCrows++;//maybe use count to determine whether stuff should be made
	}
	
	public GameObject characterObject;
	// Update is called once per frame
	void Update() {
		// Switch statement to behavriours
		if(characterObject!=null)distBetween = Vector3.Distance(characterObject.transform.position, gameObject.transform.position);
		switch(currentState){
			case eBehaviourState.pursue:
				pursue();
				break;
			case eBehaviourState.wait:
				wait();
				break;
			case eBehaviourState.attack:
				attack();
				break;
			case eBehaviourState.reproduce:
				reproduce();
				break;
		}
		
		if (currentState== eBehaviourState.pursue || currentState == eBehaviourState.attack){
			countdownToReproduce();
		}
		
	}

	Vector3 attackPoint = Vector3.zero;

	float aggroHorizon = 70;
	float attackHorizon = 30;
	float distBetween = 500;

	float attackRefractoryPeriod = 1;
	float timeSinceLastAttack = 0;

	void pursue(){
		if(characterObject!=null){
			transform.position = Vector3.MoveTowards(transform.position, characterObject.transform.position, 10 * Time.deltaTime);
		// check attack horizon vs distance see if you can attack, if so set the attack point
			timeSinceLastAttack+=Time.deltaTime;
			if ( attackHorizon > distBetween && timeSinceLastAttack>attackRefractoryPeriod){ //check if its close enough and if its too soon
				// set attack point and move there
				timeSinceLastAttack = 0;
				attackPoint = gameObject.transform.position + (2* (characterObject.transform.position- gameObject.transform.position ));
				currentState = eBehaviourState.attack;
				reportStateChange();
			}
		}
    }

	void wait(){
		//check aggro horizon vs distance and hope player comes into range
		//Debug.Log(distBetween);
		if(characterObject!=null && aggroHorizon>distBetween){
			currentState = eBehaviourState.pursue;
			reportStateChange();
		}
	}

	float reachEpsilon = 10;

	void attack(){
		// based on point set in pursue ease towards and away from point
		if( Vector3.Distance(attackPoint, gameObject.transform.position)<reachEpsilon){
			currentState = eBehaviourState.pursue;
			reportStateChange();
		}else{
			transform.position = Vector3.MoveTowards(transform.position, attackPoint, 40 * Time.deltaTime);
		}
	}


	float reproductionTimeFrame = 2;
	float reproductionCountup = 0;
	void reproduce(){
		// pause, go through animation and create new one
		reproductionCountup+=Time.deltaTime;
		if(reproductionCountup>reproductionTimeFrame){
			reproductionCountup = 0;
			//TODO: Create new Crow in space verified to be empty
			//TODO: Switch to waiting mode

			//rayvast in cardinal directions
			// first one thats not occupied have it create there
			bool hasGivenBirth = false;
			Vector3[] possibleDirections = new [] { Vector3.forward, Vector3.right, Vector3.down, Vector3.left};
			int directionIndex = Random.Range(0,4); // inclusive, exclusive
			Vector3 rayDirection;
			for(int i =0; i<4&& !hasGivenBirth; i++){
				rayDirection=possibleDirections[directionIndex];
				Debug.Log("Trying to reproduce in direction: " + rayDirection);
				if (!Physics.Raycast(transform.position, rayDirection, 20)){
					Instantiate(gameObject, gameObject.transform.position+ (13*rayDirection), gameObject.transform.rotation);
					hasGivenBirth = true;//break out
				}
				directionIndex = (directionIndex+1)%possibleDirections.Length;
			}



			currentState = eBehaviourState.wait;
			reportStateChange();
		}
	}


	// NOT A STATE: Actually a metacontext

	float reproductionCyclePeriod =5;
	float reproductionCycleTime = 0;
	void countdownToReproduce(){
		reproductionCycleTime += Time.deltaTime;
		if(reproductionCycleTime>reproductionCyclePeriod){
			reproductionCycleTime=0;
			currentState = eBehaviourState.reproduce;
			reportStateChange();
		}
	}

	void reportStateChange(){
		Debug.Log("The current state is now: " + currentState);
	}
}
