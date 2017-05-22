using UnityEngine;
using System.Collections;

public class DestroyByParticle : MonoBehaviour {

	public new ParticleSystem particleSystem;

	// Use this for initialization
	void Start () {
	
	}

	void FixedUpdate() {
		if (particleSystem.isStopped) {
			Destroy (gameObject);
		}
	}
}
