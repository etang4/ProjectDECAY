using UnityEngine;
using System.Collections;

public class SimpleEnemyScript : MonoBehaviour {

	public GameObject characterObject;
	private GameObject meatToFollow;
	private GameObject flareToFollow;

	public bool canMove = true;

	//ENEMY TYPES: "common", "carnivore", "blind", "mutated"
	public string enemyType;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		if (canMove && Vector3.Distance (transform.position, GameObject.FindGameObjectWithTag ("Player").transform.position) <= TopDownCharacterController.GetNoise ()*50f) {
						meatToFollow = GameObject.FindGameObjectWithTag ("meat");
						flareToFollow = GameObject.FindGameObjectWithTag ("flare");
						if (characterObject != null && flareToFollow == null && (meatToFollow == null || enemyType != "carnivore")) {
								transform.position = Vector3.MoveTowards (transform.position, characterObject.transform.position, 12 * Time.deltaTime);
						} else if (characterObject != null && flareToFollow != null) {
								transform.position = Vector3.MoveTowards (transform.position, flareToFollow.transform.position, 12 * Time.deltaTime);
						} else if (characterObject != null && enemyType == "carnivore" && meatToFollow != null) {
								transform.position = Vector3.MoveTowards (transform.position, meatToFollow.transform.position, 12 * Time.deltaTime);
						}
				}

		}

	public string GetEnemyType()
	{
		return enemyType;
	}

	public void SetCanMove(bool move)
	{
		canMove = move;
	}
}

