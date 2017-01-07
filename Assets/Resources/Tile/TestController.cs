using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Tile test = new Tile (-0.5f, 0.5f, 1, true, false);
		test.setNumber (1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
