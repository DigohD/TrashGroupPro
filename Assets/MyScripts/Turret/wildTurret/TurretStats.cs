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
	private float timer;
	private bool destruct;

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
														comboCount = child.GetComponent<TrashStats> ().delayedDestruction (0.5f, comboCount);
												if (child.GetComponent<TrashType> ().type.Equals("Turret"))
														comboCount = child.GetComponent<TurretStats> ().delayedDestruction ( 0.5f, comboCount);
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
