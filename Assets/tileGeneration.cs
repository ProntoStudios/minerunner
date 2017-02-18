using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tileGeneration : MonoBehaviour {

	float speed;

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

	void generateRow(int bottom, float y) {
		for (int i = 0; i < 5; i++) {
			tiles [bottom, i].setY (y + sideLength);
		}

	}

	void Start () {
		Debug.Log (Screen.width);
		Debug.Log (Screen.height);

		screenBase = Camera.main.ScreenToWorldPoint (new Vector3 (0f, 0f, 0f));
		screenSize = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width, Screen.height, 0f)) * 2.0f;

		Debug.Log (screenSize);
			//- Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width, Screen.height, 10.0f));
		sideLength = screenSize.x / 5f;

		bottomIndex = 0;
		verticalExtent = (int)(screenSize.y / sideLength) - startHeight + 1;

		//bottomIndex -= startHeight;

		tiles = new Tile[verticalExtent, 5];
		float lastTop = screenBase.y - sideLength / 2;

		speed = -0.55f;
		//float h = screenBase.y + screenSize.y - sideLength/2;

		for (int y = 0; y < verticalExtent; y++) {
			for (int x = 0; x < 5; x++) {
				Tile sq = new Tile(screenBase.x + sideLength/2 +x*sideLength, 0, 1, true, true);
				sq.setNumber (y);
				sq.setDownwardSpeed(speed);
				tiles [y, x] = sq;
			}
			generateRow (y, lastTop);
			lastTop += sideLength;
		}
	}


	// Update is called once per frame
	void Update () {
		if (tiles [bottomIndex,0].topGreater (screenBase.y + screenSize.y + sideLength/2)) {
			int topIndex = bottomIndex == 0 ? verticalExtent - 1 : bottomIndex - 1;
			Debug.Log (topIndex);
			generateRow (bottomIndex, tiles[topIndex, 0].getY());
			bottomIndex = bottomIndex == verticalExtent-1 ? 0 : bottomIndex + 1;
		}
	}
}
