using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

public class Tile : MonoBehaviour {

	// Object 
	GameObject tileObject;
	Rigidbody2D rb;

	// Properties
	public const int LAYER = 10;

	bool bomb;
	bool shown;

	Sprite[] number = {
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

	Sprite blankImg = Resources.Load<Sprite>("Tile/Paths/blank");
	Sprite crossPath = Resources.Load<Sprite>("Tile/Paths/cross");
	Sprite endPath = Resources.Load<Sprite>("Tile/Paths/end");
	Sprite lPath = Resources.Load<Sprite>("Tile/Paths/l");
	Sprite tPath = Resources.Load<Sprite>("Tile/Paths/t");
	Sprite straightPath = Resources.Load<Sprite>("Tile/Paths/straight");

	// Intact sides, true => intact
	bool[] sides = {true,true,true,true};

	public Tile(float x, float y, float length, bool bmb, bool shwn) {
		tileObject = (GameObject) Instantiate(Resources.Load("Tile/Tile"));

		tileObject.transform.localPosition = new Vector3 (x, y, (float)LAYER);

		tileObject.GetComponentInChildren<Transform> ().Find ("Background").transform.localPosition = new Vector3(0, 0, LAYER-1);
		tileObject.GetComponentInChildren<Transform> ().Find ("Object").transform.localPosition = new Vector3(0, 0, LAYER-2);
		tileObject.GetComponentInChildren<Transform> ().Find ("Number").transform.localPosition = new Vector3(0, 0, LAYER-3);

		tileObject.transform.FindChild ("Object").gameObject.GetComponent<SpriteRenderer> ().sprite = blankImg; 
		tileObject.transform.FindChild ("Object").gameObject.transform.localScale = new Vector3 (1.565f, 1.565f, 1);



		/*Vector2 sprite_size = tileObject.GetComponent<SpriteRenderer>().sprite.bounds.size;
		Debug.Log ("A: " + sprite_size.x + " " + sprite_size.y);
		float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
		float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;
		Debug.Log ("B: " + worldScreenWidth);
		worldScreenWidth /= 5.0f;
		Debug.Log ("B: " + worldScreenWidth);
		Vector3 world_scale = new Vector3 ((float)(worldScreenWidth / sprite_size.x), (float)(worldScreenWidth / sprite_size.y) ,1.0f);
		*/

		float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
		float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;
		worldScreenWidth /= 5.0f;
		Vector3 world_scale = new Vector3 (worldScreenWidth, worldScreenWidth ,1.0f);
		tileObject.transform.localScale = world_scale;


		rb = tileObject.GetComponent<Rigidbody2D>();




		bomb = bmb;
		shown = shwn;


	}

	public bool isBomb() {
		return bomb;
	}

	public bool isShown() {
		return shown;
	}

	public void setBomb(bool bmb) {
		bomb = bmb;
	}

	public void setShown(bool shwn) {
		shown = shwn;
	}

	public void setNumber(int num) {
		if (num > 9 || num < 0)
			num = 0;
		
		tileObject.transform.FindChild("Number").gameObject.GetComponent<SpriteRenderer>().sprite = number[num];
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
