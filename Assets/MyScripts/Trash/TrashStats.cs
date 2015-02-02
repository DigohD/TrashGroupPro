using UnityEngine;
using System.Collections;

public class TrashStats : MonoBehaviour {

	// Use this for initialization
	public float health;
	//public float scale;
	public float speed;

	void start ()
	{
		health = 10;
		speed = 1;
	}
	
	// Update is called once per frame
	void Update () {
		if (health == 0)
						Destroy (this);
	}
}
