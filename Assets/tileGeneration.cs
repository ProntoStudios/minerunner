using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tileGeneration : MonoBehaviour {

	float speed;

	//public GameObject testingSquare;

	// Use this for initialization
	void Start () {
		Debug.Log (Screen.width);
		Debug.Log (Screen.height);

		speed = -0.05f;

		float sideLength = Screen.width / 5.0f;
		for (int h = 0; h*sideLength < Screen.height; h++) {
			for (int x = 0; x < 5; x++) {
				Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(sideLength/2 +x*sideLength,Screen.height-sideLength/2-h*sideLength, 10.0f));
				Tile sq = new Tile (worldPos.x, worldPos.y, 1, true, true);
				sq.setNumber (Random.Range(0,10));
				sq.setDownwardSpeed(speed);
				//sq.GetComponent<Texture2D>().Resize ((int)(Screen.width * 0.2f), (int)(Screen.width * 0.2f));

				/*Debug.Log (sideLength);
				//get world space size (this version handles rotating correctly)
				Vector2 sprite_size = sq.GetComponent<SpriteRenderer>().sprite.bounds.size;
				Debug.Log ("A: " + sprite_size.x + " " + sprite_size.y);
				float pixels_per_unit = sq.GetComponent<SpriteRenderer> ().sprite.pixelsPerUnit;
				Debug.Log ("B: " + pixels_per_unit);
				Vector2 local_sprite_size = sprite_size / pixels_per_unit;
				local_sprite_size.x -= .2f;
				local_sprite_size.y -= .2f;
				Debug.Log ("C: " + local_sprite_size.x + " " + local_sprite_size.y);
				Vector3 world_scale = new Vector3 ((float)(sideLength/local_sprite_size.x), (float)(sideLength/local_sprite_size.y) ,1.0f);
				Debug.Log ("D: " + world_scale.x + " " + world_scale.y + " " + world_scale.z);
				//sq.transform.localScale = world_scale;*/

				/*
				Vector2 sprite_size = sq.GetComponent<SpriteRenderer>().sprite.bounds.size;
				Debug.Log ("A: " + sprite_size.x + " " + sprite_size.y);
				float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
				float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;
				Debug.Log ("B: " + worldScreenWidth);
				worldScreenWidth /= 5.0f;
				Debug.Log ("B: " + worldScreenWidth);
				Vector3 world_scale = new Vector3 ((float)(worldScreenWidth / sprite_size.x), (float)(worldScreenWidth / sprite_size.y) ,1.0f);
				Debug.Log ("C: " + world_scale.x + " " + world_scale.y + " " + world_scale.z);
				sq.transform.localScale = world_scale;

				*/
			}
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
