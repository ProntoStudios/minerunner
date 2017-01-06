using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tileGeneration : MonoBehaviour {

	//public GameObject testingSquare;

	// Use this for initialization
	void Start () {
		Debug.Log (Screen.width);
		Debug.Log (Screen.height);

		//get world space size (this version handles rotating correctly)
		Vector2 sprite_size = GetComponent<SpriteRenderer>().sprite.rect.size;
		Vector2 local_sprite_size = sprite_size / GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
		Vector3 world_size = local_sprite_size;
		world_size.x *= transform.lossyScale.x;
		world_size.y *= transform.lossyScale.y;

		double sideLength = Screen.width / 5.0;
		for (int h = 0; h*sideLength < Screen.height; h++) {
			for (int x = 0; x < 5; x++) {
				Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3((float)(x*sideLength),(float)(h*sideLength), 10.0f));
				GameObject sq = (GameObject) Instantiate(Resources.Load("testingSquare"), worldPos, Quaternion.Euler(0, 0, 0));
				//sq.GetComponent<Texture2D>().Resize ((int)(Screen.width * 0.2f), (int)(Screen.width * 0.2f));
			}
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
