using UnityEngine;
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
	

	public void damageTaken(float dmg){
				if (health > 0) {
						health -= dmg;

						if (health <= 0) {
								List<NetworkViewID> banana = gameObject.GetComponent<ChildList> ().get ();
								foreach (NetworkViewID childID in banana) {
										GameObject child = NetworkView.Find(childID).gameObject;
										if (child != null){
											if (child.GetComponent<TrashType> ().type.Equals("Trash"))
														child.GetComponent<TrashStats> ().takeDamage (10000.0f);
												if (child.GetComponent<TrashType> ().type.Equals("Turret"))
														child.GetComponent<TurretStats> ().damageTaken (10000.0f);
										}
				
								}
								Network.Destroy (this.gameObject);
								Network.Instantiate (explosion, transform.position, transform.rotation, 0);
						}
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
