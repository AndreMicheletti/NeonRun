using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabInstatiate : MonoBehaviour {

	public GameObject prefab;
	public float delay = 0.0f;

	private GameObject instance;

	// Use this for initialization
	void Start () {
		Invoke ("Instantiate", delay);
	}
	
	private void Instantiate() {
		instance = GameObject.Instantiate (prefab, transform);
	}
}
