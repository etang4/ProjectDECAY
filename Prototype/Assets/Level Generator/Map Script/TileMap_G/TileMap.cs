using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class TileMap : MonoBehaviour {
	
	public int size_x = 100;
	public int size_z = 50;
	public int columnOfRooms = 1;
	public int rowOfRooms = 1;
	public int numOfRooms = 1;
	public float tileSize = 1.0f;
	
	public Texture2D environmentTexture;
	public Texture2D grayTiles;
	public Texture2D grayDoors;
	public Texture2D normalTiles;
	public Texture2D normalDoors;
	public Texture2D bloodyTiles;
	public Texture2D bloodyDoors;

	public int tileResolution;
	
	// Use this for initialization
	void Start () {
		BuildMesh();
	}
	
	//Chop up all Textures. Param Texture will need to be every Texture2D.
	Color[][] ChopUpTiles() {
		//int numTilesPerRow = texture.width / tileResolution;
		//int numRows = texture.height / tileResolution;

		//Creating one long Color[][] with all the tiles in this order.
		int environmentNumTilesPerRow = environmentTexture.width / tileResolution;
		int environmentNumRows = environmentTexture.height / tileResolution;
		
		int grayNumTilesPerRow = grayTiles.width / tileResolution;
		int grayNumRows = grayTiles.height / tileResolution;

		int grayDoorNumTilesPerRow = grayDoors.width / tileResolution;
		int grayDoorNumRows = grayDoors.height / tileResolution;

		int normalNumTilesPerRow = normalTiles.width / tileResolution;
		int normalNumRows = normalTiles.height / tileResolution;

		int normalDoorNumTilesPerRow = normalDoors.width / tileResolution;
		int normalDoorNumRows = normalDoors.height / tileResolution;

		int bloodyNumTilesPerRow = bloodyTiles.width / tileResolution;
		int bloodyNumRows = bloodyTiles.height / tileResolution;

		int bloodyDoorNumTilesPerRow = bloodyDoors.width / tileResolution;
		int bloodyDoorNumRows = bloodyDoors.height / tileResolution;

		int numOfTotalTiles = (environmentNumTilesPerRow*environmentNumRows) 
			+ (grayNumTilesPerRow*grayNumRows) + (grayDoorNumTilesPerRow*grayDoorNumRows)
			+ (normalNumTilesPerRow*normalNumRows) + (normalDoorNumTilesPerRow*normalDoorNumRows)
			+ (bloodyNumTilesPerRow*bloodyNumRows) + (bloodyDoorNumTilesPerRow*bloodyDoorNumRows);
		
		//total number of tiles in first array.
		Color[][] tiles = new Color[numOfTotalTiles][];
		
		int totalTileCounter = 0;

		//adding Environment Tiles
		for(int y=0; y<environmentNumRows; y++) {
			for(int x=0; x<environmentNumTilesPerRow; x++) {
				tiles[totalTileCounter] = environmentTexture.GetPixels( 
					x*tileResolution , y*tileResolution, tileResolution, tileResolution );
				totalTileCounter++;
			}
		}
		//Adding Gray Walls & Corner Tiles
		for(int y=0; y<grayNumRows; y++) {
			for(int x=0; x<grayNumTilesPerRow; x++) {
				tiles[totalTileCounter] = grayTiles.GetPixels( 
					x*tileResolution , y*tileResolution, tileResolution, tileResolution );
				totalTileCounter++;
			}
		}
		//Adding Gray Door Tiles
		for(int y=0; y<grayDoorNumRows; y++) {
			for(int x=0; x<grayDoorNumTilesPerRow; x++) {
				tiles[totalTileCounter] = grayDoors.GetPixels( 
					x*tileResolution , y*tileResolution, tileResolution, tileResolution );
				totalTileCounter++;
			}
		}
		//Adding Normal Walls & Corner Tiles
		for(int y=0; y<normalNumRows; y++) {
			for(int x=0; x<normalNumTilesPerRow; x++) {
				tiles[totalTileCounter] = normalTiles.GetPixels( 
					x*tileResolution , y*tileResolution, tileResolution, tileResolution );
				totalTileCounter++;
			}
		}
		//Adding Normal Door Tiles
		for(int y=0; y<normalDoorNumRows; y++) {
			for(int x=0; x<normalDoorNumTilesPerRow; x++) {
				tiles[totalTileCounter] = normalDoors.GetPixels( 
					x*tileResolution , y*tileResolution, tileResolution, tileResolution );
				totalTileCounter++;
			}
		}
		//Adding Bloody Walls & Corner Tiles
		for(int y=0; y<bloodyNumRows; y++) {
			for(int x=0; x<bloodyNumTilesPerRow; x++) {
				tiles[totalTileCounter] = bloodyTiles.GetPixels( 
					x*tileResolution , y*tileResolution, tileResolution, tileResolution );
				totalTileCounter++;
			}
		}
		//Adding Bloody Door Tiles
		for(int y=0; y<bloodyDoorNumRows; y++) {
			for(int x=0; x<bloodyDoorNumTilesPerRow ; x++) {
				tiles[totalTileCounter] = bloodyDoors.GetPixels( 
					x*tileResolution , y*tileResolution, tileResolution, tileResolution );
				totalTileCounter++;
			}
		}

		//Color[][] is filled.
		
		return tiles;
	}
	
	void BuildTexture() {
		CustomDTileMap map = new CustomDTileMap(size_x, size_z, columnOfRooms , rowOfRooms , numOfRooms);
		
		int texWidth = size_x * tileResolution;
		int texHeight = size_z * tileResolution;
		Texture2D texture = new Texture2D(texWidth, texHeight);
		
		Color[][] tiles = ChopUpTiles();
		
		for(int y=0; y < size_z; y++) {
			for(int x=0; x < size_x; x++) {
				//tile is Color[][]
				Color[] p = tiles[ map.GetTileAt(x,y) ];
				texture.SetPixels(x*tileResolution, y*tileResolution, tileResolution, tileResolution, p);
			}
		}
		
		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.Apply();
		
		MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
		mesh_renderer.sharedMaterials[0].mainTexture = texture;
		
		Debug.Log ("Done Texture!");
	}
	
	public void BuildMesh() {
		int numTiles = size_x * size_z;
		int numTris = numTiles * 2;
		
		int vsize_x = size_x + 1;
		int vsize_z = size_z + 1;
		int numVerts = vsize_x * vsize_z;
		
		// Generate the mesh data
		Vector3[] vertices = new Vector3[ numVerts ];
		Vector3[] normals = new Vector3[numVerts];
		Vector2[] uv = new Vector2[numVerts];
		
		int[] triangles = new int[ numTris * 3 ];

		int x, z;
		for(z=0; z < vsize_z; z++) {
			for(x=0; x < vsize_x; x++) {
				vertices[ z * vsize_x + x ] = new Vector3( x*tileSize, 0, -z*tileSize );
				normals[ z * vsize_x + x ] = Vector3.up;
				uv[ z * vsize_x + x ] = new Vector2( (float)x / size_x, 1f - (float)z / size_z );
			}
		}
		Debug.Log ("Done Verts!");
		
		for(z=0; z < size_z; z++) {
			for(x=0; x < size_x; x++) {
				int squareIndex = z * size_x + x;
				int triOffset = squareIndex * 6;
				triangles[triOffset + 0] = z * vsize_x + x + 		   0;
				triangles[triOffset + 2] = z * vsize_x + x + vsize_x + 0;
				triangles[triOffset + 1] = z * vsize_x + x + vsize_x + 1;
				
				triangles[triOffset + 3] = z * vsize_x + x + 		   0;
				triangles[triOffset + 5] = z * vsize_x + x + vsize_x + 1;
				triangles[triOffset + 4] = z * vsize_x + x + 		   1;
			}
		}
		
		Debug.Log ("Done Triangles!");
		
		// Create a new Mesh and populate with the data
		Mesh mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.normals = normals;
		mesh.uv = uv;
		
		// Assign our mesh to our filter/renderer/collider
		MeshFilter mesh_filter = GetComponent<MeshFilter>();
		MeshCollider mesh_collider = GetComponent<MeshCollider>();
		
		mesh_filter.mesh = mesh;
		mesh_collider.sharedMesh = mesh;
		Debug.Log ("Done Mesh!");
		
		BuildTexture();
	}
	
	
}
