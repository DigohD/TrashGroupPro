using UnityEngine;
using System.Collections;

public class TurretStats : MonoBehaviour {

	public float damage;
	public float health;
	public string type;
	public float timeBetweenBullets;
	public bool isTaken;
	public string ownerID;
	public float speed;

	public void damageTaken(float dmg){
		health -= dmg;

		if (health <= 0)
			Destroy (gameObject);
	}

	public void setTaken(string newOwnerID){
		isTaken = true;
		networkView.RPC("rpcTaken", RPCMode.Others, 0);
		ownerID = newOwnerID;
		networkView.RPC("rpcTaken", RPCMode.Others, 0, newOwnerID);
	}

	[RPC]
	void rpcTaken(int wasted, string newOwnerID){
		isTaken = true;
		ownerID = newOwnerID;
	}
}
