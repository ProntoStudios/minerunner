using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tileGeneration : MonoBehaviour {

	float speed = -0.55f;

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
			if (x % 2 == 0) {
				tiles [bottom, x].hide ();
			}
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
		Debug.Log (Screen.width);
		Debug.Log (Screen.height);

		screenBase = Camera.main.ScreenToWorldPoint (new Vector3 (0f, 0f, 0f));
		screenSize = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width, Screen.height, 0f)) * 2.0f;

		Debug.Log (screenSize);
		Debug.Log (screenBase);
			//- Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width, Screen.height, 10.0f));
		sideLength = screenSize.x / 5f;

		//bottomIndex -= startHeight;
		bottomIndex = 0;
		verticalExtent = (int)(screenSize.y / sideLength) - startHeight + 1;//where start height = -ve number like -2

		tiles = new Tile[verticalExtent, 5];
		float lastTop = screenBase.y - sideLength / 2;

		//float h = screenBase.y + screenSize.y - sideLength/2;

		bool firstRow = true;
		for (int y = 0; y < verticalExtent; y++) {
			for (int x = 0; x < 5; x++) {
				Tile sq = new Tile(screenBase.x + sideLength/2 +x*sideLength, 0, !((x+y*5)%2 == 0));
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
	}


	// Update is called once per frame
	void Update () {
		if (tiles [bottomIndex,0].topGreater (screenBase.y + screenSize.y + sideLength/2)) {
			//if bottom row has exceeded screen
			int topIndex = (bottomIndex == 0) ? (verticalExtent - 1) : (bottomIndex - 1);
			Debug.Log (topIndex);
			generateRow (bottomIndex, tiles[topIndex, 0].getY());
			bottomIndex = (bottomIndex == verticalExtent-1) ? 0 : (bottomIndex + 1);
		}
	}
}
