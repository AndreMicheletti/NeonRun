using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileShooter : MonoBehaviour {

	public GameObject projectile;
	public Transform parent;
	public Range timeInSeconds;
	public RectTransform warningTransform;
	public float warningTransformOffset = 135f;
	public Animator warningAnimator;

	private bool active = false;

	void Awake () {

		setShooterActive (true);
	}

	void setShooterActive(bool a) {
		active = a;
		if (active == true)
			StartCoroutine (shootRoutine ());
	}

	void FixedUpdate() {
		if (GameController.instance.isGameOver ()) {
			active = false;
			StopAllCoroutines ();
		} else {
			if (GameController.instance.isPaused ()) {
				active = false;
				StopAllCoroutines ();
			} else {
				if (active == false) {
					setShooterActive (true);
				}
			}
		}
	}

	void shoot() {
		GameObject.Instantiate (projectile, new Vector3(transform.position.x, transform.position.y, -1), projectile.transform.rotation, parent);
	}

	void selectLane() {
		Vector3 moveTo = transform.localPosition;
		moveTo.x = Random.Range (-1, 2);
		transform.localPosition = moveTo;
		float rectX = moveTo.x * warningTransformOffset;
		warningTransform.localPosition = new Vector2 (rectX, warningTransform.localPosition.y);
	}

	IEnumerator shootRoutine() {
		do {
			yield return new WaitForSeconds (timeInSeconds.getRandom ());
			warningAnimator.SetTrigger ("Show");
			for (int i = 0; i < 3; i++) {
				selectLane ();
				yield return new WaitForSeconds (0.5f);
			}
			shoot ();
		} while (active);
	}
}
