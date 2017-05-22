using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreenHandler : MonoBehaviour {

	public GameObject titleScreenUI;
	public GameObject loadingScreen;
	public Text progressText;

	private bool loading = false;
	private int nextScene = -1;

	private AsyncOperation loadOperation;

	void Start () {
		loading = false;
		loadingScreen.SetActive (false);
	}

	void FixedUpdate () {
		if (loading) {
			// When loading
			loadingFeedback ();
		} else {
			// Check inputs if needed
			handleInput();
		}
	}

	void loadingFeedback() {
		if (loadOperation != null)
			progressText.text = (loadOperation.progress * 100.0f) + "%";

		// pulse progress text alpha
		Color newColor = progressText.color;
		newColor.a = Mathf.PingPong (Time.time, 1);
		progressText.color = newColor;
	}

	IEnumerator performLoad() {

		yield return new WaitForSeconds(2);

		loadOperation = SceneManager.LoadSceneAsync (nextScene);

		while (!loadOperation.isDone) {
			yield return null;
		}
	}

	void handleInput() {

	}

	/**
	 * PUBLIC FUNCTIONS 
	*/

	public void loadScene(int index) {
		nextScene = index;
		loading = true;
		StartCoroutine (performLoad ());
		titleScreenUI.SetActive (false);
		loadingScreen.SetActive (true);
	}

	public void openOptionsMenu() {
		// TODO implement options menu
	}

	public void exitGame() {
		
		// TODO put confirmation modal
		Application.Quit ();
	}
}
