using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoFoward : MonoBehaviour {

	public float speed = 1f;
	
	// Update is called once per frame
	void Update () {
		if (GameController.instance.isPaused ())
			return;
		transform.position += (transform.up * Time.deltaTime * speed);
	}
}
