using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class touchController : MonoBehaviour {

	void Update () {


		if ( Input.GetMouseButtonDown (0)){ 
			//Debug.Log ("mouse down");
			Vector3 worldTouch = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
			RaycastHit2D hit = Physics2D.Raycast (new Vector2 (worldTouch.x, worldTouch.y), Vector2.zero, Mathf.Infinity);
			if (hit != null) {
				Debug.Log("You selected the " + hit.transform.name); // ensure you picked right object
			}
		}



		if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Stationary)
		{
			Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;

			//Check if it is left or right?
			if(touchDeltaPosition.x < 0.0f){
				this.transform.Translate(Vector3.left * 10 * Time.deltaTime);
			} else if (touchDeltaPosition.x > 0.0f) {
				this.transform.Translate(Vector3.right * 10 * Time.deltaTime);
			}

		}

	}
}
