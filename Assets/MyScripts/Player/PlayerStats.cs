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
	public GameObject explosion;

	public List<GameObject> goList = new List<GameObject>();

	void Start () {
		speed = 3;
		dmg = 2;
		//health = 100;
	}

	public void setID(string newID){
		ID = newID;
		networkView.RPC("setPNetworkID", RPCMode.AllBuffered, newID);
	}
	
	[RPC]
	void setPNetworkID(string newID){
		//Debug.Log("Set New ID" + newID);
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
		this.gameObject.GetComponent<HealthBarScript> ().isHit ();
		if (health <= 0) {
			Camera cam = GetComponent<Camera>();
			if(networkView.isMine){
				Destroy(cam);
			}
			else{
				Destroy(cam);
			}
			Network.Destroy (this.gameObject);
			Instantiate(explosion, transform.position, transform.rotation);
			print ("You suck");
		
		}
			
	}
}
