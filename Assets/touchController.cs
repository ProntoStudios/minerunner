﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class touchController : MonoBehaviour {

	public const float threshhold = 15f;
	private int currentTouchId = -1;
	private Vector2 touchStartPosition;

	void Update () {

		if (SystemInfo.deviceType == DeviceType.Desktop) {
			if ( Input.GetMouseButtonDown (0)){ 
				Tap(Input.mousePosition);
			}
			if (Input.GetKey (KeyCode.LeftArrow)) {
				tileGeneration.instance.movePlayerLeft ();
			}
			if (Input.GetKey (KeyCode.RightArrow)) {
				tileGeneration.instance.movePlayerRight ();
			}
			if (Input.GetKey (KeyCode.UpArrow)) {
				tileGeneration.instance.movePlayerUp ();
			}
			if (Input.GetKey (KeyCode.DownArrow)) {
				tileGeneration.instance.movePlayerDown ();
			}
		}

		if(Input.touchCount > 0 ){
			switch (Input.GetTouch(0).phase) {
			case TouchPhase.Began:
				currentTouchId = Input.GetTouch (0).fingerId;
				touchStartPosition = Input.GetTouch (0).position;
				break;
			case TouchPhase.Moved:
				if (currentTouchId != -1) {//if touch not already handled
					Vector2 touchDeltaPosition = Input.GetTouch(0).position - touchStartPosition;//Input.GetTouch (0).deltaPosition;
					//Check if it is left or right?
					if (touchDeltaPosition.x < -1*threshhold && Mathf.Abs(touchDeltaPosition.x) > Mathf.Abs(touchDeltaPosition.y)) {
						Debug.Log ("left");
						tileGeneration.instance.movePlayerLeft();
						currentTouchId = -1;
					} else if (touchDeltaPosition.x > threshhold && Mathf.Abs(touchDeltaPosition.x) > Mathf.Abs(touchDeltaPosition.y)) {
						Debug.Log ("right");
						tileGeneration.instance.movePlayerRight();
						currentTouchId = -1;
					} else if (touchDeltaPosition.y < -1*threshhold) {
						Debug.Log ("down");
						tileGeneration.instance.movePlayerDown();
						currentTouchId = -1;
					} else if (touchDeltaPosition.y > threshhold) {
						Debug.Log ("up");
						tileGeneration.instance.movePlayerUp();
						currentTouchId = -1;
					} 
				}
				break;
			case TouchPhase.Stationary:
				if (currentTouchId != -1) {//if touch not already handled
					touchStartPosition = Input.GetTouch(0).position;
				}
				break;
			case TouchPhase.Ended:
				if (currentTouchId != -1) {//if touch not already handled
					Debug.Log ("tap");
					Tap(touchStartPosition);
					currentTouchId = -1;
				}
				break;
			case TouchPhase.Canceled:
				currentTouchId = -1;
				break;	
			}

		}

	}


	void Tap (Vector2 pos) {
		Vector3 worldTouch = Camera.main.ScreenToWorldPoint(pos); 
		RaycastHit2D hit = Physics2D.Raycast (new Vector2 (worldTouch.x, worldTouch.y), Vector2.zero, Mathf.Infinity);
		if (hit != null) {
			int x = hit.transform.GetComponent<TileObjectAttributes>().getIndexX();
			int y = hit.transform.GetComponent<TileObjectAttributes>().getIndexY();
			Debug.Log("Selected tile at " + x+","+y); // ensure you picked right object
			tileGeneration.instance.tileTapped(x,y);
		}
	}
}
