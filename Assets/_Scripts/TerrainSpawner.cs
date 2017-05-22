using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSpawner : MonoBehaviour {

	public GameObject[] terrainPrefabs;

	public float startY = 0;
	public float terrainHeight = 9;

	public Transform parent;

	private float spawnY = 0;
	private float lastPositionY = 0;

	// Use this for initialization
	void Start () {
		spawnY = startY;
		lastPositionY = PlayerController.instance.transform.position.y;
		spawnTerrain ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float diff = spawnY - lastPositionY;

		if (diff < terrainHeight) {
			spawnTerrain ();
		}

		lastPositionY = PlayerController.instance.transform.position.y;
	}

	void spawnTerrain() {
		GameObject selected = terrainPrefabs [Random.Range (0, terrainPrefabs.Length)];
		spawnY += terrainHeight;
		Vector3 position = new Vector3 (0, spawnY, 0);
		GameObject.Instantiate (selected, position, Quaternion.identity, parent);
	}
}
