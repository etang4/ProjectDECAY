using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

	public GameObject cube;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Horizontal"))
		{
			cube.transform.position.Set (cube.transform.position.x + Input.GetAxis ("Horizontal"), cube.transform.position.y, 0);
		}
		if (Input.GetButtonDown ("Vertical")) 
		{
			cube.transform.position.Set (cube.transform.position.x, cube.transform.position.y + Input.GetAxis ("Vertical"), 0);
		}
	}
}
