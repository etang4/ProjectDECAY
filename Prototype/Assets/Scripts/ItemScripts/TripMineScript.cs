using UnityEngine;
using System.Collections;

/*when enemy runs into mine, enemy is killed*/
	public class TripMineScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter(Collider hit)
	{
		GameObject colliding = hit.gameObject;
		if (colliding.tag == "enemy") {
						Destroy (colliding);
						Destroy (gameObject);
				}
	}
}
