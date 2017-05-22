using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	[HideInInspector] public static PlayerController instance;

	public float moveTime = 1f;

	private Rigidbody2D body;

	private float inverseMoveTime;

	public Animator animator;

	[HideInInspector] public PlayerMovementState movState;
	[HideInInspector] public PlayerCombatState comState;

	//private Vector2 touchOrigin = -Vector2.one;

	private int HIT_WALL = 11;
	private int hitTimer = 0;

	private int runCount = 0;

	// Use this for initialization
	void Awake () {
		if (PlayerController.instance == null) {
			PlayerController.instance = this;
		} else if (PlayerController.instance != this) {
			Destroy (gameObject);
		}

		runCount = PlayerPrefs.GetInt("runCount");

		body = GetComponent<Rigidbody2D> ();
		inverseMoveTime = 1f / moveTime;
	}

	void Update() {
		if (GameController.instance.isPaused ())
			return;
		if (movState == PlayerMovementState.GAMEOVER)
			return;

		updateCombatState ();
		updateMovementState ();
		handleInput ();
	}

	
	// Update is called once per frame
	void FixedUpdate () {
		if (GameController.instance.isPaused ())
			return;
		if (movState == PlayerMovementState.GAMEOVER)
			return;

		moveFoward ();
		//GameController.instance.addScore ();

	}

	void moveFoward() {
		Vector3 newPos = body.position;
		newPos.y += GameController.instance.fowardSpeed;
		body.MovePosition (newPos);
	}

	void updateMovementState() {
		if (GameController.instance.isGameOver ()) {
			changeState (PlayerMovementState.GAMEOVER);	
		}
	}

	void updateCombatState() {
		if (comState == PlayerCombatState.ATTACKING) {
			if (!AnimatorAPI.isAnimationPlaying (animator, "Attack")) {
				changeState (PlayerCombatState.NONE);
			}
		}
		/*if (comState == PlayerCombatState.DEFENDING) {
			//gameObject.GetComponentInChildren<MeshRenderer> ().materials [0].color = new Color (255, 255, 255);
			if (!AnimatorAPI.isAnimationPlaying (animator, "Defend")) {
				changeState (PlayerCombatState.NONE);
			}
		} else {
			//gameObject.GetComponentInChildren<MeshRenderer> ().materials [0].color = new Color (255, 255, 255);
		}*/
	}

	void handleInput() {
		#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR || UNITY_WEBGL
			handleKeyBoardInput();
		#else

		TouchMovements touchInput = handleTouch();
		if (touchInput == TouchMovements.NONE)
			return;

		if (comState == PlayerCombatState.NONE) {
			if (touchInput == TouchMovements.UP) {
				attack ();
			}
		}

		#endif

	}

	public void handleClick(int direction) {
		if (GameController.instance.isGameOver () || GameController.instance.isPaused())
			return;
		
		if (movState == PlayerMovementState.RUNNING && comState != PlayerCombatState.ATTACKING ) {
			moveHorizontal (direction);
		}
	}

	TouchMovements handleTouch() {
		if (Input.touchCount > 0) {

			Touch myTouch = Input.touches[0];

			if (myTouch.phase == TouchPhase.Began) {
				if (!GameController.IsPointerOverUIObject()) {
					return TouchMovements.UP;
				}
			} 				
		}

		return TouchMovements.NONE;
	}

	TouchMovements handleTouchOld() {
		if (Input.touchCount > 0) {
			
			Touch myTouch = Input.touches[0];

			if (myTouch.phase == TouchPhase.Began) {
				Vector2 pos = myTouch.position;

				if (pos.y > Screen.height / 2.0f - (Screen.height / 4.0f)) {
					return TouchMovements.UP;
				} else if (pos.x < Screen.width / 2.0f) {
					return TouchMovements.LEFT;
				} else if (pos.x > Screen.width / 2.0f) {
					return TouchMovements.RIGHT;
				}
			} 				
		}

		return TouchMovements.NONE;
	}

	void handleKeyBoardInput() {
		if (comState == PlayerCombatState.NONE) {
			if (Input.GetKeyDown (KeyCode.W)) {
				attack ();
			}
		}
		if (movState != PlayerMovementState.MOVING && comState != PlayerCombatState.ATTACKING) {
			if (Input.GetKeyDown (KeyCode.A)) {
				moveHorizontal (-1);
			}
			if (Input.GetKeyDown (KeyCode.D)) {
				moveHorizontal (1);
			}
		}
	}

	public void attack() {
		if (comState == PlayerCombatState.ATTACKING)
			return;
		animator.SetTrigger ("Attack");
		changeState (PlayerCombatState.ATTACKING);
	}

	/*public void defend() {
		if (comState == PlayerCombatState.DEFENDING)
			return;
		animator.SetTrigger ("Defend");
		changeState (PlayerCombatState.DEFENDING);
	}*/

	public void moveHorizontal(int dir) {
		if (movState == PlayerMovementState.MOVING)
			return;
		changeState (PlayerMovementState.MOVING);
		StartCoroutine (smoothMovement (transform.position.x + dir));
	}

	void changeState(PlayerMovementState s) {
		movState = s;
	}
	void changeState(PlayerCombatState s) {
		comState = s;
	}

	IEnumerator smoothMovement(float targetX) {
		float RemainingDistance = Mathf.Abs(transform.position.x - targetX);

		while (RemainingDistance > 0f) {
			float x = Mathf.MoveTowards (transform.position.x, targetX, inverseMoveTime * Time.deltaTime);
			transform.position = new Vector3(x, transform.position.y, 0);
			RemainingDistance = Mathf.Abs(transform.position.x - targetX);
			yield return new WaitForFixedUpdate();
		}
		changeState (PlayerMovementState.RUNNING);
	}

	void OnCollisionStay2D(Collision2D coll) {
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Terrain")) {
			hitTimer += 1;
			//loseBalance ();
			Debug.Log ("OOPS");
			if (hitTimer >= HIT_WALL || isOffBalance() == true) {
				Die ();
			}
		}
	}

	void OnCollisionExit2D(Collision2D coll) {
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Terrain")) {
			hitTimer = 0;
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag ("Projectile")) {
			//if (comState != PlayerCombatState.DEFENDING) {
				Die ();
			//}
			Destroy (other.gameObject);
		}
	}

	public void MoveTo(int x, int y) {
		transform.position = new Vector2 ( (int) transform.position.x + x, (int) transform.position.y + y);
	}

	void Die() {
		changeState (PlayerMovementState.GAMEOVER);
		GameController.instance.doGameOver ();
	}

	bool isOffBalance() {
		return false;
		//return AnimatorAPI.isAnimationPlaying (animator, "Off Balance");
	}

	void loseBalance() {
		animator.SetTrigger ("OffBalance");
	}

}

public enum PlayerMovementState {
	RUNNING,
	MOVING,
	OFFBALANCE,
	GAMEOVER
}

public enum PlayerCombatState {
	ATTACKING,
	NONE
}

public enum TouchMovements {
	UP,
	DOWN,
	LEFT,
	RIGHT,
	NONE
}