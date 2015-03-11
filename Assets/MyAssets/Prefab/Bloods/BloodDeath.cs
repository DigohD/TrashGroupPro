using UnityEngine;
using System.Collections;

public class BloodDeath : MonoBehaviour {

	ParticleSystem ps;

	void Start () {
		ps = gameObject.GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!ps.isPlaying)
			Destroy(this.gameObject);
	}
}
