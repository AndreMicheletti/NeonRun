using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehaviour : MonoBehaviour {

	public float rotationSpeed = 10f;

	private Rigidbody2D body;

	// Use this for initialization
	void Awake () {
		body = GetComponent<Rigidbody2D> ();
		body.angularVelocity = rotationSpeed;
	}
	
	// Update is called once per frame
	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.CompareTag ("Player")) {
			Collect ();
		}
	}

	void Collect() {
		GameController.instance.coinCollected ();
		Destroy (gameObject);
	}
}
