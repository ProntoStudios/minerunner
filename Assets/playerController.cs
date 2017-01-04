using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
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
