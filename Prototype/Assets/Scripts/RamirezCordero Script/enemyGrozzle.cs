using UnityEngine;
using System.Collections;

public class EnemyChaseScript : MonoBehaviour 
{
	public Transform target;
	public int moveSpeed;
	public int rotationSpeed;

	private Transform myTransform;

	void Awake()
	{
		//Automatically updates position
		myTransform = transform;
	}

	// Use this for initialization
	void Start () 
	{
		//Searches for object with "Player" Tag and locks on
		GameObject go = GameObject.FindGameObjectWithTag ("Player");
		target = go.transform;
	}
	
	// Update is called once per frame
	void Update ()
	{
		//Draws line towards target?
		//Debug.DrawLine(target.transform.position, myTransform.position, Color.white);

		//Look at target (3D Function)
		myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(target.position - myTransform.position), rotationSpeed * Time.deltaTime);

		//Moves towards target
		myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
	}
}
