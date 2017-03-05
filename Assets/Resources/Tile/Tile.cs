﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tile {

	private GameObject tileObject;
	private Rigidbody2D rb;

	//Object Properties
	private bool bomb;
	private bool flag;
	private bool hidden;
	private int number;
	private bool odd;
	// Intact sides, true => intact
	private bool[] sides = {true,true,true,true};

	//Static properties
	static int LAYER = 10;

	static Color hiddenColor = new Color( 0.4f, 0.4f, 0.4f, 1f );
	static Color hiddenColorOdd = new Color( 0.5f, 0.5f, 0.5f, 1f );
	static Color backColor = new Color(0.9f, 0.9f, 0.9f, 1f );
	static Color backColorOdd = new Color(1f, 1f, 1f, 1f );

	static Sprite bombSprite = Resources.Load<Sprite>("Tile/bomb");
	static Sprite flagSprite = Resources.Load<Sprite>("Tile/bomb");//CHANGE TO FLAG
	static Sprite noneSprite = Resources.Load<Sprite>("Tile/none");
	static Sprite[] numberSprite = {
		Resources.Load<Sprite>("Tile/Numbers/0"),
		Resources.Load<Sprite>("Tile/Numbers/1"),
		Resources.Load<Sprite>("Tile/Numbers/2"),
		Resources.Load<Sprite>("Tile/Numbers/3"),
		Resources.Load<Sprite>("Tile/Numbers/4"),
		Resources.Load<Sprite>("Tile/Numbers/5"),
		Resources.Load<Sprite>("Tile/Numbers/6"),
		Resources.Load<Sprite>("Tile/Numbers/7"),
		Resources.Load<Sprite>("Tile/Numbers/8"),
		Resources.Load<Sprite>("Tile/Numbers/9")
	};
	static Sprite blankImg = Resources.Load<Sprite>("Tile/Paths/blank");
	static Sprite crossPath = Resources.Load<Sprite>("Tile/Paths/cross");
	static Sprite endPath = Resources.Load<Sprite>("Tile/Paths/end");
	static Sprite lPath = Resources.Load<Sprite>("Tile/Paths/l");
	static Sprite tPath = Resources.Load<Sprite>("Tile/Paths/t");
	static Sprite straightPath = Resources.Load<Sprite>("Tile/Paths/straight");


	public Tile(float x, float y, int indexX, int indexY, bool isOdd, bool hddn = false, bool bmb = false) {
		tileObject = (GameObject) GameObject.Instantiate(Resources.Load("Tile/Tile"));

		tileObject.transform.localPosition = new Vector3 (x, y, (float)LAYER);
		tileObject.GetComponent<TileObjectAttributes>().setIndex(indexX, indexY);

		tileObject.GetComponentInChildren<Transform> ().Find ("Background").transform.localPosition = new Vector3(0, 0, LAYER-1);
		tileObject.GetComponentInChildren<Transform> ().Find ("Object").transform.localPosition = new Vector3(0, 0, LAYER-2);
		tileObject.GetComponentInChildren<Transform> ().Find ("Number").transform.localPosition = new Vector3(0, 0, LAYER-3);

		tileObject.transform.FindChild ("Object").gameObject.GetComponent<SpriteRenderer> ().sprite = blankImg; 
		tileObject.transform.FindChild ("Object").gameObject.transform.localScale = new Vector3 (1.565f, 1.565f, 1);

		float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
		float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;
		worldScreenWidth /= 5.0f;
		Vector3 world_scale = new Vector3 (worldScreenWidth, worldScreenWidth ,1.0f);
		tileObject.transform.localScale = world_scale;


		rb = tileObject.GetComponent<Rigidbody2D>();

		bomb = bmb;
		flag = false;
		hidden = hddn;
		number = 0;
		odd = isOdd;

		if (odd) {
			tileObject.transform.FindChild ("Background").gameObject.GetComponent<SpriteRenderer> ().color = backColorOdd;
		} else {
			tileObject.transform.FindChild ("Background").gameObject.GetComponent<SpriteRenderer> ().color = backColor;
		}

	}

	public bool isBomb() {
		return bomb;
	}

	public bool isFlag() {
		return flag;
	}

	public int intIsBomb() {
		return bomb ? 1 : 0;
	}

	public bool isHidden() {
		return hidden;
	}

	public void clearBomb() {
		bomb = false;
		tileObject.transform.FindChild ("Object").gameObject.GetComponent<SpriteRenderer> ().sprite = noneSprite;
	}
	public void plantBomb() {
		bomb = true;
		tileObject.transform.FindChild ("Object").gameObject.GetComponent<SpriteRenderer> ().sprite = bombSprite;
	}

	public void clearFlag() {
		flag = false;
		tileObject.transform.FindChild ("Object").gameObject.GetComponent<SpriteRenderer> ().sprite = noneSprite;
	}
	public void plantFlag() {
		flag = true;
		tileObject.transform.FindChild ("Object").gameObject.GetComponent<SpriteRenderer> ().sprite = flagSprite;
	}

	public void hide() {
		hidden = true;
		flag = false;
		tileObject.transform.FindChild ("Object").gameObject.GetComponent<SpriteRenderer> ().sprite = noneSprite;
		if (odd) {
			tileObject.transform.FindChild ("Background").gameObject.GetComponent<SpriteRenderer> ().color = hiddenColorOdd;
		} else {
			tileObject.transform.FindChild ("Background").gameObject.GetComponent<SpriteRenderer> ().color = hiddenColor;
		}
	}

	public void setNumber(int num) {
		if (num > 9 || num < 0)
			num = 0;

		number = num;
		tileObject.transform.FindChild("Number").gameObject.GetComponent<SpriteRenderer>().sprite = numberSprite[num];
	}

	public int getNumber() {
		return number;
	}

	public bool topGreater(float comparee) {
		// Is the top of the object less than the comparee
		return -tileObject.transform.position.y > comparee;
	}

	public void setY(float newY) {
		Vector3 newPos = new Vector3 (tileObject.transform.localPosition.x, newY, tileObject.transform.localPosition.z);
		tileObject.transform.transform.localPosition = newPos;
	}

	public float getY() {
		return tileObject.transform.localPosition.y;
	}

	private void updatePath() {
		SpriteRenderer path = tileObject.transform.FindChild ("Object").gameObject.GetComponent<SpriteRenderer> ();
		Transform numTransform = tileObject.transform.FindChild ("Object").gameObject.transform;

		if (sides [0] && sides[1] && sides[2] && sides[3]) {
			path.sprite = blankImg;
		}
		if (sides [0] && sides[1] && sides[2] && !sides[3]) {
			path.sprite = endPath;
			numTransform.rotation = Quaternion.Euler(0,0,90);
		}
		if (sides [0] && sides[1] && !sides[2] && sides[3]) {
			path.sprite = endPath;
			numTransform.rotation = Quaternion.Euler(0,0,0);
		}
		if (sides [0] && sides[1] && !sides[2] && !sides[3]) {
			path.sprite = lPath;
			numTransform.rotation = Quaternion.Euler(0,0,180);
		}
		if (sides [0] && !sides[1] && sides[2] && sides[3]) {
			path.sprite = endPath;
			numTransform.rotation = Quaternion.Euler(0,0,90);
		}
		if (sides [0] && !sides[1] && sides[2] && !sides[3]) {
			path.sprite = straightPath;
			numTransform.rotation = Quaternion.Euler(0,0,90);
		}
		if (sides [0] && !sides[1] && !sides[2] && sides[3]) {
			path.sprite = lPath;
			numTransform.rotation = Quaternion.Euler(0,0,-90);
		}
		if (sides [0] && !sides[1] && !sides[2] && !sides[3]) {
			path.sprite = tPath;
			numTransform.rotation = Quaternion.Euler(0,0,90);
		}
		if (!sides [0] && sides[1] && sides[2] && sides[3]) {
			path.sprite = endPath;
			numTransform.rotation = Quaternion.Euler(0,0,180);
		}
		if (!sides [0] && sides[1] && sides[2] && !sides[3]) {
			path.sprite = lPath;
			numTransform.rotation = Quaternion.Euler(0,0,90);
		}
		if (!sides [0] && sides[1] && !sides[2] && sides[3]) {
			path.sprite = straightPath;
			numTransform.rotation = Quaternion.Euler(0,0,0);
		}
		if (!sides [0] && sides[1] && !sides[2] && !sides[3]) {
			path.sprite = tPath;
			numTransform.rotation = Quaternion.Euler(0,0,-90);
		}
		if (!sides [0] && !sides[1] && sides[2] && sides[3]) {
			path.sprite = lPath;
			numTransform.rotation = Quaternion.Euler(0,0,0);
		}
		if (!sides [0] && !sides[1] && sides[2] && !sides[3]) {
			path.sprite = tPath;
			numTransform.rotation = Quaternion.Euler(0,0,180);
		}
		if (!sides [0] && !sides[1] && !sides[2] && sides[3]) {
			path.sprite = tPath;
			numTransform.rotation = Quaternion.Euler(0,0,0);
		}
		if (!sides [0] && !sides[1] && !sides[2] && !sides[3]) {
			path.sprite = crossPath;
			numTransform.rotation = Quaternion.Euler(0,0,0);
		}
	}

	public void tunnel(int dir, int into) {
		int removeSide = (dir + 2 * into) % 4;
		sides [removeSide] = false;
		updatePath ();
	}

	public void setDownwardSpeed(float speed) {
		rb.velocity = new Vector3 (0,speed,0);
	}
		

}
