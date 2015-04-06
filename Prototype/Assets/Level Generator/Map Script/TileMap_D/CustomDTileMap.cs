using UnityEngine;
using System.Collections.Generic;

public class CustomDTileMap {
	
	//Contains information about the room
	protected class DRoom {
		public int left;
		public int top;
		public int width;
		public int height;
		public bool hasWestDoor;
		public bool hasNorthDoor;
		public bool hasEastDoor;
		public bool hasSouthDoor;

		public bool firstRoom = false;
		
		//returns rightmost coordinate
		public int right {
			get {return left + width - 1;}
		}
		
		//returns leftmost coordinate
		public int bottom {
			get { return top + height - 1; }
		}
		
		//returns center x coordinate
		//Needed to connect the rooms
		public int center_x {
			get { return left + width/2; }
		}
		
		//returns center y coordinate
		//needed to connect the rooms
		public int center_y {
			get { return top + height/2; }
		}
	
	}
	//End of class DRoom

	//Global Variables
	bool[,] map_layout;
	GameObject wallGB;
	Transform wallsMGR;
	float tileSize;
	int size_z;

	int size_x;
	int size_y;
	int columnOfRooms;
	int rowOfRooms;
	int numOfRooms;
	

	//fields needed to generate map
	int currentCol;
	int currentRow;
	
	int[,] map_data;
	
	List<DRoom> rooms;
	
	/* Tile Types:
	Environmental Texture
	=====================
	 0 = unknown
	 1 = floor
	 2 = bloody floor
	 3 = Barrier
	 4 = Environmental Hazards
	 5 = Monster Spawn
	 6 = Objects
	 7 = Unknown Placeholder
	
	Gray Texture
	=============
	8 = North Gray Wall
	9 = South Gray Wall
	10 = West Gray Wall
	11 = East Gray Wall
	12 = NW Gray Corner
	13 = NE Gray Corner
	14 = SW Gray Corner
	15 = SE Gray Corner
	16 = North Gray Door
	17 = South Gray Door
	18 = West Gray Door
	19 = East Gray Door

	Normal Texture
	==============
	20 = North Normal Wall
	21 = South Normal Wall
	22 = West Normal Wall
	23 = East Normal Wall
	24 = NW Normal Corner
	25 = NE Normal Corner
	26 = SW Normal Corner
	27 = SE Normal Corner
	28 = North Normal Door
	29 = South Normal Door
	30 = West Normal Door
	31 = East Normal Door

	Bloody Texture
	==============
	32 = North Bloody Wall
	33 = South Bloody Wall
	34 = West Bloody Wall
	35 = East Bloody Wall
	36 = NW Bloody Corner
	37 = NE Bloody Corner
	38 = SW Bloody Corner
	39 = SE Bloody Corner
	40 = North Bloody Door
	41 = South Bloody Door
	42 = West Bloody Door
	43 = East Bloody Door

	 */
	enum tileType{
		Unknown, Floor, Bloody_Floor, Barrier, Environmental_Hazards, Monster_Spawn,
		Objects, Unknown2,
		N_Gray_Wall, S_Gray_Wall, W_Gray_Wall, E_Gray_Wall, 
		NW_Gray_Corner, NE_Gray_Corner, SW_Gray_Corner, SE_Gray_Corner,
		N_Gray_Door, S_Gray_Door, W_Gray_Door, E_Gray_Door,
		N_Normal_Wall, S_Normal_Wall, W_Normal_Wall, E_Normal_Wall,
		NW_Normal_Corner, NE_Normal_Corner, SW_Normal_Corner, SE_Normal_Corner,
		N_Normal_Door, S_Normal_Door, W_Normal_Door, E_Normal_Door,
		N_Bloody_Wall, S_Bloody_Wall, W_Bloody_Wall, E_Bloody_Wall, 
		NW_Bloody_Corner, NE_Bloody_Corner, SW_Bloody_Corner, SE_Bloody_Corner, 
		N_Bloody_Door, S_Bloody_Door, W_Bloody_Door, E_Bloody_Door
	};


	void UnitManager(){
		wallGB = GameObject.Find("ObjectManager").GetComponent<ObjectManager>().createWall();
		tileSize = GameObject.Find("TileMap").GetComponent<TileMap>().tileSize;
		size_z = GameObject.Find("TileMap").GetComponent<TileMap>().size_z;
		wallsMGR = GameObject.Find("Walls").transform;
	}
	
	public CustomDTileMap(int size_x, int size_y, int columnOfRooms, int rowOfRooms, int numOfRooms){
		UnitManager();
		DRoom r;
		this.size_x = size_x;
		this.size_y = size_y;
		this.columnOfRooms = columnOfRooms;
		this.rowOfRooms = rowOfRooms;
		this.numOfRooms = numOfRooms;
		
		map_data = new int[size_x,size_y];

		//make every tile unknown first.
		for(int x=0;x<size_x;x++) {
			for(int y=0;y<size_y;y++) {
				map_data[x,y] = (int) tileType.Unknown;
			}
		}
		
		//creating the map layout with a matrix first
		map_layout = new bool[columnOfRooms + 1, rowOfRooms + 1];

		currentCol = columnOfRooms/2;
		currentRow = 0;

		map_layout = generateMapMatrix(map_layout, numOfRooms);

		rooms = new List<DRoom>();
		
		//Giving room Data information to map the rooms.
		for(int col = 0; col <= columnOfRooms; col++){
			for(int row = 0; row <= rowOfRooms; row++){
				if(map_layout[col,row] == true){
					r = new DRoom();
					r.width = (size_x / (columnOfRooms + 1));
					r.height = (size_y / (rowOfRooms + 1));
					r.left = r.width * col;
					r.top = r.height * row;

					//if the room is the first room entered.
					//create entrance/exit
					if(row == 0 && (col == columnOfRooms/2)){
						//r.hasSouthDoor = true;
						r.firstRoom = true;
					}

					//add doorways variable
					if(col == 0){
						if (row == 0){
							createEastDoor(r,col,row);
							createNorthDoor(r, col, row);
						}
						else if (row == rowOfRooms){
							createEastDoor(r,col,row);
							createSouthDoor(r,col,row);
						}
						else{
							createEastDoor(r,col,row);
							createSouthDoor(r,col,row);
							createNorthDoor(r, col, row);
						}
					}
					else if (col == columnOfRooms){
						if (row == 0){
							createNorthDoor(r, col, row);
							createWestDoor(r,col,row);
						}
						else if (row == rowOfRooms){
							createWestDoor(r,col,row);
							createSouthDoor(r,col,row);
						}
						else{
							createWestDoor(r,col,row);
							createSouthDoor(r,col,row);
							createNorthDoor(r, col, row);
						}
					}
					else{
						if (row == 0){
							createNorthDoor(r, col, row);
							createWestDoor(r,col,row);
							createEastDoor(r,col,row);
						}
						else if (row == rowOfRooms){
							createWestDoor(r,col,row);
							createEastDoor(r,col,row);
							createSouthDoor(r,col,row);
						}
						else{
							createNorthDoor(r, col, row);
							createWestDoor(r,col,row);
							createEastDoor(r,col,row);
							createSouthDoor(r,col,row);
						}
					}

					//add room to room list
					rooms.Add(r);
				}
			}
		}

		//map the rooms on the mesh.
		foreach(DRoom r2 in rooms) {
			MakeRoom(r2);
		}
	}
	
	void createNorthDoor(DRoom r, int col, int row){
		if(map_layout[col,row + 1] == true){
			r.hasNorthDoor = true;
		}
	}

	void createSouthDoor(DRoom r, int col, int row){
		if(map_layout[col,row - 1] == true){
			r.hasSouthDoor = true;
		}
	}

	void createEastDoor(DRoom r, int col, int row){
		if(map_layout[col + 1,row] == true){
			r.hasEastDoor = true;
		}
	}

	void createWestDoor(DRoom r, int col, int row){
		if(map_layout[col - 1,row] == true){
			r.hasWestDoor = true;
		}
	}

	
	public int GetTileAt(int x, int y) {
		return map_data[x,y];
	}
	
	void MakeRoom(DRoom r) {
		if(r.firstRoom == true){
			makeEmptyRoom(r);
		}
		else{
			float value = Random.value;
			float ratio = 1/12f;
			Debug.Log(ratio);
			if(value >= 0f && value < ratio){
				makeSet1Body1Room(r);
			}
			else if(value >= ratio && value < ratio*2){
				makeSet1Body2Room(r);
			}
			else if(value >= ratio*2 && value < ratio*3){
				makeSet1Body3Room(r);
			}
			else if(value >= ratio*3 && value < ratio*4){
				makeSet1Body4Room(r);
			}
			else if(value >= ratio*4 && value < ratio*5){
				makeSet2Body1Room(r);
			}
			else if(value >= ratio*5 && value < ratio*6){
				makeSet2Body2Room(r);
			}
			else if(value >= ratio*6 && value < ratio*7){
				makeSet2Body3Room(r);
			}
			else if(value >= ratio*7 && value < ratio*8){
				makeSet2Body4Room(r);
			}
			else if(value >= ratio*8 && value < ratio*9){
				makeSet3Body1Room(r);
			}
			else if(value >= ratio*9 && value < ratio*10){
				makeSet3Body2Room(r);
			}
			else if(value >= ratio*10 && value < ratio*11){
				makeSet3Body3Room(r);
			}
			else{
				makeSet3Body4Room(r);
			}
		}
	}

	void makeEmptyRoom(DRoom r){
		//make floors and wall
		for(int x=0; x < r.width; x++) {
			for(int y=0; y < r.height; y++){
				//Debug.Log(x + " " + y);
				//use r property to create colliders
				if(addWallColliders(r, x, y)){
					//makes wall
				}
				//Make Content of Room
				else {
					map_data[r.left+x,r.top+y] = (int) tileType.Floor;
				}
			}
		}
		//use r property to create colliders
		
		makeDoors(r);
	}

	void makeSet1Body1Room(DRoom r){
		//make floors and wall
		for(int x=0; x < r.width; x++) {
			for(int y=0; y < r.height; y++){
				//Debug.Log(x + " " + y);
				//make wall
				//use r property to create colliders
				
				if(addWallColliders(r, x, y)){
					//makes wall
				}
				//Make Content of Room
				else if((x == 2 && (y >=3 && y <= 9)) ||
					(x == 10 && (y >=3 && y <= 9)) ||
					((x >= 4 && x <= 8) && y == 9) ||
					((x >= 4 && x <= 8) && y == 3) ||
					((x >= 5 && x <= 7) && y == 5) ||
					((x >= 5 && x <= 7) && y == 7)){
					map_data[r.left+x,r.top+y] = (int) tileType.Barrier;
					GameObject wall = createWall(r.left + x, r.top+y);
				}
				else if(x == 6 && y == 6){
					map_data[r.left+x,r.top+y] = (int) tileType.Objects;
				}
				else if((x >= 5 && x <= 7) && y == 4){
					map_data[r.left+x,r.top+y] = (int) tileType.Environmental_Hazards;
				}
				else {
					map_data[r.left+x,r.top+y] = (int) tileType.Floor;
				}
			}
		}
		//use r property to create colliders
		
		makeDoors(r);
	}

	void makeSet1Body2Room(DRoom r){
		//make floors and wall
		for(int x=0; x < r.width; x++) {
			for(int y=0; y < r.height; y++){
				//Debug.Log(x + " " + y);
				//make wall
				//use r property to create colliders
				
				if(addWallColliders(r, x, y)){
					//makes wall
				}
				//Make Content of Room
				else if(x >= 3 && x <=9 && y == 5||
					x >= 3 && x <=9 && y == 6){
					map_data[r.left+x,r.top+y] = (int) tileType.Barrier;
					GameObject wall = createWall(r.left + x, r.top+y);
				}
				else if(x == 10 && y == 6){
					map_data[r.left+x,r.top+y] = (int) tileType.Objects;
				}
				else if(x >= 2 && x <=10 && y == 2||
					x >= 2 && x <=10 && y == 10){
					map_data[r.left+x,r.top+y] = (int) tileType.Environmental_Hazards;
				}
				else if(x == 6 && (y == 4 || y == 7)){
					map_data[r.left+x,r.top+y] = (int) tileType.Monster_Spawn;
				}
				else {
					map_data[r.left+x,r.top+y] = (int) tileType.Floor;
				}
			}
		}
		//use r property to create colliders
		
		makeDoors(r);
	}

	void makeSet1Body3Room(DRoom r){
		//make floors and wall
		for(int x=0; x < r.width; x++) {
			for(int y=0; y < r.height; y++){
				//Debug.Log(x + " " + y);
				//make wall
				//use r property to create colliders
				
				if(addWallColliders(r, x, y)){
					//makes wall
				}
				//Make Content of Room
				else if(x >= 1 && x <=8 && y == 5|| x == 3 && y >= 1 && y <=3 ||
					((x == 2 || x == 4 || x == 6 || x == 8) && (y == 8 || y == 10)) ||
					(x == 7 &&y == 1) || (x == 8 && y == 2)||(x == 9 && y == 3)||
					(x == 10 && y == 4)){
					map_data[r.left+x,r.top+y] = (int) tileType.Barrier;
					GameObject wall = createWall(r.left + x, r.top+y);
				}
				else if (x == 8 && y == 1){
					map_data[r.left+x,r.top+y] = (int) tileType.Objects;
				}
				else if (((x == 9 || x == 10) && y == 2) || 
					((x == 10 || x == 11) && y == 1)){
					map_data[r.left+x,r.top+y] = (int) tileType.Environmental_Hazards;
				}
				else if((x == 2 && y == 1)||(x == 2 && y == 9)||(x == 5 && y ==9)){
					map_data[r.left+x,r.top+y] = (int) tileType.Monster_Spawn;
				}
				else {
					map_data[r.left+x,r.top+y] = (int) tileType.Floor;
				}
			}
		}
		//use r property to create colliders
		
		makeDoors(r);
	}

	void makeSet1Body4Room(DRoom r){
		//make floors and wall
		for(int x=0; x < r.width; x++) {
			for(int y=0; y < r.height; y++){
				//Debug.Log(x + " " + y);
				//make wall
				//use r property to create colliders
				
				if(addWallColliders(r, x, y)){
					//makes wall
				}
				//Make Content of Room
				else if(x != 6 && y != 6 && (x >=3 && x <= 9 && y >=3 && y <= 9)){
					map_data[r.left+x,r.top+y] = (int) tileType.Barrier;
					GameObject wall = createWall(r.left + x, r.top+y);
				}
				else if(x == 6 && y == 6){
					map_data[r.left+x,r.top+y] = (int) tileType.Objects;
				}
				else if((x == 5 || x == 7) && y == 6){
					map_data[r.left+x,r.top+y] = (int) tileType.Monster_Spawn;
				}
				else {
					map_data[r.left+x,r.top+y] = (int) tileType.Floor;
				}
			}
		}
		//use r property to create colliders
		
		makeDoors(r);
	}

	void makeSet2Body1Room(DRoom r){
		//make floors and wall
		for(int x=0; x < r.width; x++) {
			for(int y=0; y < r.height; y++){
				//Debug.Log(x + " " + y);
				//make wall
				//use r property to create colliders
				
				if(addWallColliders(r, x, y)){
					//makes wall
				}
				//Make Content of Room
				else if(((x == 1 || x ==2)&&(y == 1 || y ==2))||
					(y == 10 && x >= 1 && x <=5)|| (x == 11 && y == 7) ||
					(x == 9 && (y <= 11 && y >= 7)) || (x == 9 && y == 2)||
					(y == 3 && x <= 11 && x >=7)){
					map_data[r.left+x,r.top+y] = (int) tileType.Barrier;
					GameObject wall = createWall(r.left + x, r.top+y);
				}
				else if(x == 1 && y == 11){
					map_data[r.left+x,r.top+y] = (int) tileType.Objects;
				}
				else if((x == 2 && y == 11) || (x == 5 && y == 6) ||
					(x == 10 && y == 2) || (x == 10 && y == 10)){
					map_data[r.left+x,r.top+y] = (int) tileType.Monster_Spawn;
				}
				else if(x == 4 && (y >= 4 && y <= 8)){
					map_data[r.left+x,r.top+y] = (int) tileType.Environmental_Hazards;
				}
				else {
					map_data[r.left+x,r.top+y] = (int) tileType.Floor;
				}
			}
		}
		//use r property to create colliders
		
		makeDoors(r);
	}

	void makeSet2Body2Room(DRoom r){
		//make floors and wall
		for(int x=0; x < r.width; x++) {
			for(int y=0; y < r.height; y++){
				//Debug.Log(x + " " + y);
				//make wall
				//use r property to create colliders
				
				if(addWallColliders(r, x, y)){
					//makes wall
				}
				//Make Content of Room
				else if((x >= 1 && x <= 4 && y == 4) ||
					(x == 2 && y >= 6 && y <= 9) || ((x ==3 || x ==4) && y == 9) ||
					(x >= 8 && x <= 10 && (y ==3 || y==4)) ||
					(x == 10 && (y >=5 && y <= 9)) || ((x == 8 || x == 9) && y == 9)){
					map_data[r.left+x,r.top+y] = (int) tileType.Barrier;
					GameObject wall = createWall(r.left + x, r.top+y);
				}
				else if(x == 1 && y == 2 || x == 6 && y == 7){
					map_data[r.left+x,r.top+y] = (int) tileType.Objects;
				}
				else if((x == 2 && y == 1) || (x == 3 && y == 3) ||
					(x == 3 && y == 6) || (x == 9 && y == 8)){
					map_data[r.left+x,r.top+y] = (int) tileType.Monster_Spawn;
				}
				else if((x == 4 && (y == 1 || y == 3)) || 
					(x >= 5 && x <=7 && (y == 9 || y == 6)) ||
					(x == 5 && y == 7) || (x == 7 && y == 7)){
					map_data[r.left+x,r.top+y] = (int) tileType.Environmental_Hazards;
				}
				else {
					map_data[r.left+x,r.top+y] = (int) tileType.Floor;
				}
			}
		}
		//use r property to create colliders
		
		makeDoors(r);
	}

	void makeSet2Body3Room(DRoom r){
		//make floors and wall
		for(int x=0; x < r.width; x++) {
			for(int y=0; y < r.height; y++){
				//Debug.Log(x + " " + y);
				//make wall
				//use r property to create colliders
				
				if(addWallColliders(r, x, y)){
					//makes wall
				}
				//Make Content of Room
				else if((x == 3 && y <= 10 && y >= 7)||
					(y == 10 & x >= 4 && x <=6) ||
					(x >= 7 && x <= 9 && y == 4) ||
					(x == 9 && y >= 4 && y <= 6)){
					map_data[r.left+x,r.top+y] = (int) tileType.Barrier;
					GameObject wall = createWall(r.left + x, r.top+y);
				}
				else if(x == 6 & y == 7){
					map_data[r.left+x,r.top+y] = (int) tileType.Objects;
				}
				else if((x == 4 && y == 9) || (x == 8 && y == 5)){
					map_data[r.left+x,r.top+y] = (int) tileType.Monster_Spawn;
				}
				else if(((x != 4 && y != 9) || (x != 5 && y != 5) || (x != 6 & y != 7)) && 
					(x >= 4 && x <=8 && y >=5 && y <= 9) || 
					(x>=1 && x<=3 && y>=1 && y<=3) || (x >=10 && x <= 11 && y>= 10 && y<=11)){
					map_data[r.left+x,r.top+y] = (int) tileType.Environmental_Hazards;
				}
				else {
					map_data[r.left+x,r.top+y] = (int) tileType.Floor;
				}
			}
		}
		//use r property to create colliders
		
		makeDoors(r);
	}

	void makeSet2Body4Room(DRoom r){
		//make floors and wall
		for(int x=0; x < r.width; x++) {
			for(int y=0; y < r.height; y++){
				//Debug.Log(x + " " + y);
				//make wall
				//use r property to create colliders
				
				if(addWallColliders(r, x, y)){
					//makes wall
				}
				//Make Content of Room
				else if((x == 4 && (y == 2 ||y == 3)) || (x == 8 && y == 2) || 
					(x == 3 && y >= 2 && y <= 10 && y != 5) ||
					(x == 9 && y >= 2 && y <= 10 && y != 8) ||
					(y == 10 && x >= 4 && x <= 8 && x != 6)){
					map_data[r.left+x,r.top+y] = (int) tileType.Barrier;
					GameObject wall = createWall(r.left + x, r.top+y);
				}
				else if((x == 5 && y == 5) || (x == 7 && y == 7)){
					map_data[r.left+x,r.top+y] = (int) tileType.Monster_Spawn;
				}
				else if (x >=5 && x <= 6 && y >= 2 && y <= 3){
					map_data[r.left+x,r.top+y] = (int) tileType.Environmental_Hazards;
				}
				else {
					map_data[r.left+x,r.top+y] = (int) tileType.Floor;
				}
			}
		}
		//use r property to create colliders
		
		makeDoors(r);
	}

	void makeSet3Body1Room(DRoom r){
		//make floors and wall
		for(int x=0; x < r.width; x++) {
			for(int y=0; y < r.height; y++){
				//Debug.Log(x + " " + y);
				//make wall
				//use r property to create colliders
				
				if(addWallColliders(r, x, y)){
					//makes wall
				}
				//Make Content of Room
				else if(((x == 1 || x == 2) && (y ==4 || y == 10))||
					((x == 10 || x == 11) && (y == 2 || y == 8))||
					(x == 4 && (y != 1 && y != 2)) ||
					(x == 8 && (y != 10 && y != 11)) ||
					((x == 5 || x == 7) && y == 6)){
					map_data[r.left+x,r.top+y] = (int) tileType.Barrier;
					GameObject wall = createWall(r.left + x, r.top+y);
				}
				else if(x == 1 & y == 11){
					map_data[r.left+x,r.top+y] = (int) tileType.Objects;
				}
				else if((x == 5 && y == 5) || (x == 7 && y == 7) || (x ==11 && y == 1)){
					map_data[r.left+x,r.top+y] = (int) tileType.Monster_Spawn;
				}
				else if ((x >=1 && x <= 2 && y >= 1 && y <= 3) ||
					(x >=10 && x <= 11 && y >= 9 && y <= 11) ||
					((x == 5 || x == 6) && y == 3) || ((x == 6 || x == 7) && y == 9) ){
					map_data[r.left+x,r.top+y] = (int) tileType.Environmental_Hazards;
				}
				else {
					map_data[r.left+x,r.top+y] = (int) tileType.Floor;
				}
			}
		}
		//use r property to create colliders
		
		makeDoors(r);
	}

	void makeSet3Body2Room(DRoom r){
		//make floors and wall
		for(int x=0; x < r.width; x++) {
			for(int y=0; y < r.height; y++){
				//Debug.Log(x + " " + y);
				//make wall
				//use r property to create colliders
				
				if(addWallColliders(r, x, y)){
					//makes wall
				}
				//Make Content of Room
				else if((y == 3 && x >=3 && x<=9) || (y == 5 && x >=6 && x<=10)||
					(y == 9 && x >=2 && x<=6) || (x == 3 && y >=3 && y<=7) ||
					(x ==6 && (y == 6 || y == 8)) || (x >=9 && x <= 10 && y >=8 && y <= 9)){
					map_data[r.left+x,r.top+y] = (int) tileType.Barrier;
					GameObject wall = createWall(r.left + x, r.top+y);
				}
				else if(x == 5 & y == 6){
					map_data[r.left+x,r.top+y] = (int) tileType.Objects;
				}
				else if(x == 7 && y == 6){
					map_data[r.left+x,r.top+y] = (int) tileType.Monster_Spawn;
				}
				else if ((x == 4 && y == 4) || (x>= 5 && x <=7 && y == 7)){
					map_data[r.left+x,r.top+y] = (int) tileType.Environmental_Hazards;
				}
				else {
					map_data[r.left+x,r.top+y] = (int) tileType.Floor;
				}
			}
		}
		//use r property to create colliders
		
		makeDoors(r);
	}

	void makeSet3Body3Room(DRoom r){
		//make floors and wall
		for(int x=0; x < r.width; x++) {
			for(int y=0; y < r.height; y++){
				//Debug.Log(x + " " + y);
				//make wall
				//use r property to create colliders
				
				if(addWallColliders(r, x, y)){
					//makes wall
				}
				//Make Content of Room
				else if((x >= 1 && x <= 2 && y >=1 && y <= 3) ||
					(y >= 10 && y <= 11 && x >=1 && x <= 2) || 
					(x >= 8 && x <= 9 && y >= 10 && y <= 11) ||
					(x >= 2 && x <= 3 && y >=6 && y <= 7) || 
					(x >= 4 && x <= 5 && y >=6 && y <= 9) || 
					(x >= 9 && x <= 10 && y >=4 && y <= 5) ||
					(x == 5 && y >=2 && y <= 3) || (x == 6 && y >=2 && y <= 6) ||
					(x == 7 && y >=4 && y <= 7) || (x == 8 && y >=5 && y <= 8) ||
					(x == 11 && y >=7 && y <= 11) || (x == 5 && y == 5) ||
					(x == 9 && (y == 7 || y == 8))){
					map_data[r.left+x,r.top+y] = (int) tileType.Barrier;
					GameObject wall = createWall(r.left + x, r.top+y);
				}
				else if(x == 6 & y == 7){
					map_data[r.left+x,r.top+y] = (int) tileType.Objects;
				}
				else if(x == 8 && y == 4){
					map_data[r.left+x,r.top+y] = (int) tileType.Monster_Spawn;
				}
				else if ((x == 5 && y == 4) || (x ==9 && y == 6)){
					map_data[r.left+x,r.top+y] = (int) tileType.Environmental_Hazards;
				}
				else {
					map_data[r.left+x,r.top+y] = (int) tileType.Floor;
				}
			}
		}
		//use r property to create colliders
		
		makeDoors(r);
	}

	void makeSet3Body4Room(DRoom r){
		//make floors and wall
		for(int x=0; x < r.width; x++) {
			for(int y=0; y < r.height; y++){
				//Debug.Log(x + " " + y);
				//make wall
				//use r property to create colliders
				
				if(addWallColliders(r, x, y)){
					//makes wall
				}
				//Make Content of Room
				else if(((x == 5 || x == 7) && ((y >= 1 && y <=3) || y >= 9 && y<= 11)) ||
					((y == 5 || y == 7) && ((x >= 1 && x <=3) || x >= 9 && x <= 11))){
					map_data[r.left+x,r.top+y] = (int) tileType.Barrier;
					GameObject wall = createWall(r.left + x, r.top+y);
				}
				else if(x == 2 & y == 2){
					map_data[r.left+x,r.top+y] = (int) tileType.Objects;
				}
				else if(x == 1 && y == 9 || x == 10 && y == 10 || x == 11 && y == 3){
					map_data[r.left+x,r.top+y] = (int) tileType.Monster_Spawn;
				}
				else if (((y == 1 || y == 3 || y == 10) && x >= 1 && x <= 3) ||
					(y == 6 && x >= 5 && x <= 7) || (y == 2 && x >= 9 && x <= 11) ||
					(x == 1 && y == 2) || (x == 2 && y == 9) || (x == 6 && (y == 5 || y == 7)) ||
					(x == 9 && y == 10) || (y == 9 && (x == 9 || x == 10)) || 
					(y == 1 && (x == 10 || x == 11)) || (x == 10 && y == 3)){
					map_data[r.left+x,r.top+y] = (int) tileType.Environmental_Hazards;
				}
				else {
					map_data[r.left+x,r.top+y] = (int) tileType.Floor;
				}
			}
		}
		
		makeDoors(r);
	}

	GameObject createWall(int x, int y){
		GameObject wall = (GameObject) GameObject.Instantiate(wallGB, 
			new Vector3((x * tileSize) + (tileSize/2),0,(y* tileSize) + -(size_z * tileSize)+ (tileSize/2)),
			Quaternion.identity);
		wall.transform.parent = wallsMGR;
		return wall;
	}

	bool addWallColliders(DRoom r, int x, int y){
		if(x == 0 && y == 0){
			map_data[r.left+x,r.top+y] = (int) tileType.SW_Normal_Corner;
			GameObject wall = createWall(r.left + x, r.top+y);
			return true;
		}
		else if(x == 0 && y == r.height - 1){
			map_data[r.left+x,r.top+y] = (int) tileType.NW_Normal_Corner;
			GameObject wall = createWall(r.left + x, r.top+y);
			return true;
		}
		else if(x == r.width - 1 && y == 0){
			map_data[r.left+x,r.top+y] = (int) tileType.SE_Normal_Corner;
			GameObject wall = createWall(r.left + x, r.top+y);
			return true;
		}
		else if(x == r.width - 1 && y == r.height - 1){
			map_data[r.left+x,r.top+y] = (int) tileType.NE_Normal_Corner;
			GameObject wall = createWall(r.left + x, r.top+y);
			return true;
		}
		else if(x == 0){
			if(y == (r.height/2) && r.hasWestDoor){
				return true;
			}
			else{
				map_data[r.left+x,r.top+y] = (int) tileType.W_Normal_Wall;
				GameObject wall = createWall(r.left + x, r.top+y);
				return true;
			}
		}
		else if(x == r.width-1){
			if(y == (r.height/2) && r.hasEastDoor){
				return true;
			}
			else{
				map_data[r.left+x,r.top+y] = (int) tileType.E_Normal_Wall;
				GameObject wall = createWall(r.left + x, r.top+y);
				return true;
			}
		}
		else if(y==0){
			if(x == (r.width/2) && r.hasSouthDoor){
				return true;
			}
			else{
				map_data[r.left+x,r.top+y] = (int) tileType.S_Normal_Wall;
				GameObject wall = createWall(r.left + x, r.top+y);
				return true;
			}
		}
		else if(y == r.height-1) {
			if(x == (r.width/2) && r.hasNorthDoor){
				return true;
			}
			else{
				map_data[r.left+x,r.top+y] = (int) tileType.N_Normal_Wall;
				GameObject wall = createWall(r.left + x, r.top+y);
				return true;
			}
		}
		return false;
	}

	void makeDoors(DRoom r){
		//create doors where needed.
		if(r.hasNorthDoor){
			map_data[r.center_x,r.bottom] = (int) tileType.N_Normal_Door;
			//Debug.Log(new Vector3(r.center_x,0,r.bottom));
			//GameObject wall = (GameObject) GameObject.Instantiate(wallGB, 
			//new Vector3((r.center_x * tileSize),0,(r.bottom * tileSize) + -(size_z * tileSize)), Quaternion.identity);
		}
		if(r.hasWestDoor){
			map_data[r.left,r.center_y] = (int) tileType.W_Normal_Door;
			//GameObject wall = (GameObject) GameObject.Instantiate(wallGB, 
			//new Vector3((r.left * tileSize),0,(r.center_y * tileSize) + -(size_z * tileSize)), Quaternion.identity);
		}
		if(r.hasSouthDoor){
			map_data[r.center_x,r.top] = (int) tileType.S_Normal_Door;
			//GameObject wall = (GameObject) GameObject.Instantiate(wallGB, 
			//new Vector3((r.center_x * tileSize),0,(r.top * tileSize) + -(size_z * tileSize)), Quaternion.identity);
		}
		if(r.hasEastDoor){
			map_data[r.right,r.center_y] = (int) tileType.E_Normal_Door;
			//GameObject wall = (GameObject) GameObject.Instantiate(wallGB,
			// new Vector3((r.right * tileSize),0,(r.center_y * tileSize) + -(size_z * tileSize)), Quaternion.identity);
		}
	}

	//generates boolean matrix that will represent the map
	bool[,] generateMapMatrix(bool[,] map_layout, int numOfRooms){
		if(numOfRooms <= 0){
			return map_layout;
		}
		//still needs to create more rooms
		else{
			//room does not exist yet, create it.
			//Otherwise continue random traversal.
			
			if(map_layout[currentCol,currentRow] == false){
				numOfRooms--;
				map_layout[currentCol,currentRow] = true;
			}

			float direction = Random.value;
			//Debug.Log(direction);
			//current at leftmost column
			if(currentCol == 0){
				//we can only traverse down or right.
				if(currentRow == 0){
					//Checks if the room to the right exist
					if(direction >= 0.0f && direction <= 0.5f){
						currentCol++;
						return generateMapMatrix(map_layout, numOfRooms);
					}
					//Checks if the room below exist
					else{
						currentRow++;
						return generateMapMatrix(map_layout, numOfRooms);
					}
				}
				//we can only traverse up or right
				else if (currentRow == rowOfRooms){
					//Checks if the room to the right exist
					if(direction >= 0.0f && direction <= 0.5f){
						currentCol++;
						return generateMapMatrix(map_layout, numOfRooms);
					}
					//Checks if the room to the top exist
					else{
						currentRow--;
						return generateMapMatrix(map_layout, numOfRooms);
					}
				}
				//we can traverse up, down or right.
				else{
					//Checks if the room to the right exist
					if(direction >= 0.0f && direction <= 0.33f){
						currentCol++;
						return generateMapMatrix(map_layout, numOfRooms);
					}
					//Checks if the room below exist
					else if(direction > 0.33f && direction <= 0.67f){
						currentRow++;
						return generateMapMatrix(map_layout, numOfRooms);
					}
					//Checks if the room to the top exist
					else{
						currentRow--;
						return generateMapMatrix(map_layout, numOfRooms);
					}
				}
			}
			//current at rightmost column
			else if (currentCol == columnOfRooms){
				//we can only traverse down or left.
				if(currentRow == 0){
					//Checks if the room to the left exist
					if(direction >= 0.0f && direction <= 0.5f){
						currentCol--;
						return generateMapMatrix(map_layout, numOfRooms);
					}
					//Checks if the room below exist
					else{
						currentRow++;
						return generateMapMatrix(map_layout, numOfRooms);
					}
				}
				//we can only traverse up or left
				else if (currentRow == rowOfRooms){
					//Checks if the room to the left exist
					if(direction >= 0.0f && direction <= 0.5f){
						currentCol--;
						return generateMapMatrix(map_layout, numOfRooms);
					}
					//Checks if the room to the top exist
					else{
						currentRow--;
						return generateMapMatrix(map_layout, numOfRooms);
					}
				}
				//we can traverse up, down or left.
				else{
					//Checks if the room to the left exist
					if(direction >= 0.0f && direction <= 0.33f){
						currentCol--;
						return generateMapMatrix(map_layout, numOfRooms);
					}
					//Checks if the room below exist
					else if(direction > 0.33f && direction <= 0.67f){
						currentRow++;
						return generateMapMatrix(map_layout, numOfRooms);
					}
					//Checks if the room to the top exist
					else{
						currentRow--;
						return generateMapMatrix(map_layout, numOfRooms);
					}
				}
			}
			//current in the middle of matrix.
			else{
				//we can only traverse down, left or right.
				if(currentRow == 0){
					//Checks if the room to the left exist
					if(direction >= 0.0f && direction <= 0.33f){
						currentCol--;
						return generateMapMatrix(map_layout, numOfRooms);
					}
					//Checks if the room below exist
					else if(direction > 0.33f && direction <= 0.67f){
						currentRow++;
						return generateMapMatrix(map_layout, numOfRooms);
					}
					//Checks if the room to the right exist
					else{
						currentCol++;
						return generateMapMatrix(map_layout, numOfRooms);
					}
				}
				//we can only traverse up ,left or right.
				else if (currentRow == rowOfRooms){
					//Checks if the room to the left exist
					if(direction >= 0.0f && direction <= 0.33f){
						currentCol--;
						return generateMapMatrix(map_layout, numOfRooms);
					}
					//Checks if the room to the top exist
					else if(direction > 0.33f && direction <= 0.67f){
						currentRow--;
						return generateMapMatrix(map_layout, numOfRooms);
					}
					//Checks if the room to the right exist
					else{
						currentCol++;
						return generateMapMatrix(map_layout, numOfRooms);
					}
				}
				//we can traverse in all four directions
				else{
					//Checks if the room to the left exist
					if(direction >= 0.0f && direction <= 0.25f){
						currentCol--;
						return generateMapMatrix(map_layout, numOfRooms);
					}
					//Checks if the room to the top exist
					else if(direction > 0.25f && direction <= 0.5f){
						currentRow--;
						return generateMapMatrix(map_layout, numOfRooms);
					}
					//Checks if the room to the right exist
					else if(direction > 0.5f && direction <= 0.75f){
						currentCol++;
						return generateMapMatrix(map_layout, numOfRooms);
					}
					//Checks if the room below exist
					else{ 
						currentRow++;
						return generateMapMatrix(map_layout, numOfRooms);
					}
				}
			}
		}
	}
}
