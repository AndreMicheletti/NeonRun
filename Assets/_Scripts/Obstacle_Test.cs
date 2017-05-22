using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_Test : MonoBehaviour {

	public KeyCode triggerKey = KeyCode.W;
	public GameObject explosion;
	private bool explode = false;


	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag ("Player")) {
			if (PlayerController.instance.comState == PlayerCombatState.ATTACKING) {
				DestroyThis ();
			}
		}
	}

	void DestroyThis() {
		Destroy (gameObject);
		if (!explode) 
			GameObject.Instantiate (explosion, transform.position, Quaternion.identity);
		explode = true;		
	}

	public bool isVisibleOnScreen() {
		Vector3 screenPoint = Camera.main.WorldToScreenPoint (transform.position);
		if (screenPoint.x > 0 && screenPoint.x < Screen.width)
			if (screenPoint.y > 0 && screenPoint.y < Screen.height)
				return true;

		return false;
	}
}
