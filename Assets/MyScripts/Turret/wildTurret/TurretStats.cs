﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurretStats : MonoBehaviour {


	public GameObject explosion;
	public float damage;
	public float health;
	public string type;
	public float timeBetweenBullets;
	public bool isTaken;
	public string ownerID;

	public float speed;
	public float rSpeed;

	private NetworkViewID parent;
	private float timer;
	private bool destruct;

	/*
	 * Called when a turret has taken damage.
	 * 
	 * If a turret is destroyed, this function is used recursively to destroy
	 * all children of that turret. The resulting return value is the total number of trash
	 * pieces destroyed. That number is then used to announce combos.
	 */ 
	public int damageTaken(float dmg, int comboCount){
		health -= dmg;
		// initComboCount = the number of pieces already destroyed in the chain reaction
		int initComboCount = comboCount;

		// If this turret is destroyed due to damage taken
		if (health <= 0) {
			comboCount++;

			// Fetch list of children attached to this turret
			List<NetworkViewID> banana = gameObject.GetComponent<ChildList> ().get ();

			// For each child in list
			foreach (NetworkViewID childID in banana) {
				// Fetch the GameObject of the child
				GameObject child = NetworkView.Find(childID).gameObject;
				// If child still exists, destroy it and update comboCount
				if (child != null){
					if (child.GetComponent<TrashType> ().type.Equals("Trash"))
						comboCount = child.GetComponent<TrashStats> ().delayedDestruction (0.5f, comboCount);
					if (child.GetComponent<TrashType> ().type.Equals("Turret"))
						comboCount = child.GetComponent<TurretStats> ().delayedDestruction ( 0.5f, comboCount);
					//banana.Remove(child.networkView.viewID);						
				}
			}

			// Remove this turret from its parent's child list
			if(initComboCount == 0)
				NetworkView.Find(parent).gameObject.GetComponent<ChildList>().removeChild(networkView.viewID);

			// Destroy turret
			Network.Destroy (this.gameObject);
			Network.Instantiate (explosion, transform.position, transform.rotation, 0);
		}
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
					combo = child.GetComponent<TrashStats> ().delayedDestruction (time + 0.2f, combo);
				if (child.GetComponent<TrashType> ().type.Equals("Turret"))
					combo = child.GetComponent<TurretStats> ().delayedDestruction (time + 0.2f, combo);
				//banana.Remove(child.networkView.viewID);						
			}
		}
		return combo;
	}
	
	public void setTaken(string newOwnerID, NetworkViewID newParent){
		isTaken = true;
		parent = newParent;
		ownerID = newOwnerID;
		networkView.RPC("rpcWTurretTaken", RPCMode.Others, 0, newOwnerID, newParent);
	}

	// Set this turret as a body part, update all clients over the network
	public void setToBodyPart(){
		networkView.RPC ("rpcSetTurretToBodyPart", RPCMode.All, 0);
	}

	// Used to update turret ownership changes on connected clients
	[RPC]
	void rpcWTurretTaken(int wasted, string newOwnerID, NetworkViewID newParent){
		isTaken = true;
		parent = newParent;
		ownerID = newOwnerID;
	}

	// Used to update bodyPart tag on connected clients
	[RPC]
	void rpcSetTurretToBodyPart(int wasted){
		gameObject.tag = "BodyPart";
	}
}
