using UnityEngine;
using System.Collections;

/*Defines all actions the player undertakes
 *in using various items*/
public class ItemUseScript : MonoBehaviour {

	//for item scrolling:
	private int selected = 0;
	private string[] items = {"*Air Freshener",
							  "*Botany Book",
							  "*Epipen",
							  "Extinguisher",
							  "*Meat",
							  "*Flare Gun",
							  "*Trip Mine",
							  "*Mutated Fruit",
							  "*Geiger Counter",
							  "*Muffling Rags",
							  "Kevlar Vest",
							  "Night-Vision Goggles"};

	//0 FOR AIR FRESHENER
	private bool canUseAirFreshener = true;

	//1 FOR BOTANY BOOK
	private bool canUseBook = true;
		//as well as variables for MUTATED FRUITS

	//2 FOR EPIPEN
	private bool canUseEpipen = true; //whether player can use Epipen or not

	//3 FOR EXTINGUISHER

	//4 FOR MEAT
	public GameObject meat;
	private bool hasMeat = true;
	private float meatSpeed = 10f;

	//5 FOR FLARE GUN
	private int numFlares = 2;
	public GameObject flare;
	public float flareSpeed = 20f;

	//6 FOR TRIP MINE
	public GameObject mine;
	private int numMines = 3;

	//7 FOR MUTATED FRUITS
	private int numFruits = 3;
	private float damage = 2f;
	private float lifeIncrease = 1f;
	private int plusFruits = 2;

	//8 FOR GEIGER COUNTER
	private bool canUseCounter = true;

	//9 FOR MUFFLING RAGS
	private bool canUseRags = true;

	//10 FOR KEVLAR VEST

	//11 FOR NIGHT-VISION GOGGLES

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		ItemScroll ();

		/*Allows player to use selected Item with I*/
		if (Input.GetKeyDown (KeyCode.I))
		{
			switch(selected)
			{
				case 0:
					UseAirFreshener();
					break;
				case 1:
					BotanyBook();
					break;
				case 2:
					Epipen();
					break;
				case 3:
					Extinguisher();
					break;
				case 4:
					ThrowMeat();
					break;
				case 5:
					UseFlare();
					break;
				case 6:
					DropMine();
					break;
				case 7:
					UseMutatedFruit();
					break;
				case 8:
					GeigerCounter();
					break;
				case 9:
					MufflingRags();
					break;
				case 10:
					KevlarVest();
					break;
				case 11:
					NightVision();
					break;
			}
		}
	}

	/*Item Scroll: By using the right or left arrow keys, Player
	 *can scroll through available items*/
	//MORE FOR TESTING
	void ItemScroll()
	{
		//Scrolls through items
		if (Input.GetKeyDown (KeyCode.RightArrow)) //scrolling right
		{
			selected++;
			if (selected >= 12)
			{
					selected = 0;
			}
			GameObject.FindGameObjectWithTag ("itemlist").guiText.text = items [selected];
		} 
		else if (Input.GetKeyDown (KeyCode.LeftArrow)) //scrolling left
		{
			selected --;
			if (selected <= -1)
			{
				selected = 11;
			}
			GameObject.FindGameObjectWithTag ("itemlist").guiText.text = items [selected];
		}
	
	}
	
	/*AIR FRESHENER: the player becomes invisible to blind enemies*/
	void UseAirFreshener()
	{
		canUseAirFreshener = false;
		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("enemy");
		for (int i = 0; i < enemies.Length; i++)
		{
			if(enemies[i].GetComponent<SimpleEnemyScript>().GetEnemyType() == "blind")
			{
				enemies[i].GetComponent<SimpleEnemyScript>().SetCanMove (false); //enemy doesn't move
			}
		}
	}

	/*BOTANY BOOK: the player will have more positive and less
	 *negative effects from mutated fruit*/
	//POTENTIALLY MORE TO THIS THAN IS HERE
	void BotanyBook()
	{
		if (canUseBook)
		{
			canUseBook = false;
			damage = 1f;
			lifeIncrease = 2f;
			plusFruits = 3;
		}
	}

	/*EPIPEN: the player can move faster and takes less damage*/
	void Epipen()
	{
		if (canUseEpipen)
		{
			canUseEpipen = false;
			//Adjust speed of player and amount of damage done
			TopDownCharacterController.SetMaxSpeed(50f);
			PlayerGetsHit.SetDamage(0.5f);
		}
	}

	/*EXTINGUISHER: player can spray extinguisher fluid*/
	void Extinguisher()
	{
	
	}

	/*FLARE GUN: player shoots flare to which enemies gather*/
	void UseFlare()
	{
		if (numFlares > 0)
		{
			GameObject shotFlare = Instantiate (flare, transform.position, transform.rotation) as GameObject;
			shotFlare.rigidbody.velocity = (gameObject.rigidbody.velocity + transform.forward*flareSpeed);
			numFlares --;
		}
	}

	/*MEAT: player throws meat to which carnivorous enemies gather*/
	void ThrowMeat()
	{
		if (hasMeat)
		{
			GameObject thrownMeat = Instantiate (meat, transform.position, transform.rotation) as GameObject;
			thrownMeat.rigidbody.velocity = (gameObject.rigidbody.velocity + transform.forward*meatSpeed);
			hasMeat = false;
		}
	}

	/*TRIP MINE: player drops mine*/
	void DropMine()
	{
		if (numMines > 0)
		{
			Instantiate(mine, transform.position, transform.rotation);
			numMines --;
		}
	}

	/*MUTATED FRUIT: player uses a mutated fruit to various effects*/
	void UseMutatedFruit()
	{
		if (numFruits != 0)
		{
			numFruits --;

			int effect = Random.Range (0, 3);
			switch (effect) {
			case 0: //Player will get hurt twice as much
					PlayerGetsHit.SetDamage (damage);
					Debug.Log ("HURT TWICE AS MUCH");
					break;
			case 1: //Player regains health
					PlayerGetsHit.IncreaseLife (lifeIncrease);
					Debug.Log ("LIFE INCREASE +1");
					break;
			case 2: //Player gets more fruit
					numFruits += plusFruits;
					Debug.Log ("TWO MORE FRUITS");
					break;
			}
		}
	}

	/*GEIGER COUNTER: warns player of nearby mutated enemy*/
	void GeigerCounter()
	{
		//This still has errors - will come up with a NullReferenceException for line 38 of GeigerCounterScript
		/*gameObject.GetComponent<GeigerCounterScript> ().enabled = true;
		canUseCounter = false;*/
	}

	/*MUFFLING RAGS: walking makes less noise*/
	void MufflingRags()
	{
		TopDownCharacterController.SetNoise (Random.Range(0.0f,2.0f));
		canUseRags = false;
	}

	/*KEVLAR VEST: protects player form projectiles*/
	void KevlarVest()
	{

	}

	/*NIGHT-VISION GOGGLES: player can see better in dark*/
	void NightVision()
	{

	}
}
