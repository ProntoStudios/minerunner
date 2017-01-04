using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSquare : MonoBehaviour {

    bool left = false;
    bool right = false;
    bool up = false;
    bool down = false;

    // Use this for initialization
    void Start () {

	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            left = true;
        if (Input.GetKeyUp(KeyCode.LeftArrow))
            left = false;

        if (Input.GetKeyDown(KeyCode.RightArrow))
            right = true;
        if (Input.GetKeyUp(KeyCode.RightArrow))
            right = false;

        if (Input.GetKeyDown(KeyCode.UpArrow))
            up = true;
        if (Input.GetKeyUp(KeyCode.UpArrow))
            up = false;

        if (Input.GetKeyDown(KeyCode.DownArrow))
            down = true;
        if (Input.GetKeyUp(KeyCode.DownArrow))
            down = false;

        Vector3 position = this.transform.position;

        if (left)
            position.x--;
        if (right)
            position.x++;
        if (up)
            position.y++;
        if (down)
            position.y--;

        this.transform.position = position;
    }
}
