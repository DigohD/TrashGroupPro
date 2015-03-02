using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrashStats : MonoBehaviour {

	// Use this for initialization
	public float health;
	//public float scale;
	public float speed;
	public float boost;
	public bool isTaken;
	public string type;
	AudioSource barrelAudio;
	public string ownerID;
	public GameObject explosion;
	public float maxHealth;

	void Start ()
	{
		if(type.Equals("Barrel")){
			barrelAudio = GetComponent<AudioSource> ();
		}
		maxHealth = health;
	}


	public void setTaken(string newOwnerID){
		isTaken = true;
		ownerID = newOwnerID;
		networkView.RPC("rpcSetTTaken", RPCMode.Others, 0, newOwnerID);
	}

	public void takeDamage ( float incDmg ) {
		health -= incDmg;
		gameObject.GetComponent<DamageBarrel> ().barrelHit ();

		if(type.Equals("Barrel"))
			barrelAudio.Play ();

		if (health <= 0){
			print("health zero, time to delete");
			List<GameObject> banana = gameObject.GetComponent<ChildList>().get();
			int counter = 0;
			foreach(GameObject child in banana ){
				Debug.Log("damaging: "+ gameObject);
				counter++;
				if(child != null){
					if(child.GetComponent<TrashType>().type == "Trash")
					child.GetComponent<TrashStats>().takeDamage(100000);
					if(child.GetComponent<TrashType>().type == "Turret")	
					child.GetComponent<TurretStats>().damageTaken(100000);



				}
				if(counter > 100)
					return;
				Debug.Log("Loop iteration: " + counter);
			}
			Destroy(this.gameObject);
			networkView.RPC("rpcTrashChainDestroy", RPCMode.Others, 0);
			Instantiate(explosion, transform.position, transform.rotation);	
		}
		
	}

	public void setToBodyPart(){
		networkView.RPC ("rpcSetBodyPart", RPCMode.All, 0);
	}

	[RPC]
	void rpcSetTTaken(int wasted, string newOwnerID){
		isTaken = true;
		ownerID = newOwnerID;
	}

	[RPC]
	void rpcSetBodyPart(int wasted){
		gameObject.tag = "BodyPart";
	}

	[RPC]
	void rpcTrashChainDestroy(int wasted){
		Destroy(this.gameObject);
	}
}
