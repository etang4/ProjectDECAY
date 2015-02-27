using UnityEngine;
using System.Collections;

/*If enabled, the player will have warning of nearby mutated enemies*/
public class GeigerCounterScript : MonoBehaviour {

	private GameObject[] enemies;
	private GameObject[] mutatedEnemies;
	private int filled;

	private int foundMutated = 0;

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
		filled = 0;
		enemies = GameObject.FindGameObjectsWithTag ("enemy");
		Debug.Log (enemies.Length);
		mutatedEnemies = new GameObject[enemies.Length];
		for (int i = 0; i < enemies.Length; i++)
		{
			if(enemies[i].GetComponent<SimpleEnemyScript>().GetEnemyType() == "mutated")
			{
				mutatedEnemies[i] = enemies[i];
				filled ++;
			}
		}


		for (int i = 0; i < filled; i++)
		{
			if (Vector3.Distance (mutatedEnemies[i].transform.position, gameObject.transform.position) < 50f)
			{
				foundMutated++;
			}
		}

		if(foundMutated > 0)
		{
			GameObject.FindGameObjectWithTag("warning").guiText.text = "Nearby";
			foundMutated = 0;
		}
		else
		{
			GameObject.FindGameObjectWithTag("warning").guiText.text = "";
		}

	}

}
