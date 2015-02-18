using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour {

	public float speed;
	public float dmg;
	public bool magnetOn = false;
	public float health;
	public string pName;
	public string ID;

	public List<GameObject> goList = new List<GameObject>();

	void Start () {
		speed = 3;
		dmg = 2;
		health = 100;

	}

	public void setID(string newID){
		ID = newID;
		networkView.RPC("setPNetworkID", RPCMode.Others, newID);
	}

	[RPC]
	void setPNetworkID(string newID){
		ID = newID;
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
	public void takeDamage ( float incDmg ) {
		health -= incDmg;
		if (health <= 0) {
			Destroy (this.gameObject);
			print ("You suck");
		
		}
			
	}
}
