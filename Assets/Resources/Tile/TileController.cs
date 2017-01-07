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

	public Tile(float x, float y, float length, bool bmb, bool shwn) {
		tileObject = (GameObject) Instantiate(Resources.Load("Tile/Tile"));

		tileObject.transform.localPosition = new Vector3 (x, y, (float)LAYER);

		tileObject.GetComponentInChildren<Transform> ().Find ("Background").transform.localPosition = new Vector3(0, 0, LAYER-1);
		tileObject.GetComponentInChildren<Transform> ().Find ("Object").transform.localPosition = new Vector3(0, 0, LAYER-2);
		tileObject.GetComponentInChildren<Transform> ().Find ("Number").transform.localPosition = new Vector3(0, 0, LAYER-3);

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
}