using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tile {

	private GameObject tileObject;
	private Rigidbody2D rb;

	//Object Fields
	private bool bomb;
	private int flag;
	private bool hidden;
	private int number;
	private bool odd;
	// Intact sides, true => intact
	private bool[] sides = {true,true,true,true};

	//[Static] Constant Fields
	private const int LAYER = 10;
	private const int FLAG_NONE = 0;
	private const int FLAG_RED = 1;
	private const int FLAG_GREEN = 2;

	//Static Fields
	private static Color hiddenColor = new Color( 0.4f, 0.4f, 0.4f, 1f );
	private static Color hiddenColorOdd = new Color( 0.5f, 0.5f, 0.5f, 1f );
	//225 225 225
	private static Color backColor = new Color(0.9215686274509803f, 0.9215686274509803f, 0.9215686274509803f, 1f );
	private static Color backColorOdd = new Color(1f, 1f, 1f, 1f );

	private static Sprite bombSprite = Resources.Load<Sprite>("Tile/bomb");
	private static Sprite flagGreenSprite = Resources.Load<Sprite>("Tile/flagGreen");
	private static Sprite flagRedSprite = Resources.Load<Sprite>("Tile/flagRed");
	private static Sprite noneSprite = Resources.Load<Sprite>("Tile/none");
	private static Sprite[] numberSprite = {
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
	private static Sprite blankImg = Resources.Load<Sprite>("Tile/Paths/blank");
	private static Sprite crossPath = Resources.Load<Sprite>("Tile/Paths/cross");
	private static Sprite endPath = Resources.Load<Sprite>("Tile/Paths/end");
	private static Sprite lPath = Resources.Load<Sprite>("Tile/Paths/l");
	private static Sprite tPath = Resources.Load<Sprite>("Tile/Paths/t");
	private static Sprite straightPath = Resources.Load<Sprite>("Tile/Paths/straight");


	public Tile(float x, float y, int indexX, int indexY, bool isOdd, bool bmb = false) {
		tileObject = (GameObject) GameObject.Instantiate(Resources.Load("Tile/Tile"));
		tileObject.name = "(" + indexX.ToString () + "," + indexY.ToString () + ")";

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
		flag = FLAG_NONE;
		hidden = false;
		number = 0;
		odd = isOdd;

		if (odd) {
			tileObject.transform.FindChild ("Background").gameObject.GetComponent<SpriteRenderer> ().color = hiddenColor;
		} else {
			tileObject.transform.FindChild ("Background").gameObject.GetComponent<SpriteRenderer> ().color = hiddenColorOdd;
		}

	}

	public bool isBomb() {
		return bomb;
	}
	public int intIsBomb() {
		return (bomb)?1:0;
	}

	public int getFlag() {
		return flag;
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
		flag = FLAG_NONE;
		tileObject.transform.FindChild ("Object").gameObject.GetComponent<SpriteRenderer> ().sprite = noneSprite;
	}
	public void nextFlag() {
		flag = (flag+1)%3;
		if (flag == FLAG_GREEN) {
			tileObject.transform.FindChild ("Object").gameObject.GetComponent<SpriteRenderer> ().sprite = flagGreenSprite;
		} else if (flag == FLAG_RED) {
			tileObject.transform.FindChild ("Object").gameObject.GetComponent<SpriteRenderer> ().sprite = flagRedSprite;
		} else {
			tileObject.transform.FindChild ("Object").gameObject.GetComponent<SpriteRenderer> ().sprite = noneSprite;
		}
	}

	public void hide() {
		hidden = true;
		flag = 0;
		tileObject.transform.FindChild ("Object").gameObject.GetComponent<SpriteRenderer> ().sprite = noneSprite;
		if (odd) {
			tileObject.transform.FindChild ("Background").gameObject.GetComponent<SpriteRenderer> ().color = hiddenColorOdd;
		} else {
			tileObject.transform.FindChild ("Background").gameObject.GetComponent<SpriteRenderer> ().color = hiddenColor;
		}
	}

	public void reset() {
		hidden = true;
		flag = 0;
		tileObject.transform.FindChild ("Object").gameObject.GetComponent<SpriteRenderer> ().sprite = noneSprite;
		sides[0] = true; sides [1] = true; sides [2] = true; sides [3] = true;
		updatePath();
	}
		
	public void show() {
		hidden = false;
		flag = 0;
		if (bomb) {
			tileObject.transform.FindChild ("Object").gameObject.GetComponent<SpriteRenderer> ().sprite = bombSprite;
		} else {
			tileObject.transform.FindChild ("Object").gameObject.GetComponent<SpriteRenderer> ().sprite = noneSprite;
		}
		tileObject.transform.FindChild ("Background").gameObject.GetComponent<SpriteRenderer> ().color = backColor;
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

	public float getX() {
		return tileObject.transform.localPosition.x;
	}

	public void updatePath() {
		SpriteRenderer path = tileObject.transform.FindChild ("Object").gameObject.GetComponent<SpriteRenderer> ();
		Transform numTransform = tileObject.transform.FindChild ("Object").gameObject.transform;

		if (sides [0] && sides[1] && sides[2] && sides[3]) {
			path.sprite = blankImg;
		}
		if (sides [0] && sides[1] && sides[2] && !sides[3]) {
			path.sprite = endPath;
			numTransform.rotation = Quaternion.Euler(0,0,-90);
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
			Debug.Log ("0111");
			path.sprite = tPath;
			numTransform.rotation = Quaternion.Euler(0,0,0);
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
			Debug.Log ("1011");
			path.sprite = tPath;
			numTransform.rotation = Quaternion.Euler(0,0,-90);
		}
		if (!sides [0] && !sides[1] && sides[2] && sides[3]) {
			path.sprite = lPath;
			numTransform.rotation = Quaternion.Euler(0,0,0);
		}
		if (!sides [0] && !sides[1] && sides[2] && !sides[3]) {
			Debug.Log ("1101");
			path.sprite = tPath;
			numTransform.rotation = Quaternion.Euler(0,0,180);
		}
		if (!sides [0] && !sides[1] && !sides[2] && sides[3]) {
			Debug.Log ("1110");
			path.sprite = tPath;
			numTransform.rotation = Quaternion.Euler(0,0,90);
		}
		if (!sides [0] && !sides[1] && !sides[2] && !sides[3]) {
			path.sprite = crossPath;
			numTransform.rotation = Quaternion.Euler(0,0,0);
		}
		if (hidden == false) {
			path.sprite = blankImg;
			Debug.Log ("setting blank");
		}
		//Debug.Log ("setting blank");
	}

	public void tunnel(int dir, int into) {
		int removeSide = (dir + 2 * into) % 4;
		Debug.Log (removeSide);
		sides [removeSide] = false;
	}

	public void setDownwardSpeed(float speed) {
		rb.velocity = new Vector3 (0,speed,0);
	}
		

}
