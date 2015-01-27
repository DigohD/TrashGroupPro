using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour {

	public float speed;
	
	// Update is called once per frame
	void start () {
		speed = 3;
	}

	public void addAttributes(float nSpeed)
	{
		speed += nSpeed;
	}
}
