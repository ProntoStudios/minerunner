using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObjectAttributes : MonoBehaviour {
	private int indexX = -1;
	private int indexY = -1;

	public void setIndex(int x, int y) {
		indexX = x;
		indexY = y;
	}

	public int getIndexX() {
		return indexX;
	}

	public int getIndexY() {
		return indexY;
	}
}
