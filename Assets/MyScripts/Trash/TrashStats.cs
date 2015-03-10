using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrashStats : MonoBehaviour {

	public float health;
	//public float scale;
	public float speed;
	public float rSpeed;
	public float boost;
	public bool isTaken;
	public string type;
	AudioSource barrelAudio;
	public string ownerID;
	public GameObject explosion;
	public float maxHealth;

	private NetworkViewID parent;
	private float timer;
	private bool destruct;

	// Use this for initialization
	void Start ()
	{
		if(type.Equals("Barrel")){
			barrelAudio = GetComponent<AudioSource> ();
		}
		maxHealth = health;
	}

	// Set this trash as owned by a player
	public void setTaken(string newOwnerID, NetworkViewID newParent){
		isTaken = true;
		ownerID = newOwnerID;
		parent = newParent;
		// Synchronize ownership changes on all other clients over the network
		networkView.RPC("rpcSetTTaken", RPCMode.Others, 0, newOwnerID, newParent);
	}

	/*
	 * Called when a piece of trash has taken damage.
	 * 
	 * If a piece of trash is destroyed, this function is used recursively to destroy
	 * all children of that piece of trash. The resulting return value is the total number of trash
	 * pieces destroyed. That number is then used to announce combos.
	 */ 
	public int takeDamage (float incDmg, int comboCount) {
		health -= incDmg;
		// initComboCount = the number of pieces already destroyed in the chain reaction
		int initComboCount = comboCount;
		gameObject.GetComponent<DamageBarrel> ().barrelHit ();

		// Play the barrel hit audio on all clients on the network
		if (type.Equals ("Barrel"))
			networkView.RPC("rpcBarrelAudio", RPCMode.All, 0);

		// If this trash is destroyed due to damage taken
		if (health <= 0) {
			comboCount++;

			// Fetch list of children attached to this trash
			List<NetworkViewID> banana = gameObject.GetComponent<ChildList> ().get ();

			// For each child in list
			foreach (NetworkViewID childID in banana) {
				// Fetch the GameObject of the child
				GameObject child = NetworkView.Find(childID).gameObject;
				// If child still exists, destroy it and update comboCount
				if (child != null) {
					if (child.GetComponent<TrashType> ().type.Equals("Trash"))
						comboCount = child.GetComponent<TrashStats> ().delayedDestruction (0.2f, comboCount);
					if (child.GetComponent<TrashType> ().type.Equals("Turret"))
						comboCount = child.GetComponent<TurretStats> ().delayedDestruction (0.2f, comboCount);
					//banana.Remove(child.networkView.viewID);
				}
			}
				
			// Remove this piece of trash from its parent's child list
			if(isTaken && initComboCount == 0)
				NetworkView.Find(parent).gameObject.GetComponent<ChildList>().removeChild(networkView.viewID);

			// Destroy trash
			Network.Destroy (this.gameObject);
			Network.Instantiate (explosion, transform.position, transform.rotation, 0);	
		}
		// Return the final comboCount
		return comboCount;
	}


	public void Update(){
		if (destruct) {
			timer-=Time.deltaTime;
			if(timer <=0){
				Network.Destroy (this.gameObject);
				Network.Instantiate (explosion, transform.position, transform.rotation, 0);
			}
		}
	}
	
	
	public int delayedDestruction(float time, int combo){
		destruct = true;
		timer = time;
		combo ++;
		List<NetworkViewID> banana = gameObject.GetComponent<ChildList> ().get ();
		foreach (NetworkViewID childID in banana) {
			GameObject child = NetworkView.Find(childID).gameObject;
			if (child != null){
				if (child.GetComponent<TrashType> ().type.Equals("Trash"))
					combo = child.GetComponent<TrashStats> ().delayedDestruction (time + 0.5f, combo);
				if (child.GetComponent<TrashType> ().type.Equals("Turret"))
					combo = child.GetComponent<TurretStats> ().delayedDestruction (time + 0.5f, combo);
				//banana.Remove(child.networkView.viewID);						
			}
		}
		return combo;
	}

	// Set this piece of trash as a body part, update all clients over the network
	public void setToBodyPart(){
		networkView.RPC ("rpcSetBodyPart", RPCMode.All, 0);
	}

	// Used to play barrel audio on connected clients
	[RPC]
	void rpcBarrelAudio(int wasted){
		barrelAudio.Play();
	}

	// Used to update trash ownership changes on connected clients
	[RPC]
	void rpcSetTTaken(int wasted, string newOwnerID, NetworkViewID newParent){
		isTaken = true;
		parent = newParent;
		ownerID = newOwnerID;
	}

	// Used to update bodyPart tag on connected clients
	[RPC]
	void rpcSetBodyPart(int wasted){
		gameObject.tag = "BodyPart";
	}
	
}
