using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour {
	
	const float moveSpeedRatio = 20f;

	float innerDistance;
	float downSpeed;

	Vector2 moving = new Vector3(0, 0);
	float distanceRatio = 5f;

	Tile destTile;
	Tile startTile;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (moving.x == -1) {
			if (transform.localPosition.x <= destTile.getX()) {
				moving.x = 0;
				transform.localPosition = new Vector3(destTile.getX(), transform.localPosition.y, transform.localPosition.z);
				transform.GetComponent<Rigidbody2D>().velocity = new Vector3 (0, downSpeed, 0);
				tileGeneration.instance.movingPlayer = false;
			}
			if (transform.localPosition.x <= destTile.getX () + tileGeneration.instance.sideLength / distanceRatio) {
				startTile.updatePath ();
				destTile.updatePath ();
			}
		}
		if (moving.x == 1) {
			if (transform.localPosition.x >= destTile.getX()) {
				moving.x = 0;
				transform.localPosition = new Vector3(destTile.getX(), transform.localPosition.y, transform.localPosition.z);
				transform.GetComponent<Rigidbody2D>().velocity = new Vector3 (0, downSpeed, 0);
				tileGeneration.instance.movingPlayer = false;
			}
			if (transform.localPosition.x >= destTile.getX () - tileGeneration.instance.sideLength / distanceRatio) {
				startTile.updatePath ();
				destTile.updatePath ();
			}
		}
		if (moving.y == 1) {
			if (transform.localPosition.y >= destTile.getY()) {
				moving.y = 0;
				transform.localPosition = new Vector3(transform.localPosition.x, destTile.getY(), transform.localPosition.z);
				transform.GetComponent<Rigidbody2D>().velocity = new Vector3 (0, downSpeed, 0);
				tileGeneration.instance.movingPlayer = false;
			}
			if (transform.localPosition.y >= destTile.getY () - tileGeneration.instance.sideLength / distanceRatio) {
				startTile.updatePath ();
				destTile.updatePath ();
			}
		}
		if (moving.y == -1) {
			if (transform.localPosition.y <= destTile.getY()) {
				moving.y = 0;
				transform.localPosition = new Vector3(transform.localPosition.x, destTile.getY(), transform.localPosition.z);
				transform.GetComponent<Rigidbody2D>().velocity = new Vector3 (0, downSpeed, 0);
				tileGeneration.instance.movingPlayer = false;
			}
			if (transform.localPosition.y <= destTile.getY () + tileGeneration.instance.sideLength / distanceRatio) {
				startTile.updatePath ();
				destTile.updatePath ();
			}
		}
		//Debug.Log (transform.localPosition);
	}

	public void setPosition (float x, float y) {
		transform.localPosition = new Vector3 (x, y, 17.5f);
	}

	public void move (int x, int y, Tile destination, Tile start) {

		// move by sidelength up/down x=1/-1, or left/right y=1/-1
		if (moving.x != 0 || moving.y != 0) {
			// only allow one movement at a time.
			return;
		}

		destTile = destination;

		startTile = start;

		moving = new Vector2 (x, y);
		transform.GetComponent<Rigidbody2D> ().velocity += 
			new Vector2 (x * Mathf.Abs(transform.GetComponent<Rigidbody2D> ().velocity.y) * moveSpeedRatio,
				y * Mathf.Abs(transform.GetComponent<Rigidbody2D> ().velocity.y) * moveSpeedRatio);
		
		//Debug.Log ("moving (" + x + "," + y + ")");

	}

	public void setDownwardSpeed (float speed) {
		downSpeed = speed;
		transform.GetComponent<Rigidbody2D>().velocity = new Vector3 (0, speed, 0);
	}

}	
