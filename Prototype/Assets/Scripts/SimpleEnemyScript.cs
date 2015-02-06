using UnityEngine;
using System.Collections;

public class SimpleEnemyScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	public GameObject characterObject;
	// Update is called once per frame
	void Update () {
		if(characterObject!=null)
        transform.position = Vector3.MoveTowards(transform.position, characterObject.transform.position, 12 * Time.deltaTime);
	}
}
