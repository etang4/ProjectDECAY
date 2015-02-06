using UnityEngine;
using System.Collections;

public class EnemyGetHitScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider collider) {
		if(collider.gameObject.tag =="sword") Destroy(this.gameObject);
		if(collider.gameObject.tag =="bullet") Destroy(this.gameObject);
    }
}
