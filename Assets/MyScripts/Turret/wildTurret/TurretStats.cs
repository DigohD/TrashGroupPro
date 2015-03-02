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

	private NetworkViewID parent;

	public int damageTaken(float dmg, int comboCount){
				//if (health > 0) {
						health -= dmg;
						int initComboCount = comboCount;
						if (health <= 0) {
								comboCount++;
								List<NetworkViewID> banana = gameObject.GetComponent<ChildList> ().get ();
								foreach (NetworkViewID childID in banana) {
										GameObject child = NetworkView.Find(childID).gameObject;
										if (child != null){
											if (child.GetComponent<TrashType> ().type.Equals("Trash"))
														comboCount = child.GetComponent<TrashStats> ().takeDamage (10000.0f, comboCount);
												if (child.GetComponent<TrashType> ().type.Equals("Turret"))
														comboCount = child.GetComponent<TurretStats> ().damageTaken (10000.0f, comboCount);
											//banana.Remove(child.networkView.viewID);						
										}
						
								}
								
								if(initComboCount == 0)
									NetworkView.Find(parent).gameObject.GetComponent<ChildList>().removeChild(networkView.viewID);
								Network.Destroy (this.gameObject);
								Network.Instantiate (explosion, transform.position, transform.rotation, 0);
						}
				//}
		return comboCount;
	}

	public void setTaken(string newOwnerID, NetworkViewID newParent){
		isTaken = true;
		parent = newParent;
		ownerID = newOwnerID;
		networkView.RPC("rpcWTurretTaken", RPCMode.Others, 0, newOwnerID, newParent);
	}

	public void setToBodyPart(){
		networkView.RPC ("rpcSetTurretToBodyPart", RPCMode.All, 0);
	}

	[RPC]
	void rpcWTurretTaken(int wasted, string newOwnerID, NetworkViewID newParent){
		isTaken = true;
		parent = newParent;
		ownerID = newOwnerID;
	}

	[RPC]
	void rpcSetTurretToBodyPart(int wasted){
		gameObject.tag = "BodyPart";
	}
}
