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


	public void damageTaken(float dmg){
		health -= dmg;

		if (health <= 0){
			List<GameObject> banana = gameObject.GetComponent<ChildList>().get();
			foreach(GameObject child in banana ){
				if(child != null){
					if(child.GetComponent<TrashType>().type == "Trash")
						child.GetComponent<TrashStats>().takeDamage(100000);
					if(child.GetComponent<TrashType>().type == "Turret")	
						child.GetComponent<TurretStats>().damageTaken(100000);
				}
		}
			Network.Destroy (this.gameObject);
			Instantiate(explosion, transform.position, transform.rotation);
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
