using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour {

	public float speed;
	public float rSpeed;
	public float dmg;
	public bool magnetOn = false;
	public float health;
	public string pName;

	// The player user name
	public string ID;

	public GameObject explosion;

	public List<GameObject> goList = new List<GameObject>();

	void Start () {
		//speed = 3;
		//dmg = 2;
		//health = 100;
	}

	// Set the user name of this player object, and update the same player object on all clients, using RPC
	public void setID(string newID){
		ID = newID;
		networkView.RPC("setPNetworkID", RPCMode.AllBuffered, newID);
	}

	// Used to synchronize player ID's over the network
	[RPC]
	void setPNetworkID(string newID){
		ID = newID;
	}

	public void addAttributes(float nSpeed)
	{
		speed += nSpeed;
	}

	public void addAttributes(float nSpeed, float nRSpeed)
	{
		speed += nSpeed;
		rSpeed += nRSpeed;
	}

	public void addAttributes(float nSpeed, float nDamage, float nRSpeed)
	{
		speed += nSpeed;
		dmg += nDamage;
		rSpeed += nRSpeed;
	}

	public void magnetSwitch(){
		magnetOn = !magnetOn;
		if (magnetOn)
			gameObject.GetComponent<Magnet> ().turnOn ();
		else
			gameObject.GetComponent<Magnet> ().turnOff ();
	}

	// Updates the damage on all connected clients, using RPC
	public void takeDamage ( float incDmg ) {
		networkView.RPC ("rpcPlayerTakeDamage", RPCMode.All, incDmg);
	}

	// Used to calculate damage dealt to a player object. This is an RPC, since it needs to be updated
	// on all clients.
	[RPC]
	void rpcPlayerTakeDamage(float incDmg){
		// Subtract damage taken from health
		health -= incDmg;
		// Update green portion of the health bar
		this.gameObject.GetComponent<HealthBarScript> ().isHit ();
		
		if (health <= 0) {
			// Destroy the camera
			Camera cam = GetComponent<Camera>();
			if(networkView.isMine){
				Destroy(cam);
			}
			else{
				Destroy(cam);
			}
			// Destroy this player object
			Network.Destroy (this.gameObject);
			// Create explosion effect
			Instantiate(explosion, transform.position, transform.rotation);
			print ("You suck");
		}
	}
}
