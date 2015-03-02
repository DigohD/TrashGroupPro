using UnityEngine;
using System.Collections;

public class ShotTimer : MonoBehaviour {

	public float timer = 0;
	void Start () {
	 if (timer == 0)
						timer = 1.5f;
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if (timer <= 0)
						Destroy (this.gameObject);
	
	}
}
