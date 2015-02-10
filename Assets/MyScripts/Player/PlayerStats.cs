using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour {

	public float speed;
	public float dmg;
	public bool magnetOn = false;
	public float health;

	public List<GameObject> goList = new List<GameObject>();


	
	// Update is called once per frame
	void start () {
		speed = 3;
		dmg = 2;
		health = 100;

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
	public void magnetSwitch(){
		magnetOn = !magnetOn;
	}
}
