using UnityEngine;
using System.Collections;

public class TrashSettings : MonoBehaviour {

	// Use this for initialization
	public float health;
	public float scale;

	
	// Update is called once per frame
	void Update () {
		if (health == 0)
						Destroy (this);
	}
}
