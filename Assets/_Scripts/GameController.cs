using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if !UNITY_WEBGL
using UnityEngine.Advertisements;
#endif
using UnityEngine.EventSystems;

[System.Serializable]
public class Range {
	public float min;
	public float max;	

	public Range(float min, float max) {
		this.min = min;
		this.max = max;
	}

	public float getRandom() {
		return Random.Range (min, max);
	}

	public int getRandomInt() {
		return Random.Range((int) min, (int) max);
	}
}

public class GameController : MonoBehaviour {

	public static GameController instance;

	public GameObject gameOverUI;
	public GameObject pauseUI;
	//public Text scoreDisplay;
	public Text coinsDisplay;
	public Text bestCoinsDisplay;
	public Text finalScoreDisplay;

	public float fowardSpeed = 0.1f;
	//public int scoreGain = 1;

	public float speedIncreaseRate = 0.005f;
	public float speedIncreaseTime = 1;

	public float maxFowardSpeed = 0.4f;

	//[HideInInspector] public int score = 0;
	[HideInInspector] public int coins = 0;
	[HideInInspector] public int bestCoins = 0;

	private GameState gameState;

	private int runCount = 0;

	// Use this for initialization
	void Awake () {
		if (GameController.instance == null) {
			GameController.instance = this;
		} else if (GameController.instance != this) {
			Destroy (gameObject);
		}

		runCount = PlayerPrefs.GetInt("runCount");
		bestCoins = PlayerPrefs.GetInt ("bestCoins");
		bestCoinsDisplay.text = bestCoins + "";

		PlayerPrefs.SetInt ("runCount", runCount + 1);

		initializeGame ();
	}

	void initializeGame() {
		gameOverUI.SetActive (false);
		gameState = GameState.PLAY;
		increaseFowardSpeed ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		updateUI ();
		if (isGameOver ()) {
			Debug.Log ("GAMEOVER!");
		}
	}

	void updateUI() {
		/*scoreDisplay.text = "" + score;*/
		finalScoreDisplay.text = "" + coins;
		coinsDisplay.text = "" + coins;
	}

	void increaseFowardSpeed() {
		if (isGameOver ())
			return;
		
		if (!isPaused ()) {
			if (fowardSpeed < maxFowardSpeed)
				fowardSpeed += (fowardSpeed * speedIncreaseRate);
		}

		Invoke ("increaseFowardSpeed", speedIncreaseTime);
	}

	public void coinCollected() {
		coins += 1;
	}

	public bool isGameOver() {
		return gameState == GameState.GAMEOVER;
	}

	public void doGameOver() {
		changeState (GameState.GAMEOVER);
		gameOverUI.SetActive (true);
		if (coins > bestCoins)
			PlayerPrefs.SetInt ("bestCoins", coins);
	}

	public void loadLevel(int index) {
		SceneManager.LoadScene (index);
	}

	#if !UNITY_WEBGL
	public void ShowAd(string type) {
		ShowOptions options = new ShowOptions ();
		options.resultCallback = handleAdResult;
		if (Advertisement.IsReady ()) {
			Debug.Log ("Showing AD '" + type + "'");
			Advertisement.Show (type, options);
		}
	}

	public void ShowRewardedVideo() {
		ShowAd ("rewardedVideo");
	}
		
	private void handleAdResult(ShowResult result) {
		if (result == ShowResult.Finished) {
			Debug.Log ("AD Finished");
			respawn ();
		}
	}
	#endif

	public void respawn() {
		GameObject[] array = GameObject.FindGameObjectsWithTag ("Destructible");
		for (int i = 0; i < array.Length; i++) {
			Obstacle_Test obstacle = array [i].GetComponent<Obstacle_Test> ();
			if (obstacle.isVisibleOnScreen ()) {
				obstacle.gameObject.SetActive (false);
			}
		}
		PlayerController.instance.MoveTo (0, -1);
		Invoke ("initializeGame", 1f);
	}

	void changeState(GameState s) {
		gameState = s;
	}

	public void tooglePause() {
		if (!isGameOver ()) {
			gameState = (gameState == GameState.PLAY ? GameState.PAUSE : GameState.PLAY);
			pauseUI.SetActive (isPaused ());
		}
	}

	public void toogleTutorial(bool tutorial) {
		if (!isGameOver ()) {
			gameState = (tutorial == true ? GameState.TUTORIAL : GameState.PLAY);
		}
	}

	public bool isPaused() {
		return gameState == GameState.PAUSE || gameState == GameState.TUTORIAL;
	}

	public static bool IsPointerOverUIObject() {
		PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
		eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		List<RaycastResult> results = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
		return results.Count > 0;
	}

}

public enum GameState {
	PLAY,
	PAUSE,
	TUTORIAL,
	GAMEOVER
}

