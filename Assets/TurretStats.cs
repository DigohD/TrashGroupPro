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

		if (health <= 0){
			Network.Destroy (this.gameObject);
		}
	}

	public void setTaken(string newOwnerID){
		isTaken = true;
		ownerID = newOwnerID;
		networkView.RPC("rpcWTurretTaken", RPCMode.Others, 0, newOwnerID);
	}

	public void setToBodyPart(){
		networkView.RPC ("rpcSetTurretToBodyPart", RPCMode.All, 0);
	}

	[RPC]
	void rpcWTurretTaken(int wasted, string newOwnerID){
		isTaken = true;
		ownerID = newOwnerID;
	}

	[RPC]
	void rpcSetTurretToBodyPart(int wasted){
		gameObject.tag = "BodyPart";
	}
}
