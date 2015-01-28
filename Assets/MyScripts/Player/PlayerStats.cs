using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour {

	public float speed;
	public float dmg;
	
	// Update is called once per frame
	void start () {
		speed = 3;
		dmg = 2;
	}

	public void addAttributes(float nSpeed)
	{
		speed += nSpeed;
	}
	public void addAttributes(float nSpeed, float nDamage)
	{
		speed += nSpeed;
		dmg += nDamage;
	}
}
