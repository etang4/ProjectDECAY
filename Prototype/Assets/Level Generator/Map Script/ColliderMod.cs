using UnityEngine;
using System.Collections;

public class ColliderMod : MonoBehaviour {

	public BoxCollider bc;
	public float x;
	public float y;
	public float z;

	// Use this for initialization
	void Start () {
		x = 32;
		y = 32;
		z = 32;
		bc.size = new Vector3 (x,y,z);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
