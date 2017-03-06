using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tileGeneration : MonoBehaviour {

	public static tileGeneration instance;

	float speed = -0.15f;

	//public GameObject testingSquare;

	Tile[,] tiles;
	float sideLength; //= Screen.width / 5.0f;
	int startHeight = -2;

	int bottomIndex;
	int verticalExtent;

	// previous location of generation for path
	int lastGen = 2;

	Vector3 screenSize;	
	Vector3 screenBase;

	struct Int2 {
		public int x;
		public int y;
	}

	Int2 playerLoc;
	public bool movingPlayer = false;

	private GameObject player;
	playerController playerScript;

	void generateRow(int bottom, float y, bool first = false) {
		for (int i = 0; i < 5; i++) {
			tiles [bottom, i].setY (y + sideLength);
			tiles [bottom, i].clearBomb ();
		}
		//    |---|
		//    | 0 |
		//|--- ___---|
		//|-1 |str|1 |
		//
		int direction = -1;
		bool[] clear = new bool[5];

		while (direction != 0) {
			clear [lastGen] = true;
			direction = Random.Range (-1, 3);
			if (lastGen == 0 && direction == -1) {
				direction = 0;
			}
			if (lastGen == 4 && direction == 1) {
				direction = 0;
			}
			if (direction == 2) {
				direction = 0;
			}
			lastGen += direction;
		}

		for (int x = 0; x < 5; x++) {
			if (!clear[x] && Random.Range (0, 2) == 1) {
				tiles [bottom, x].plantBomb ();
			}
			tiles [bottom, x].hide ();
		}

		//updating current row numbers
		//int upper = (bottom == verticalExtent - 1) ? 0 : (bottom + 1);
		int under = (bottom == 0) ? (verticalExtent - 1) : (bottom - 1);

		for (int x = 0; x < 5; x++) {
			if (x == 0) {
				if (!first) {
					tiles [bottom, x].setNumber (
						tiles [under, 0].intIsBomb () + tiles [under, 1].intIsBomb ()
						+ tiles[bottom, 0].intIsBomb() + tiles[bottom, 1].intIsBomb());
					tiles [under, x].setNumber (
						tiles [under, x].getNumber() + tiles [bottom, 0].intIsBomb () + tiles [bottom, 1].intIsBomb ());
				} else {
					tiles [bottom, x].setNumber (0);
				}
			} else if (x == 4) {
				if (!first) {
					tiles [bottom, x].setNumber (
						tiles [under, 3].intIsBomb () + tiles [under, 4].intIsBomb ()
						+ tiles[bottom, 3].intIsBomb() + tiles[bottom, 4].intIsBomb());
					tiles [under, x].setNumber (
						tiles [under, x].getNumber() + tiles [bottom, 3].intIsBomb () + tiles [bottom, 4].intIsBomb ());
				} else {
					tiles [bottom, x].setNumber (0);
				}
			} else {
				if (!first) {
					tiles [bottom, x].setNumber (
						tiles [under, x - 1].intIsBomb () + tiles [under, x].intIsBomb () + tiles [under, x + 1].intIsBomb ()
						+ tiles[bottom, x-1].intIsBomb() + tiles[bottom, x].intIsBomb() + tiles[bottom, x+1].intIsBomb());
					tiles [under, x].setNumber (
						tiles [under, x].getNumber() + tiles [bottom, x - 1].intIsBomb () + tiles [bottom, x].intIsBomb () + tiles [bottom, x + 1].intIsBomb ());
				} else {
					tiles [bottom, x].setNumber (0);
				}
			}
		}

	}
		

	void Start () {
		instance = this;
		/*if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}*/

		Debug.Log (Screen.width);
		Debug.Log (Screen.height);

		screenBase = Camera.main.ScreenToWorldPoint (new Vector3 (0f, 0f, 0f));
		screenSize = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width, Screen.height, 0f)) * 2.0f;

		player = (GameObject) GameObject.Instantiate(Resources.Load("Player/Player"));
		playerScript = player.transform.GetComponent<playerController> ();

		Debug.Log (screenSize);
		Debug.Log (screenBase);
			//- Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width, Screen.height, 10.0f));
		sideLength = screenSize.x / 5f;

		//bottomIndex -= startHeight;
		bottomIndex = 0;
		verticalExtent = (int)(screenSize.y / sideLength) - startHeight + 1;//where start height = -ve number like -2
		verticalExtent += (verticalExtent % 2); //make the number of vertical tiles an even number

		tiles = new Tile[verticalExtent, 5];
		float lastTop = screenBase.y - sideLength / 2;

		//float h = screenBase.y + screenSize.y - sideLength/2;

		bool firstRow = true;
		for (int y = 0; y < verticalExtent; y++) {
			for (int x = 0; x < 5; x++) {
				
				Tile sq = new Tile(screenBase.x + sideLength/2 +x*sideLength, 0f, y, x, !((x+y*5)%2 == 0));
				//y set when row generated

				//sq.setNumber (y);
				sq.setDownwardSpeed(speed);
				tiles [y, x] = sq;
			}
			if (firstRow) {
				generateRow (y, lastTop, true);
				firstRow = false;
			} else {
				generateRow (y, lastTop);
			}
			lastTop += sideLength;
		}

		float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
		float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;
		worldScreenWidth /= 5.0f;
		Vector3 world_scale = new Vector3 (worldScreenWidth, worldScreenWidth, 1.0f);

		playerScript.sideLength = sideLength;
		player.transform.localScale = world_scale;
		playerScript.setDownwardSpeed (speed);
		playerScript.setPosition (screenBase.x + sideLength / 2 + 2 * sideLength, sideLength * 4 + screenBase.y - sideLength / 2);

		playerLoc.x = 2;
		playerLoc.y = 3;

	}
		
	// Update is called once per frame
	void Update () {
		if (tiles [bottomIndex,0].topGreater (screenBase.y + screenSize.y + sideLength/2)) {
			//if bottom row has exceeded screen
			int topIndex = (bottomIndex == 0) ? (verticalExtent - 1) : (bottomIndex - 1);
			//Debug.Log (topIndex);
			generateRow (bottomIndex, tiles[topIndex, 0].getY());
			bottomIndex = (bottomIndex == verticalExtent-1) ? 0 : (bottomIndex + 1);
		}
	}


	public void tileTapped(int indexX, int indexY) {
		Tile hit = tiles[indexX, indexY];
		if (hit.isHidden()/* && not path??*/) {
			hit.nextFlag ();
		}
	}

	public void movePlayerLeft() {
		if (!movingPlayer && playerLoc.x > 0) {
			movingPlayer = true;
			tiles [playerLoc.y, playerLoc.x].tunnel (3, 0);
			playerScript.move (-1, 0, tiles[playerLoc.y, --playerLoc.x]);
			tiles [playerLoc.y, playerLoc.x].tunnel (3, 1);
			Debug.Log (playerLoc.x.ToString () + ", " + playerLoc.y.ToString ());
		}
	}
	public void movePlayerRight() {
		if (!movingPlayer && playerLoc.x < 4) {
			movingPlayer = true;
			tiles [playerLoc.y, playerLoc.x].tunnel (1, 0);
			playerScript.move (1, 0, tiles[playerLoc.y, ++playerLoc.x]);
			tiles [playerLoc.y, playerLoc.x].tunnel (1, 1);
			Debug.Log (playerLoc.x.ToString () + ", " + playerLoc.y.ToString ());
		}
	}
	public void movePlayerUp() {
		if (!movingPlayer) {
			movingPlayer = true;
			tiles [playerLoc.y, playerLoc.x].tunnel (0, 0);
			if (playerLoc.y < verticalExtent - 1) {
				playerScript.move (0, 1, tiles [++playerLoc.y, playerLoc.x]);
			} else {
				playerScript.move (0, 1, tiles [0, playerLoc.x]);
				playerLoc.y = 0;
			}
			tiles [playerLoc.y, playerLoc.x].tunnel (0, 1);
			Debug.Log (playerLoc.x.ToString () + ", " + playerLoc.y.ToString ());
		}
	}
	public void movePlayerDown() {
		if (!movingPlayer) {
			movingPlayer = true;
			tiles [playerLoc.y, playerLoc.x].tunnel (2, 0);
			if (playerLoc.y > 0) {
				playerScript.move (0, -1, tiles[--playerLoc.y, playerLoc.x]);
			} else {
				playerScript.move (0, -1, tiles [verticalExtent-1, playerLoc.x]);
				playerLoc.y = verticalExtent - 1;
			}
			tiles [playerLoc.y, playerLoc.x].tunnel (2, 1);
			Debug.Log (playerLoc.x.ToString () + ", " + playerLoc.y.ToString ());
		}
	}

}
