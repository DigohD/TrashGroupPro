using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrashStats : MonoBehaviour {

	// Use this for initialization
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
				if (health > 0) {
						health -= incDmg;
						gameObject.GetComponent<DamageBarrel> ().barrelHit ();

						if (type.Equals ("Barrel"))
								barrelAudio.Play ();

						if (health <= 0) {
								print ("health zero, time to delete");
								List<NetworkViewID> banana = gameObject.GetComponent<ChildList> ().get ();
								int counter = 0;
								foreach (NetworkViewID childID in banana) {
										Debug.Log ("damaging: " + gameObject);
										GameObject child = NetworkView.Find(childID).gameObject;
										counter++;
										if (child != null) {
												if (child.GetComponent<TrashType> ().type.Equals("Trash"))
														child.GetComponent<TrashStats> ().takeDamage (10000.0f);
												if (child.GetComponent<TrashType> ().type.Equals("Turret"))
														child.GetComponent<TurretStats> ().damageTaken (10000.0f);



										}
										if (counter > 100)
												return;
										Debug.Log ("Loop iteration: " + counter);
								}
								Network.Destroy (this.gameObject);
								Network.Instantiate (explosion, transform.position, transform.rotation, 0);	
						}
		
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
	
}
