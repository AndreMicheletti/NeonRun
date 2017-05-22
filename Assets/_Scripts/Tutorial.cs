using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {


	public string TargetTag;
	public GameObject tutorialUI;
	public TouchMovements[] movementToExit;
	private bool toogle = false;
	private Vector2 touchOrigin = -Vector2.one;

	private int runCount = 0;

	void Start() {
		
		runCount = PlayerPrefs.GetInt("runCount");

		#if !UNITY_EDITOR
		if (runCount != null)
		if (runCount > 2)
			Destroy (gameObject);
		#endif
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (toogle) {
			handleInput ();
		}
		if (GameController.instance.isGameOver ()) {
			exitTutorial ();
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag (TargetTag)) {
			showTutorial ();
		}
	}

	void exitTutorial() {
		toogle = false;
		GameController.instance.toogleTutorial (false);
		tutorialUI.SetActive (false);
		Destroy (gameObject);
	}

	void showTutorial () {
		toogle = true;
		GameController.instance.toogleTutorial (true);
		tutorialUI.SetActive (true);
	}

	void handleInput() {

		TouchMovements touchInput = handleTouch();

		#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR

		if (Input.GetKeyDown (KeyCode.A)) {
			touchInput = TouchMovements.LEFT;
		}
		if (Input.GetKeyDown (KeyCode.D)) {
			touchInput = TouchMovements.RIGHT;
		}
		if (Input.GetKeyDown (KeyCode.S)) {
			touchInput = TouchMovements.DOWN;
		}
		if (Input.GetKeyDown (KeyCode.W)) {
			touchInput = TouchMovements.UP;
		}

		#endif

		foreach(TouchMovements mov in movementToExit) {
			if (mov == touchInput) {
				exitTutorial ();
				break;
			}
		}
	}

	void doAction(TouchMovements mov) {
		if (mov == TouchMovements.LEFT) {
			PlayerController.instance.moveHorizontal (-1);
		}
		if (mov == TouchMovements.RIGHT) {
			PlayerController.instance.moveHorizontal (1);
		}
		if (mov == TouchMovements.UP) {
			PlayerController.instance.attack ();
		}
		/*if (mov == TouchMovements.DOWN) {
			PlayerController.instance.defend ();
		}*/
	}

	TouchMovements handleTouch() {
		if (Input.touchCount > 0) {

			Touch myTouch = Input.touches[0];

			if (myTouch.phase == TouchPhase.Began) {
				touchOrigin = myTouch.position;
			} 

			else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0) {
				Vector2 touchEnd = myTouch.position;

				float x = touchEnd.x - touchOrigin.x;
				float y = touchEnd.y - touchOrigin.y;

				touchOrigin.x = -1;

				if (Mathf.Abs (x) > Mathf.Abs (y)) {
					return (x > 0 ? TouchMovements.RIGHT : TouchMovements.LEFT);
				} else {
					return (y > 0 ? TouchMovements.UP : TouchMovements.DOWN);
				}
			}
		}

		return TouchMovements.NONE;
	}
}
