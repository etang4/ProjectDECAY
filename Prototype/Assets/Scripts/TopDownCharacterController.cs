using UnityEngine;
using System.Collections;

public class TopDownCharacterController : MonoBehaviour {

	public GameObject mine;
	private int numMines;

	float mPlayerRotation;
	static float mMaxSpeed;
	Vector2 velo;
	enum WeaponModes: int{sword, gun}
	int numOfWeaponModes =2;
	WeaponModes currentWeapon;


	// Use this for initialization
	void Start () {
		numMines = 3;
		mPlayerRotation= 0;
		mMaxSpeed = 20;
		velo = Vector2.zero;
		currentWeapon = WeaponModes.sword;
	}
	
	// Update is called once per frame
	void Update () {
		Move();
		Weapons();
		DropMine ();
	}

	/*player drops mine*/
	void DropMine()
	{
		if (Input.GetKeyDown (KeyCode.Q) && numMines > 0)
		{
			Instantiate(mine, transform.position, transform.rotation);
			numMines --;
		}
	}

	void Move(){
		velo = Vector2.zero;
		if (Input.GetKey(KeyCode.W)){
			velo.y+=1;
		}
		if (Input.GetKey(KeyCode.A)){
			velo.x-=1;
		}
		if (Input.GetKey(KeyCode.S)){
			velo.y-=1;
		}
		if (Input.GetKey(KeyCode.D)){
			velo.x+=1;
		}
		velo.Normalize();
		velo *= mMaxSpeed* Time.deltaTime;
		gameObject.transform.position+= new Vector3(velo.x, 0, velo.y);
		// rotate based on orientation of velo
		if (velo != Vector2.zero)transform.rotation = Quaternion.LookRotation(new Vector3(velo.x, 0, velo.y));
	}

	public GameObject swordPrefab;
	public GameObject currentSword;
	public GameObject cannonBallPrefab;
	public GameObject currentCannonBall;

	void Weapons(){
		if(Input.GetKeyUp(KeyCode.Tab)){
			if(currentWeapon== WeaponModes.sword){
				currentWeapon = WeaponModes.gun;
			}else{
				currentWeapon = WeaponModes.sword;
			}
		}
		if(Input.GetKeyDown(KeyCode.Space)){
			if(currentWeapon==WeaponModes.sword)sword();
			if(currentWeapon==WeaponModes.gun)cannon();
		}
	}

	void sword(){
		if(currentSword!=null) Destroy(currentSword);
		currentSword = Instantiate(swordPrefab, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
		currentSword.transform.parent = gameObject.transform;
		currentSword.transform.localPosition += new Vector3(2,0.5f,0);
	}


	void cannon(){
		currentCannonBall = Instantiate(cannonBallPrefab, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
		currentCannonBall.transform.parent = gameObject.transform;
		currentCannonBall.transform.localPosition += new Vector3(0,0.75f,2.5f);
		currentCannonBall.GetComponent<Rigidbody>().velocity = (gameObject.transform.forward+(Vector3.up *0.05f))* 40; 
		currentCannonBall.transform.parent = null;
	}

	/*Set a new max speed for player*/
	public static void SetMaxSpeed(float newSpeed)
	{
		mMaxSpeed = newSpeed;
	}

}
