using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour {

	public Transform target;
	public Vector2 offset;

	public bool followX = false;
	public bool followY = false;
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 pos = transform.position;
		if (followX)
			pos.x = target.position.x + offset.x;
		if (followY)
		pos.y = target.position.y + offset.y;

		transform.position = pos;
	}
}
