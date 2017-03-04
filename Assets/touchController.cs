using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class touchController : MonoBehaviour {

	public const float threshhold = 15f;
	private int currentTouchId = -1;
	private Vector2 touchStartPosition;

	void Update () {


		/*if ( Input.GetMouseButtonDown (0)){ 
			//Debug.Log ("mouse down");
			Vector3 worldTouch = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
			RaycastHit2D hit = Physics2D.Raycast (new Vector2 (worldTouch.x, worldTouch.y), Vector2.zero, Mathf.Infinity);
			if (hit != null) {
				Debug.Log("You selected the " + hit.transform.name); // ensure you picked right object
			}
		}*/


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
						currentTouchId = -1;
					} else if (touchDeltaPosition.x > threshhold && Mathf.Abs(touchDeltaPosition.x) > Mathf.Abs(touchDeltaPosition.y)) {
						Debug.Log ("right");
						currentTouchId = -1;
					} else if (touchDeltaPosition.y < -1*threshhold) {
						Debug.Log ("down");
						currentTouchId = -1;
					} else if (touchDeltaPosition.y > threshhold) {
						Debug.Log ("up");
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
					currentTouchId = -1;
				}
				break;
			case TouchPhase.Canceled:
				currentTouchId = -1;
				break;	
			}

		}

	}
}
